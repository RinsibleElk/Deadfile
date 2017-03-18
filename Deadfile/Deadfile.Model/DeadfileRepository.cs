using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Reporting;

namespace Deadfile.Model
{
    public sealed class DeadfileRepository : IDeadfileRepository
    {
        private readonly IRandomNumberGenerator _rng;
        private readonly IModelEntityMapper _modelEntityMapper;

        private static readonly QuotationModel _emptyQuotationModel = new QuotationModel
        {
            Author = "Oliver Samson",
            Phrase = "No Quotations defined. Soz."
        };

        public DeadfileRepository(IModelEntityMapper modelEntityMapper, IRandomNumberGenerator rng)
        {
            _modelEntityMapper = modelEntityMapper;
            _rng = rng;
        }

        private static string _connectionString = null;
        public static string UserId = "NotMyName";
        public static string Password = "ObviouslyNotARealPassword";
        public static string ServerName = @".\SQLEXPRESS";
        public static string DatabaseName = "Deadfile";
        private static bool MakeConnectionString()
        {
            var sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName,
                InitialCatalog = DatabaseName,
                IntegratedSecurity = false,
                Authentication = SqlAuthenticationMethod.SqlPassword,
                UserID = UserId,
                Password = Password,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };
            var providerString = sqlBuilder.ToString();
            try
            {
                using (var context = new DeadfileContext(providerString))
                {
                    var ids = context.Clients.Select((c) => c.ClientId).ToArray();
                    _connectionString = providerString;
                }
            }
            catch (Exception e)
            {
                _connectionString = null;
            }
            return !String.IsNullOrEmpty(_connectionString);
        }

        public static bool RebuildConnectionString()
        {
            return MakeConnectionString();
        }

        private static bool HasConnectionString()
        {
            if (String.IsNullOrEmpty(_connectionString))
                return MakeConnectionString();
            return true;
        }

        public IEnumerable<ClientModel> GetClients()
        {
            if (!HasConnectionString())
                return new ClientModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<ClientModel>();
                foreach (var client in (from client in dbContext.Clients
                    select client))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ClientModel>(client));
                }
                return li;
            }
        }

        public IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId, string filter)
        {
            if (!HasConnectionString())
                return new ApplicationModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<ApplicationModel>();
                foreach (var application in (from application in dbContext.Applications
                    where application.JobId == jobId
                    where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                    orderby application.CreationDate
                    select application))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ApplicationModel>(application));
                }
                return li;
            }
        }

        public IEnumerable<BillableHourModel> GetBillableHoursForJob(int jobId, string filter)
        {
            if (!HasConnectionString())
                return new BillableHourModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<BillableHourModel>();
                foreach (var billableHour in (from billableHour in dbContext.BillableHours
                    where billableHour.JobId == jobId
                    select billableHour))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<BillableHourModel>(billableHour));
                }
                return li;
            }
        }

        public IEnumerable<ExpenseModel> GetExpensesForJob(int jobId, string filter)
        {
            if (!HasConnectionString())
                return new ExpenseModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<ExpenseModel>();
                foreach (var expense in (from expense in dbContext.Expenses
                    where expense.JobId == jobId
                    select expense))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ExpenseModel>(expense));
                }
                return li;
            }
        }

        public IEnumerable<LocalAuthorityModel> GetLocalAuthorities(string filter)
        {
            if (!HasConnectionString())
                return new LocalAuthorityModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<LocalAuthorityModel>();
                foreach (var localAuthority in (from localAuthority in dbContext.LocalAuthorities
                    where (filter == null || filter == "" || localAuthority.Name.Contains(filter))
                    orderby localAuthority.Name
                    select localAuthority))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<LocalAuthorityModel>(localAuthority));
                }
                return li;
            }
        }

        public IEnumerable<ClientModel> GetFilteredClients(string filter)
        {
            if (!HasConnectionString())
                return new ClientModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<ClientModel>();
                foreach (var client in (from client in dbContext.Clients
                    where (client.FirstName + " " + client.LastName).Contains(filter)
                    select client))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ClientModel>(client));
                }
                return li;
            }
        }

        /// <summary>
        /// Based on the settings, do a very specific filter and sort.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEnumerable<BrowserModel> GetBrowserItems(BrowserSettings settings)
        {
            if (!HasConnectionString())
                return new BrowserModel[0];
            switch (settings.Mode)
            {
                case BrowserMode.Client:
                    var clients = new List<BrowserClient>();
                    using (var dbContext = new DeadfileContext(_connectionString))
                    {
                        foreach (var client in (from client in dbContext.Clients
                            where
                            ((settings.FilterText == null || settings.FilterText == "") ||
                                ((client.FirstName == null || client.FirstName == "")
                                        ? client.Title + " " + client.LastName
                                        : client.FirstName + " " + client.LastName).Contains(settings.FilterText))
                            where
                            ((settings.IncludeInactiveEnabled) || client.Status == ClientStatus.Active)
                            orderby
                            ((settings.Sort == BrowserSort.ClientFirstName)
                                ? ((client.FirstName == null || client.FirstName == "")
                                    ? client.Title
                                    : client.FirstName)
                                : client.LastName)
                            select
                            new BrowserClient()
                            {
                                Id = client.ClientId,
                                FullName =
                                ((client.FirstName == null || client.FirstName == "")
                                    ? ((client.Title == null || client.Title == "") ? client.LastName : (client.Title + " " + client.LastName))
                                    : client.FirstName + " " + client.LastName),
                                Status = client.Status
                            }))
                        {
                            client.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            clients.Add(client);
                        }
                    }
                    return clients;
                case BrowserMode.Job:
                    var jobs = new List<BrowserJob>();
                    using (var dbContext = new DeadfileContext(_connectionString))
                    {
                        foreach (var job in (from job in dbContext.Jobs
                            where
                            ((settings.FilterText == null || settings.FilterText == "") ||
                             job.AddressFirstLine.Contains(settings.FilterText))
                            where
                            ((settings.IncludeInactiveEnabled) || job.Status == JobStatus.Active)
                            orderby job.AddressFirstLine
                            select
                            new BrowserJob()
                            {
                                Id = job.JobId,
                                ParentId = job.ClientId,
                                FullAddress = job.AddressFirstLine,
                                Status = job.Status
                            }))
                        {
                            job.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            jobs.Add(job);
                        }
                    }
                    return jobs;
                case BrowserMode.Invoice:
                    var invoices = new List<BrowserInvoice>();
                    using (var dbContext = new DeadfileContext(_connectionString))
                    {
                        foreach (var invoice in (from invoice in dbContext.Invoices
                            where
                            ((  settings.FilterText == null ||
                                settings.FilterText == "") ||
                                invoice.InvoiceReference.ToString().Contains(settings.FilterText) ||
                                invoice.ClientName.Contains(settings.FilterText) ||
                                invoice.Project.Contains(settings.FilterText))
                            where
                            ((settings.IncludeInactiveEnabled) || invoice.Status == InvoiceStatus.Created)
                            orderby
                            (settings.Sort == BrowserSort.InvoiceCreationDate
                                ? -(invoice.CreatedDate.Year*10000 + invoice.CreatedDate.Month*100 +
                                    invoice.CreatedDate.Day)
                                : invoice.InvoiceReference)
                            select
                            new BrowserInvoice()
                            {
                                Id = invoice.InvoiceId,
                                ParentId = invoice.ClientId,
                                InvoiceReference = invoice.InvoiceReference,
                                Status = invoice.Status,
                                ClientName = invoice.ClientName,
                                Project = invoice.Project
                            }))
                        {
                            invoice.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            invoices.Add(invoice);
                        }
                    }
                    return invoices;
                default:
                    return new List<BrowserModel>();
            }
        }

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(BrowserMode mode, bool includeInactiveEnabled,
            int clientId)
        {
            var li = new List<BrowserJob>();
            if (!HasConnectionString())
                return li;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var job in (from job in dbContext.Jobs
                    where job.ClientId == clientId
                    where (includeInactiveEnabled || job.Status == JobStatus.Active)
                    select
                    new BrowserJob()
                    {
                        Id = job.JobId,
                        ParentId = clientId,
                        FullAddress =
                            job.AddressFirstLine +
                            ((job.AddressSecondLine == null || job.AddressSecondLine == "")
                                ? ""
                                : ", " + job.AddressSecondLine) +
                            ((job.AddressThirdLine == null || job.AddressThirdLine == "")
                                ? ""
                                : ", " + job.AddressThirdLine) +
                            ((job.AddressPostCode == null || job.AddressPostCode == "")
                                ? ""
                                : ", " + job.AddressPostCode),
                        Status = job.Status
                    }))
                {
                    job.SetRepository(mode, includeInactiveEnabled, false, this);
                    li.Add(job);
                }
            }
            return li;
        }

        public IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(BrowserMode mode, bool includeInactiveEnabled,
            int jobId)
        {
            var invoiceIdSet = new HashSet<int>();
            var li = new List<BrowserInvoice>();
            if (!HasConnectionString())
                return li;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var invoiceId in (from billable in dbContext.Expenses
                    where billable.InvoiceId.HasValue
                    where billable.JobId == jobId
                    select billable.InvoiceId.Value))
                {
                    invoiceIdSet.Add(invoiceId);
                }
                foreach (var invoiceId in (from billable in dbContext.BillableHours
                    where billable.InvoiceId.HasValue
                    where billable.JobId == jobId
                    select billable.InvoiceId.Value))
                {
                    invoiceIdSet.Add(invoiceId);
                }
                foreach (var invoiceId in invoiceIdSet)
                {
                    var invoiceEntity = dbContext.Invoices.Find(new object[1] {invoiceId});
                    var invoice =
                        new BrowserInvoice()
                        {
                            InvoiceReference = invoiceEntity.InvoiceReference,
                            Id = invoiceEntity.InvoiceId,
                            ParentId = invoiceEntity.ClientId,
                            Status = invoiceEntity.Status,
                            ClientName = invoiceEntity.ClientName,
                            Project = invoiceEntity.Project
                        };
                    invoice.SetRepository(mode, includeInactiveEnabled, true, this);
                    li.Add(invoice);
                }
            }
            return li;
        }

        public IEnumerable<QuotationModel> GetQuotations(string filterText)
        {
            var li = new List<QuotationModel>();
            if (!HasConnectionString())
                return li;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var quotation in (from quotation in dbContext.Quotations
                                           where (filterText == null || quotation.Author.Contains(filterText) || quotation.Phrase.Contains(filterText))
                                           select quotation))
                {
                    var quotationModel = _modelEntityMapper.Mapper.Map<QuotationModel>(quotation);
                    li.Add(quotationModel);
                }
            }
            return li;
        }

        public QuotationModel GetRandomQuotation()
        {
            if (!HasConnectionString())
                return new QuotationModel {Author = "Rinsible Elk", Phrase = "Please Log In"};
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var quotations = (from quotation in dbContext.Quotations select quotation).ToArray();
                if (quotations.Length == 0)
                    return _emptyQuotationModel;
                var index = _rng.Next(quotations.Length);
                return _modelEntityMapper.Mapper.Map<QuotationModel>(quotations[index]);
            }
        }

        public ClientModel GetClientById(int clientId)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var client = dbContext.Clients.Find(new object[1] {clientId});
                return _modelEntityMapper.Mapper.Map<ClientModel>(client);
            }
        }

        public JobModel GetJobById(int jobId)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var job = dbContext.Jobs.Find(new object[1] {jobId});
                return _modelEntityMapper.Mapper.Map<JobModel>(job);
            }
        }

        public InvoiceModel GetInvoiceById(int invoiceId)
        {
            // Get the invoice model.
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            InvoiceModel invoiceModel;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var invoice = dbContext.Invoices.Find(invoiceId);
                invoiceModel = _modelEntityMapper.Mapper.Map<InvoiceModel>(invoice);
            }

            // Get the active items.
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var invoiceItem in (from invoiceItem in dbContext.InvoiceItems
                    where invoiceItem.InvoiceId == invoiceId
                    select invoiceItem))
                {
                    invoiceModel.ChildrenList.Add(_modelEntityMapper.Mapper.Map<InvoiceItemModel>(invoiceItem));
                }
            }
            invoiceModel.ChildrenUpdated();
            return invoiceModel;
        }

        public InvoiceModel GetFirstActiveInvoiceForClient(int clientId)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var activeInvoice = (from invoice in dbContext.Invoices
                                     where invoice.ClientId == clientId
                                     where invoice.Status == InvoiceStatus.Created
                                     orderby invoice.CreatedDate
                                     select invoice).FirstOrDefault();
                if (activeInvoice == null) return null;
                return _modelEntityMapper.Mapper.Map<InvoiceModel>(activeInvoice);
            }
        }

        /// <summary>
        /// Save changes to a client (or add a new one, or 'delete' one).
        /// </summary>
        /// <param name="clientModel"></param>
        public void SaveClient(ClientModel clientModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            try
            {
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    Client client;
                    if (clientModel.ClientId == ModelBase.NewModelId)
                    {
                        // Add
                        client = _modelEntityMapper.Mapper.Map<ClientModel, Client>(clientModel);
                        dbContext.Clients.Add(client);
                    }
                    else
                    {
                        // Edit
                        client = dbContext.Clients.Find(clientModel.ClientId);
                        _modelEntityMapper.Mapper.Map<ClientModel, Client>(clientModel, client);
                    }
                    dbContext.SaveChanges();
                    clientModel.ClientId = client.ClientId;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var dbEntityValidationResult in e.EntityValidationErrors)
                {
                    foreach (DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors)
                    {
                        
                    }
                }
                throw;
            }
        }

        /// <summary>
        /// Save changes to an invoice (or add a new one, or 'delete' one).
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <param name="billableJobs"></param>
        public void SaveInvoice(InvoiceModel invoiceModel, IEnumerable<BillableJob> billableJobs)
        {
            int invoiceId;
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (invoiceModel.InvoiceId == ModelBase.NewModelId)
            {
                // Add
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    var invoice = _modelEntityMapper.Mapper.Map<InvoiceModel, Invoice>(invoiceModel);
                    dbContext.Invoices.Add(invoice);
                    dbContext.SaveChanges();
                    invoiceId = invoice.InvoiceId;
                }

                // Now I'm rather hoping that the invoiceId has been updated.
                if (invoiceId == ModelBase.NewModelId)
                    throw new ApplicationException("The invoice id has not been updated");
                invoiceModel.InvoiceId = invoiceId;
            }
            else
            {
                invoiceId = invoiceModel.InvoiceId;
                // Edit
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    var invoice = dbContext.Invoices.Find(new object[1] {invoiceModel.InvoiceId});
                    _modelEntityMapper.Mapper.Map<InvoiceModel, Invoice>(invoiceModel, invoice);
                    dbContext.SaveChanges();
                }
            }

            // Save or add each of the billable items under the billable jobs, setting the InvoiceId where needed.
            foreach (var billableJob in billableJobs)
            {
                foreach (var billable in billableJob.Children)
                {
                    if (billable.State == BillableModelState.FullyIncluded)
                    {
                        SetInvoiceForBillable(billable, invoiceId, invoiceModel.Status);
                    }
                    else if (billable.State == BillableModelState.Excluded)
                    {
                        SetInvoiceForBillable(billable, null, invoiceModel.Status);
                    }
                }
            }

            // Finally add/edit the active invoice items.
            foreach (var invoiceItemModel in invoiceModel.ChildrenList)
            {
                invoiceItemModel.InvoiceId = invoiceId;
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    if (invoiceItemModel.InvoiceItemId == ModelBase.NewModelId)
                    {
                        if (!invoiceItemModel.DeletePending)
                            dbContext.InvoiceItems.Add(_modelEntityMapper.Mapper.Map<InvoiceItemModel, InvoiceItem>(invoiceItemModel));
                    }
                    else
                    {
                        var invoiceItem = dbContext.InvoiceItems.Find(new object[1] {invoiceItemModel.InvoiceItemId});
                        if (invoiceItemModel.DeletePending)
                        {
                            dbContext.InvoiceItems.Remove(invoiceItem);
                        }
                        else
                        {
                            _modelEntityMapper.Mapper.Map<InvoiceItemModel, InvoiceItem>(invoiceItemModel, invoiceItem);
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        private void SetInvoiceForBillable(BillableModel billableModel, int? invoiceId, InvoiceStatus invoiceStatus)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (billableModel.ModelType == BillableModelType.Expense)
            {
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    var billable = dbContext.Expenses.Find(new object[] { billableModel.Id });
                    billable.InvoiceId = invoiceId;
                    if (invoiceId == null)
                        billable.State = BillableState.Active;
                    else
                    {
                        switch (invoiceStatus)
                        {
                            case InvoiceStatus.Created:
                                billable.State = BillableState.Billed;
                                break;
                            case InvoiceStatus.Cancelled:
                                billable.State = BillableState.Cancelled;
                                break;
                            case InvoiceStatus.Paid:
                                billable.State = BillableState.Paid;
                                break;
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
            else if (billableModel.ModelType == BillableModelType.BillableHour)
            {
                using (var dbContext = new DeadfileContext(_connectionString))
                {
                    var billable = dbContext.BillableHours.Find(new object[] { billableModel.Id });
                    billable.InvoiceId = invoiceId;
                    if (invoiceId == null)
                        billable.State = BillableState.Active;
                    else
                    {
                        switch (invoiceStatus)
                        {
                            case InvoiceStatus.Created:
                                billable.State = BillableState.Billed;
                                break;
                            case InvoiceStatus.Cancelled:
                                billable.State = BillableState.Cancelled;
                                break;
                            case InvoiceStatus.Paid:
                                billable.State = BillableState.Paid;
                                break;
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public bool HasUniqueInvoiceReference(InvoiceModel invoiceModel, int invoiceReference)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (invoiceReference == 0) return invoiceModel.Status == InvoiceStatus.Cancelled;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                return !(from invoice in dbContext.Invoices
                           where invoice.Company == invoiceModel.Company
                           where invoice.InvoiceReference == invoiceReference
                           where
                               (invoiceModel.InvoiceId == ModelBase.NewModelId ||
                                invoiceModel.InvoiceId != invoice.InvoiceId)
                           select invoice.InvoiceId).Any();
            }
        }

        public int[] GetSuggestedInvoiceReferenceIdsForCompany(Company company)
        {
            if (!HasConnectionString())
                return new int[0];
            List<int> usedInvoiceIds;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                usedInvoiceIds = new List<int>(from invoice in dbContext.Invoices
                    where invoice.Company == company
                    where invoice.InvoiceReference != 0
                    orderby invoice.InvoiceReference
                    select invoice.InvoiceReference);
            }
            if (usedInvoiceIds.Count == 0)
                return new int[] {1};
            var lastInvoiceId = 0;
            var highest = usedInvoiceIds[usedInvoiceIds.Count - 1];
            var highestPlus1 = highest + 1;
            var unusedInvoiceIds = new List<int>();
            foreach (var usedInvoiceId in usedInvoiceIds)
            {
                if (lastInvoiceId != 0)
                {
                    if (usedInvoiceId - lastInvoiceId > 1)
                    {
                        for (int i = lastInvoiceId + 1; i < usedInvoiceId; i++)
                        {
                            unusedInvoiceIds.Add(i);
                            if (unusedInvoiceIds.Count >= 2) break;
                        }
                    }
                }
                lastInvoiceId = usedInvoiceId;
                if (unusedInvoiceIds.Count >= 2) break;
            }
            return unusedInvoiceIds.Concat(new int[] {highestPlus1}).ToArray();
        }

        public int GetNextSuggestedJobNumber()
        {
            if (!HasConnectionString())
                return 1;
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var jobNumber = dbContext.Jobs.OrderByDescending((j) => j.JobNumber).FirstOrDefault()?.JobNumber;
                return jobNumber + 1 ?? 1;
            }
        }

        public IEnumerable<JobTaskModel> GetJobTasksForJob(int jobId, string filter)
        {
            if (!HasConnectionString())
                return new JobTaskModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var li = new List<JobTaskModel>();
                foreach (var jobTask in (from jobTask in dbContext.JobTasks
                                         where jobTask.JobId == jobId
                                         where (jobTask.Description == null || filter == null || jobTask.Description.Contains(filter))
                                         join job in dbContext.Jobs on jobTask.JobId equals job.JobId
                                         join client in dbContext.Clients on jobTask.ClientId equals client.ClientId
                                         orderby jobTask.DueDate descending
                                         select
                                            new {
                                                FullName =
                                                    ((client.FirstName == null || client.FirstName == "")
                                                        ? ((client.Title == null || client.Title == "") ? client.LastName : (client.Title + " " + client.LastName))
                                                        : client.FirstName + " " + client.LastName),
                                                Property = job.AddressFirstLine,
                                                JobTask = jobTask
                                            }))
                {
                    var jobTaskModel = _modelEntityMapper.Mapper.Map<JobTaskModel>(jobTask.JobTask);
                    jobTaskModel.ClientFullName = jobTask.FullName;
                    jobTaskModel.Property = jobTask.Property;
                    li.Add(jobTaskModel);
                }
                return li;
            }
        }

        public void SaveJobTask(JobTaskModel jobTaskModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                if (jobTaskModel.JobTaskId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.JobTasks.Add(_modelEntityMapper.Mapper.Map<JobTaskModel, JobTask>(jobTaskModel));
                }
                else
                {
                    // Edit
                    var jobTask = dbContext.JobTasks.Find(jobTaskModel.JobTaskId);
                    _modelEntityMapper.Mapper.Map<JobTaskModel, JobTask>(jobTaskModel, jobTask);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveQuotation(QuotationModel quotationModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                Quotation quotation;
                if (quotationModel.QuotationId == ModelBase.NewModelId)
                {
                    // Add
                    quotation = _modelEntityMapper.Mapper.Map<QuotationModel, Quotation>(quotationModel);
                    dbContext.Quotations.Add(quotation);
                }
                else
                {
                    // Edit
                    quotation = dbContext.Quotations.Find(quotationModel.QuotationId);
                    _modelEntityMapper.Mapper.Map<QuotationModel, Quotation>(quotationModel, quotation);
                }
                dbContext.SaveChanges();
                quotationModel.QuotationId = quotation.QuotationId;
            }
        }

        public IEnumerable<UnbilledJobModel> GetUnbilledJobs(string filterText)
        {
            if (!HasConnectionString())
                return new UnbilledJobModel[0];
            var d = new Dictionary<int, UnbilledJobModel>();
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var expense in (from expense in dbContext.Expenses
                                         where expense.Job.Status == JobStatus.Active
                                         where (filterText == null || filterText == "" || expense.Job.AddressFirstLine.Contains(filterText))
                                         where expense.State == BillableState.Active
                                         where expense.NetAmount > 0
                                         select new
                                         {
                                             Billable = expense,
                                             Job = expense.Job,
                                             Client = expense.Job.Client
                                         }))
                {
                    var billable = expense.Billable;
                    if ((billable.State == BillableState.Active) && (billable.NetAmount > 0))
                    {
                        var job = expense.Job;
                        var client = expense.Client;
                        if (d.ContainsKey(job.JobId))
                        {
                            var model = d[job.JobId];
                            d[job.JobId] = new UnbilledJobModel
                            {
                                ClientId = model.ClientId,
                                JobId = model.JobId,
                                FullName = model.FullName,
                                AddressFirstLine = model.AddressFirstLine,
                                UnbilledAmount = model.UnbilledAmount + billable.NetAmount
                            };
                        }
                        else
                        {
                            var fullName = (String.IsNullOrWhiteSpace(client.FirstName))
                                ? ((String.IsNullOrWhiteSpace(client.Title))
                                    ? client.LastName
                                    : $"{client.Title} {client.LastName}")
                                : $"{client.FirstName} {client.LastName}";
                            d.Add(job.JobId, new UnbilledJobModel
                            {
                                ClientId = job.Client.ClientId,
                                JobId = job.JobId,
                                FullName = fullName,
                                AddressFirstLine = job.AddressFirstLine,
                                UnbilledAmount = billable.NetAmount
                            });
                        }
                    }
                }
                foreach (var expense in (from expense in dbContext.BillableHours
                                         where expense.Job.Status == JobStatus.Active
                                         where (filterText == null || filterText == "" || expense.Job.AddressFirstLine.Contains(filterText))
                                         where expense.State == BillableState.Active
                                         where expense.NetAmount > 0
                                         select new
                                         {
                                             Billable = expense,
                                             Job = expense.Job,
                                             Client = expense.Job.Client
                                         }))
                {
                    var billable = expense.Billable;
                    if ((billable.State == BillableState.Active) && (billable.NetAmount > 0))
                    {
                        var job = expense.Job;
                        var client = expense.Client;
                        if (d.ContainsKey(job.JobId))
                        {
                            var model = d[job.JobId];
                            d[job.JobId] = new UnbilledJobModel
                            {
                                ClientId = model.ClientId,
                                JobId = model.JobId,
                                FullName = model.FullName,
                                AddressFirstLine = model.AddressFirstLine,
                                UnbilledAmount = model.UnbilledAmount + billable.NetAmount
                            };
                        }
                        else
                        {
                            var fullName = (String.IsNullOrWhiteSpace(client.FirstName))
                                ? ((String.IsNullOrWhiteSpace(client.Title))
                                    ? client.LastName
                                    : $"{client.Title} {client.LastName}")
                                : $"{client.FirstName} {client.LastName}";
                            d.Add(job.JobId, new UnbilledJobModel
                            {
                                ClientId = job.Client.ClientId,
                                JobId = job.JobId,
                                FullName = fullName,
                                AddressFirstLine = job.AddressFirstLine,
                                UnbilledAmount = billable.NetAmount
                            });
                        }
                    }
                }
            }
            return d.Values.OrderByDescending((s) => s.UnbilledAmount).ToArray();
        }

        public IEnumerable<JobTaskModel> GetJobTasks(DateTime startDate, DateTime endDate, string filter, bool includeInactive)
        {
            var li = new List<JobTaskModel>();
            if (!HasConnectionString())
                return new JobTaskModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var jobTask in (from jobTask in dbContext.JobTasks
                                         where (includeInactive || jobTask.State == JobTaskState.Active)
                                         where (jobTask.State == JobTaskState.Active || jobTask.DueDate >= startDate)
                                         where jobTask.DueDate <= endDate
                                         where (filter == null || filter == "" || jobTask.Description.Contains(filter))
                                         join job in dbContext.Jobs on jobTask.JobId equals job.JobId
                                         join client in dbContext.Clients on jobTask.ClientId equals client.ClientId
                                         orderby (jobTask.DueDate)
                                         select
                                            new
                                            {
                                                FullName =
                                                    ((client.FirstName == null || client.FirstName == "")
                                                        ? ((client.Title == null || client.Title == "") ? client.LastName : (client.Title + " " + client.LastName))
                                                        : client.FirstName + " " + client.LastName),
                                                Property = job.AddressFirstLine,
                                                JobTask = jobTask
                                            }))
                {
                    var jobTaskModel = _modelEntityMapper.Mapper.Map<JobTaskModel>(jobTask.JobTask);
                    jobTaskModel.ClientFullName = jobTask.FullName;
                    jobTaskModel.Property = jobTask.Property;
                    li.Add(jobTaskModel);
                }
            }
            return li;
        }

        public void DeleteJobTask(JobTaskModel jobTaskModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var jobTask = dbContext.JobTasks.Find(new object[1] {jobTaskModel.Id});
                dbContext.JobTasks.Remove(jobTask);
                dbContext.SaveChanges();
            }
        }

        public void DeleteExpense(ExpenseModel expenseModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var expense = dbContext.Expenses.Find(new object[1] { expenseModel.Id });
                dbContext.Expenses.Remove(expense);
                dbContext.SaveChanges();
            }
        }

        public void DeleteLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var localAuthority = dbContext.LocalAuthorities.Find(new object[1] { localAuthorityModel.Id });
                dbContext.LocalAuthorities.Remove(localAuthority);
                dbContext.SaveChanges();
            }
        }

        public void DeleteQuotation(QuotationModel quotationModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var quotation = dbContext.Quotations.Find(new object[1] { quotationModel.Id });
                dbContext.Quotations.Remove(quotation);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<InvoiceModel> GetUnpaidInvoices(string filter)
        {
            var li = new List<InvoiceModel>();
            if (!HasConnectionString())
                return new InvoiceModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var invoice in (from invoice in dbContext.Invoices
                                         where invoice.Status == InvoiceStatus.Created
                                         where (filter == null || filter == "" || invoice.InvoiceReference.ToString().Contains(filter))
                                         select invoice))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<Invoice, InvoiceModel>(invoice));
                }
            }
            return li;
        }

        public IEnumerable<CurrentApplicationModel> GetCurrentApplications(string filter)
        {
            var li = new List<CurrentApplicationModel>();
            if (!HasConnectionString())
                return new CurrentApplicationModel[0];
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var application in (from application in dbContext.Applications
                                             where application.State == ApplicationState.Current
                                             where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                                             orderby application.EstimatedDecisionDate descending
                                             join job in dbContext.Jobs on application.JobId equals job.JobId
                                             select new {Job = job, Application = application}))
                {
                    var currentApplicationModel = new CurrentApplicationModel();
                    _modelEntityMapper.Mapper.Map(application.Application, currentApplicationModel);
                    currentApplicationModel.JobAddressFirstLine = application.Job.AddressFirstLine;
                    li.Add(currentApplicationModel);
                }
            }
            return li;
        }

        /// <summary>
        /// The current connection string.
        /// </summary>
        public string ConnectionString => _connectionString;

        public void SaveLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                if (localAuthorityModel.LocalAuthorityId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.LocalAuthorities.Add(_modelEntityMapper.Mapper.Map<LocalAuthorityModel, LocalAuthority>(localAuthorityModel));
                }
                else
                {
                    // Edit
                    var localAuthority = dbContext.LocalAuthorities.Find(localAuthorityModel.LocalAuthorityId);
                    _modelEntityMapper.Mapper.Map<LocalAuthorityModel, LocalAuthority>(localAuthorityModel, localAuthority);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveApplication(ApplicationModel applicationModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                if (applicationModel.ApplicationId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.Applications.Add(_modelEntityMapper.Mapper.Map<ApplicationModel, Application>(applicationModel));
                }
                else
                {
                    // Edit
                    var application = dbContext.Applications.Find(applicationModel.ApplicationId);
                    _modelEntityMapper.Mapper.Map<ApplicationModel, Application>(applicationModel, application);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveExpense(ExpenseModel expenseModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                if (expenseModel.ExpenseId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.Expenses.Add(_modelEntityMapper.Mapper.Map<ExpenseModel, Expense>(expenseModel));
                }
                else
                {
                    // Edit
                    var expense = dbContext.Expenses.Find(expenseModel.ExpenseId);
                    _modelEntityMapper.Mapper.Map<ExpenseModel, Expense>(expenseModel, expense);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveBillableHour(BillableHourModel billableHourModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                if (billableHourModel.BillableHourId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.BillableHours.Add(_modelEntityMapper.Mapper.Map<BillableHourModel, BillableHour>(billableHourModel));
                }
                else
                {
                    // Edit
                    var billableHour = dbContext.BillableHours.Find(billableHourModel.BillableHourId);
                    _modelEntityMapper.Mapper.Map<BillableHourModel, BillableHour>(billableHourModel, billableHour);
                }
                dbContext.SaveChanges();
            }
        }

        public BrowserModel GetBrowserClientById(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                var client = dbContext.Clients.Find(new object[1] { clientId });
                var model = new BrowserClient()
                    {
                        Id = client.ClientId,
                        FullName =
                                ((client.FirstName == null || client.FirstName == "")
                                    ? ((client.Title == null || client.Title == "") ? client.LastName : (client.Title + " " + client.LastName))
                                    : client.FirstName + " " + client.LastName),
                        Status = client.Status
                    };
                model.SetRepository(mode, includeInactiveEnabled, true, this);
                return model;
            }
        }

        public IEnumerable<BrowserModel> GetBrowserJobsForInvoice(BrowserMode mode, bool includeInactiveEnabled, int invoiceId)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            var jobIdSet = new HashSet<int>();
            var li = new List<BrowserJob>();
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var jobId in (from billable in dbContext.Expenses
                                       where billable.InvoiceId.HasValue
                                       where billable.InvoiceId.Value == invoiceId
                                       select billable.JobId))
                {
                    jobIdSet.Add(jobId);
                }
                foreach (var jobId in (from billable in dbContext.BillableHours
                                       where billable.InvoiceId.HasValue
                                       where billable.InvoiceId.Value == invoiceId
                                       select billable.JobId))
                {
                    jobIdSet.Add(jobId);
                }
                foreach (var jobId in jobIdSet)
                {
                    var jobEntity = dbContext.Jobs.Find(new object[1] {jobId});
                    var job =
                        new BrowserJob()
                        {
                            Id = jobEntity.JobId,
                            ParentId = jobEntity.ClientId,
                            FullAddress =
                                jobEntity.AddressFirstLine +
                                ((jobEntity.AddressSecondLine == null || jobEntity.AddressSecondLine == "")
                                    ? ""
                                    : ", " + jobEntity.AddressSecondLine) +
                                ((jobEntity.AddressThirdLine == null || jobEntity.AddressThirdLine == "")
                                    ? ""
                                    : ", " + jobEntity.AddressThirdLine) +
                                ((jobEntity.AddressPostCode == null || jobEntity.AddressPostCode == "")
                                    ? ""
                                    : ", " + jobEntity.AddressPostCode)
                        };
                    job.SetRepository(mode, includeInactiveEnabled, true, this);
                    li.Add(job);
                }
            }
            return li;
        }

        private static void AddBillableToJob(BillableJob job,
            BillableModel billable,
            ref bool hasClaimed,
            ref bool hasExcluded,
            ref bool hasIncluded,
            ref double totalAmount,
            ref double includedAmount)
        {
            switch (billable.State)
            {
                case BillableModelState.Claimed:
                    hasClaimed = true;
                    break;
                case BillableModelState.Excluded:
                    hasExcluded = true;
                    totalAmount += billable.NetAmount;
                    break;
                default:
                    hasIncluded = true;
                    includedAmount += billable.NetAmount;
                    totalAmount += billable.NetAmount;
                    break;
            }
            job.Children.Add(billable);
        }

        /// <summary>
        /// Save changes to a job (or add a new one, or 'delete' one).
        /// </summary>
        /// <param name="jobModel"></param>
        public void SaveJob(JobModel jobModel)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                Job job;
                if (jobModel.JobId == ModelBase.NewModelId)
                {
                    // Add
                    job = _modelEntityMapper.Mapper.Map<JobModel, Job>(jobModel);
                    dbContext.Jobs.Add(job);
                }
                else
                {
                    // Edit
                    job = dbContext.Jobs.Find(jobModel.JobId);
                    _modelEntityMapper.Mapper.Map<JobModel, Job>(jobModel, job);
                }
                dbContext.SaveChanges();
                jobModel.JobId = job.JobId;
            }
        }

        /// <summary>
        /// Get the billable models for a client. Attribute them by state based on <see cref="invoiceId"/>.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public IEnumerable<BillableModel> GetBillableModelsForClientAndInvoice(int clientId, int invoiceId)
        {
            var li2 = new List<BillableJob>();
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var job in (from job in dbContext.Jobs
                    where job.ClientId == clientId
                    select new BillableJob()
                    {
                        JobId = job.JobId,
                        FullAddress = job.AddressFirstLine
                    }))
                {
                    li2.Add(job);
                }
            }

            var li = new List<BillableModel>();
            foreach (var job in li2)
            {
                var hasClaimed = false;
                var hasIncluded = false;
                var hasExcluded = false;
                double includedAmount = 0;
                double totalAmount = 0;

                // Find all expenses for this job.
                AddExpensesForJob(invoiceId, job, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount, ref includedAmount);

                // Find all billable hours for this job.
                AddBillableHoursForJob(invoiceId, job, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount, ref includedAmount);

                // Calculate the job state based on the billable items that are included.
                if (hasIncluded)
                {
                    // Don't care about claimed if there are any excluded or included.
                    job.State = hasExcluded
                        ? BillableModelState.PartiallyIncluded
                        : BillableModelState.FullyIncluded;
                }
                else if (hasClaimed)
                {
                    job.State = hasExcluded ? BillableModelState.Excluded : BillableModelState.Claimed;
                }
                else
                {
                    // Includes the case where there are not any billables at all.
                    job.State = BillableModelState.Excluded;
                }
                job.NetAmount = includedAmount;
                job.TotalPossibleNetAmount = totalAmount;
                li.Add(job);
            }
            return li;
        }

        private static void AddBillableHoursForJob(int invoiceId, BillableJob job,
            ref bool hasClaimed, ref bool hasExcluded, ref bool hasIncluded, ref double totalAmount,
            ref double includedAmount)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var billableHour in (from billableHour in dbContext.BillableHours
                    where billableHour.JobId == job.JobId
                    select new BillableBillableHour()
                    {
                        NetAmount = billableHour.NetAmount,
                        Description = billableHour.Description,
                        State =
                            (invoiceId == ModelBase.NewModelId)
                                ? (billableHour.InvoiceId == null
                                    ? BillableModelState.Excluded
                                    : BillableModelState.Claimed)
                                : (billableHour.InvoiceId == null
                                    ? BillableModelState.Excluded
                                    : (billableHour.InvoiceId.Value == invoiceId
                                        ? BillableModelState.FullyIncluded
                                        : BillableModelState.Claimed)),
                        BillableHourId = billableHour.BillableHourId
                    }))
                {
                    AddBillableToJob(job, billableHour, ref hasClaimed, ref hasExcluded, ref hasIncluded,
                        ref totalAmount,
                        ref includedAmount);
                }
            }
        }

        private static void AddExpensesForJob(int invoiceId, BillableJob job, ref bool hasClaimed,
            ref bool hasExcluded, ref bool hasIncluded, ref double totalAmount, ref double includedAmount)
        {
            if (!HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = new DeadfileContext(_connectionString))
            {
                foreach (var expense in (from expense in dbContext.Expenses
                    where expense.JobId == job.JobId
                    select new BillableExpense()
                    {
                        NetAmount = expense.NetAmount,
                        Description = expense.Description,
                        State =
                            (invoiceId == ModelBase.NewModelId)
                                ? (expense.InvoiceId == null ? BillableModelState.Excluded : BillableModelState.Claimed)
                                : (expense.InvoiceId == null
                                    ? BillableModelState.Excluded
                                    : (expense.InvoiceId.Value == invoiceId
                                        ? BillableModelState.FullyIncluded
                                        : BillableModelState.Claimed)),
                        ExpenseId = expense.ExpenseId
                    }))
                {
                    AddBillableToJob(job, expense, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount,
                        ref includedAmount);
                }
            }
        }
    }
}
