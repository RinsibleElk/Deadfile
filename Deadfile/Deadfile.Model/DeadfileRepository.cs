using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly IDeadfileContextAbstractionFactory _deadfileContextAbstractionFactory;
        private readonly IModelEntityMapper _modelEntityMapper;

        private static readonly QuotationModel EmptyQuotationModel = new QuotationModel
        {
            Author = "Oliver Samson",
            Phrase = "No Quotations defined. Soz."
        };

        public DeadfileRepository(IModelEntityMapper modelEntityMapper, IRandomNumberGenerator rng, IDeadfileContextAbstractionFactory deadfileContextAbstractionFactory)
        {
            _modelEntityMapper = modelEntityMapper;
            _rng = rng;
            _deadfileContextAbstractionFactory = deadfileContextAbstractionFactory;
        }

        public IEnumerable<ClientModel> GetClients()
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new ClientModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<ClientModel>();
                foreach (var client in dbContext.GetClients(null))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ClientModel>(client));
                }
                return li;
            }
        }

        public IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId, string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new ApplicationModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<ApplicationModel>();
                foreach (var application in dbContext.GetApplicationsForJob(jobId, filter))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ApplicationModel>(application));
                }
                return li;
            }
        }

        public IEnumerable<BillableHourModel> GetBillableHoursForJob(int jobId, string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new BillableHourModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<BillableHourModel>();
                foreach (var billableHour in dbContext.GetBillableHoursForJob(jobId, filter))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<BillableHourModel>(billableHour));
                }
                return li;
            }
        }

        public IEnumerable<ExpenseModel> GetExpensesForJob(int jobId, string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new ExpenseModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<ExpenseModel>();
                foreach (var expense in dbContext.GetExpensesForJob(jobId, filter))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ExpenseModel>(expense));
                }
                return li;
            }
        }

        public IEnumerable<LocalAuthorityModel> GetLocalAuthorities(string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new LocalAuthorityModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<LocalAuthorityModel>();
                foreach (var localAuthority in dbContext.GetLocalAuthorities(filter))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<LocalAuthorityModel>(localAuthority));
                }
                return li;
            }
        }

        public IEnumerable<ClientModel> GetFilteredClients(string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new ClientModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<ClientModel>();
                foreach (var client in dbContext.GetClients(filter))
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new BrowserModel[0];
            switch (settings.Mode)
            {
                case BrowserMode.Client:
                    var clients = new List<BrowserClient>();
                    using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                    {
                        foreach (var client in
                        (from client in dbContext.GetOrderedClients(settings)
                            select
                            new BrowserClient()
                            {
                                Id = client.ClientId,
                                FullName =
                                ((client.FirstName == null || client.FirstName == "")
                                    ? ((client.Title == null || client.Title == "")
                                        ? client.LastName
                                        : (client.Title + " " + client.LastName))
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
                    using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                    {
                        foreach (var job in (from job in dbContext.GetOrderedJobs(ModelBase.NewModelId, settings)
                            select
                            new BrowserJob
                            {
                                Id = job.JobId,
                                ParentId = job.ClientId,
                                FullAddress = job.AddressFirstLine,
                                Status = job.Status,
                                JobNumber = job.JobNumber
                            }))
                        {
                            job.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            jobs.Add(job);
                        }
                    }
                    return jobs;
                case BrowserMode.Invoice:
                    var invoices = new List<BrowserInvoice>();
                    using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                    {
                        foreach (var invoice in (from invoice in dbContext.GetOrderedInvoices(settings)
                            select
                            new BrowserInvoice()
                            {
                                Id = invoice.InvoiceId,
                                ParentId = invoice.ClientId,
                                InvoiceReference = invoice.InvoiceReference,
                                Status = invoice.Status,
                                ClientName = invoice.ClientName,
                                Project = invoice.Project,
                                CreatedDate = invoice.CreatedDate
                            }))
                        {
                            invoice.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            invoices.Add(invoice);
                        }
                    }
                    invoices.Sort(GetInvoiceComparer(settings.Sort));
                    return invoices;
                default:
                    return new List<BrowserModel>();
            }
        }

        private class BrowserInvoiceByInvoiceAddressComparer : IComparer<BrowserInvoice>
        {
            private static Regex addressAndNumber = new Regex(@"^(([0-9]*)([A-Za-z]?)\s+)?([^,]+)(,|$)");

            private readonly Dictionary<string, Tuple<int, string, string>> _cache =
                new Dictionary<string, Tuple<int, string, string>>();

            private void GetResultsFromCache(string project, out int number, out string letter, out string road)
            {
                number = Int32.MinValue;
                letter = null;
                road = "";
                Tuple<int, string, string> cacheResult;
                if (!_cache.TryGetValue(project, out cacheResult))
                {
                    var xProject = addressAndNumber.Match(project);
                    if (xProject.Success)
                    {
                        if (xProject.Groups[2].Success && xProject.Groups[2].Value.Length > 0) number = Int32.Parse(xProject.Groups[2].Value);
                        if (xProject.Groups[3].Success) letter = xProject.Groups[3].Value;
                        if (xProject.Groups[4].Success) road = xProject.Groups[4].Value;
                    }
                    else
                    {
                        road = project;
                    }
                    _cache.Add(project, new Tuple<int, string, string>(number, letter, road));
                }
                else
                {
                    number = cacheResult.Item1;
                    letter = cacheResult.Item2;
                    road = cacheResult.Item3;
                }

            }

            public int Compare(BrowserInvoice x, BrowserInvoice y)
            {
                if (x.Project == y.Project) return 0;
                int xNumber;
                string xLetter;
                string xRoad;
                int yNumber;
                string yLetter;
                string yRoad;
                GetResultsFromCache(x.Project, out xNumber, out xLetter, out xRoad);
                GetResultsFromCache(y.Project, out yNumber, out yLetter, out yRoad);
                var roadComp = String.Compare(xRoad, yRoad, StringComparison.InvariantCultureIgnoreCase);
                if (roadComp != 0) return roadComp;
                if (xNumber != yNumber) return xNumber.CompareTo(yNumber);
                return String.Compare(xLetter, yLetter, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private class BrowserInvoiceByReferenceComparer : IComparer<BrowserInvoice>
        {
            public int Compare(BrowserInvoice x, BrowserInvoice y)
            {
                return x.InvoiceReference.CompareTo(y.InvoiceReference);
            }
        }

        private class BrowserInvoiceByClientNameComparer : IComparer<BrowserInvoice>
        {
            public int Compare(BrowserInvoice x, BrowserInvoice y)
            {
                return String.Compare(x.ClientName, y.ClientName, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private class BrowserInvoiceByCreatedDateComparer : IComparer<BrowserInvoice>
        {
            public int Compare(BrowserInvoice x, BrowserInvoice y)
            {
                return y.CreatedDate.CompareTo(x.CreatedDate);
            }
        }

        private IComparer<BrowserInvoice> GetInvoiceComparer(BrowserSort settingsSort)
        {
            switch (settingsSort)
            {
                case BrowserSort.InvoiceAddress:
                    return new BrowserInvoiceByInvoiceAddressComparer();
                case BrowserSort.InvoiceReference:
                    return new BrowserInvoiceByReferenceComparer();
                case BrowserSort.InvoiceClient:
                    return new BrowserInvoiceByClientNameComparer();
                default: // Invoice CreatedDate
                    return new BrowserInvoiceByCreatedDateComparer();
            }
        }

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            var li = new List<BrowserJob>();
            if (!DeadfileContextAbstraction.HasConnectionString())
                return li;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var job in (from job in dbContext.GetOrderedJobs(clientId, new BrowserSettings { FilterText=null, IncludeInactiveEnabled=includeInactiveEnabled,Mode=mode, Sort=BrowserSort.ClientFirstName})
                    select
                    new BrowserJob
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
                        Status = job.Status,
                        JobNumber = job.JobNumber
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                return li;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var invoiceId in dbContext.GetInvoiceIdsForJobExpenses(jobId))
                {
                    invoiceIdSet.Add(invoiceId);
                }
                foreach (var invoiceId in dbContext.GetInvoiceIdsForJobBillableHours(jobId))
                {
                    invoiceIdSet.Add(invoiceId);
                }
                foreach (var invoiceId in invoiceIdSet)
                {
                    var invoiceEntity = dbContext.GetInvoiceById(invoiceId);
                    var invoice =
                        new BrowserInvoice()
                        {
                            InvoiceReference = invoiceEntity.InvoiceReference,
                            Id = invoiceEntity.InvoiceId,
                            ParentId = invoiceEntity.ClientId,
                            Status = invoiceEntity.Status,
                            ClientName = invoiceEntity.ClientName,
                            Project = invoiceEntity.Project,
                            CreatedDate = invoiceEntity.CreatedDate
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                return li;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var quotation in dbContext.GetQuotations(filterText))
                {
                    var quotationModel = _modelEntityMapper.Mapper.Map<QuotationModel>(quotation);
                    li.Add(quotationModel);
                }
            }
            return li;
        }

        public QuotationModel GetRandomQuotation()
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new QuotationModel {Author = "Rinsible Elk", Phrase = "Please Log In"};
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var quotations = (dbContext.GetQuotations(null)).ToArray();
                if (quotations.Length == 0)
                    return EmptyQuotationModel;
                var index = _rng.Next(quotations.Length);
                return _modelEntityMapper.Mapper.Map<QuotationModel>(quotations[index]);
            }
        }

        public ClientModel GetClientById(int clientId)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var client = dbContext.GetClientById(clientId);
                return _modelEntityMapper.Mapper.Map<ClientModel>(client);
            }
        }

        public JobModel GetJobById(int jobId)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var job = dbContext.GetJobById(jobId);
                return _modelEntityMapper.Mapper.Map<JobModel>(job);
            }
        }

        public InvoiceModel GetInvoiceById(int invoiceId)
        {
            // Get the invoice model.
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            InvoiceModel invoiceModel;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var invoice = dbContext.GetInvoiceById(invoiceId);
                invoiceModel = _modelEntityMapper.Mapper.Map<InvoiceModel>(invoice);
            }

            // Get the active items.
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var invoiceItem in dbContext.GetInvoiceItems(invoiceId))
                {
                    invoiceModel.ChildrenList.Add(_modelEntityMapper.Mapper.Map<InvoiceItemModel>(invoiceItem));
                }
            }
            invoiceModel.ChildrenUpdated();
            return invoiceModel;
        }

        public InvoiceModel GetFirstActiveInvoiceForClient(int clientId)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var activeInvoice = dbContext.GetFirstActiveInvoiceForClient(clientId);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            try
            {
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    Client client;
                    if (clientModel.ClientId == ModelBase.NewModelId)
                    {
                        // Add
                        client = _modelEntityMapper.Mapper.Map<ClientModel, Client>(clientModel);
                        dbContext.AddClient(client);
                    }
                    else
                    {
                        // Edit
                        client = dbContext.GetClientById(clientModel.ClientId);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (invoiceModel.InvoiceId == ModelBase.NewModelId)
            {
                // Add
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    var invoice = _modelEntityMapper.Mapper.Map<InvoiceModel, Invoice>(invoiceModel);
                    dbContext.AddInvoice(invoice);
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
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    var invoice = dbContext.GetInvoiceById(invoiceModel.InvoiceId);
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
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    if (invoiceItemModel.InvoiceItemId == ModelBase.NewModelId)
                    {
                        if (!invoiceItemModel.DeletePending)
                            dbContext.AddInvoiceItem(_modelEntityMapper.Mapper.Map<InvoiceItemModel, InvoiceItem>(invoiceItemModel));
                    }
                    else
                    {
                        var invoiceItem = dbContext.GetInvoiceItemById(invoiceItemModel.InvoiceItemId);
                        if (invoiceItemModel.DeletePending)
                        {
                            dbContext.RemoveInvoiceItem(invoiceItem);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (billableModel.ModelType == BillableModelType.Expense)
            {
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    var billable = dbContext.GetExpenseById(billableModel.Id);
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
                using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
                {
                    var billable = dbContext.GetBillableHourById(billableModel.Id);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            if (invoiceReference == 0) return invoiceModel.Status == InvoiceStatus.Cancelled;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                return dbContext.HasUniqueInvoiceReference(invoiceModel.Company, invoiceModel.InvoiceId,
                    invoiceModel.InvoiceReference);
            }
        }

        public int[] GetSuggestedInvoiceReferenceIdsForCompany(Company company)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new int[0];
            List<int> usedInvoiceIds;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                usedInvoiceIds = new List<int>(dbContext.GetUsedInvoiceReferencesForCompany(company));
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                return 1;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var jobNumber = dbContext.GetJobs(null).OrderByDescending((j) => j.JobNumber).FirstOrDefault()?.JobNumber;
                return jobNumber + 1 ?? 1;
            }
        }

        public IEnumerable<JobTaskModel> GetJobTasksForJob(int jobId, string filter)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new JobTaskModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var li = new List<JobTaskModel>();
                foreach (var jobTask in dbContext.GetJobTasksWithPropertiesForJob(jobId, filter))
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                if (jobTaskModel.JobTaskId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.AddJobTask(_modelEntityMapper.Mapper.Map<JobTaskModel, JobTask>(jobTaskModel));
                }
                else
                {
                    // Edit
                    var jobTask = dbContext.GetJobTaskById(jobTaskModel.JobTaskId);
                    _modelEntityMapper.Mapper.Map<JobTaskModel, JobTask>(jobTaskModel, jobTask);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveQuotation(QuotationModel quotationModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                Quotation quotation;
                if (quotationModel.QuotationId == ModelBase.NewModelId)
                {
                    // Add
                    quotation = _modelEntityMapper.Mapper.Map<QuotationModel, Quotation>(quotationModel);
                    dbContext.AddQuotation(quotation);
                }
                else
                {
                    // Edit
                    quotation = dbContext.GetQuotationById(quotationModel.QuotationId);
                    _modelEntityMapper.Mapper.Map<QuotationModel, Quotation>(quotationModel, quotation);
                }
                dbContext.SaveChanges();
                quotationModel.QuotationId = quotation.QuotationId;
            }
        }

        public IEnumerable<UnbilledJobModel> GetUnbilledJobs(string filterText)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new UnbilledJobModel[0];
            var d = new Dictionary<int, UnbilledJobModel>();
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var expense in dbContext.GetActiveExpenses(filterText))
                {
                    var billable = expense.Expense;
                    if ((billable.State == BillableState.Active) && (billable.NetAmount > 0))
                    {
                        var job = expense.Job;
                        var client = expense.Client;
                        if (d.ContainsKey(job.JobId))
                        {
                            var model = d[job.JobId];
                            model.UnbilledAmount += billable.NetAmount;
                            model.Items.Add(_modelEntityMapper.Mapper.Map<UnbilledItemModel>(billable));
                        }
                        else
                        {
                            var fullName = (String.IsNullOrWhiteSpace(client.FirstName))
                                ? ((String.IsNullOrWhiteSpace(client.Title))
                                    ? client.LastName
                                    : $"{client.Title} {client.LastName}")
                                : $"{client.FirstName} {client.LastName}";
                            var model =
                                new UnbilledJobModel
                                {
                                    ClientId = job.Client.ClientId,
                                    JobId = job.JobId,
                                    FullName = fullName,
                                    AddressFirstLine = job.AddressFirstLine,
                                    JobNumber = job.JobNumber,
                                    UnbilledHours = 0,
                                    UnbilledAmount = billable.NetAmount
                                };
                            model.Items.Add(_modelEntityMapper.Mapper.Map<UnbilledItemModel>(billable));
                            d.Add(job.JobId, model);
                        }
                    }
                }
                foreach (var expense in dbContext.GetActiveBillableHours(filterText))
                {
                    var billable = expense.BillableHour;
                    if ((billable.State == BillableState.Active) && (billable.HoursWorked > 0))
                    {
                        var job = expense.Job;
                        var client = expense.Client;
                        if (d.ContainsKey(job.JobId))
                        {
                            var model = d[job.JobId];
                            model.UnbilledHours += billable.HoursWorked;
                            model.Items.Add(_modelEntityMapper.Mapper.Map<UnbilledItemModel>(billable));
                        }
                        else
                        {
                            var fullName = (String.IsNullOrWhiteSpace(client.FirstName))
                                ? ((String.IsNullOrWhiteSpace(client.Title))
                                    ? client.LastName
                                    : $"{client.Title} {client.LastName}")
                                : $"{client.FirstName} {client.LastName}";
                            var model = new UnbilledJobModel
                            {
                                ClientId = job.Client.ClientId,
                                JobId = job.JobId,
                                FullName = fullName,
                                AddressFirstLine = job.AddressFirstLine,
                                JobNumber = job.JobNumber,
                                UnbilledHours = billable.HoursWorked,
                                UnbilledAmount = 0
                            };
                            d.Add(job.JobId, model);
                            model.Items.Add(_modelEntityMapper.Mapper.Map<UnbilledItemModel>(billable));
                        }
                    }
                }
            }
            return d.Values.OrderByDescending((s) => s.UnbilledAmount).ToArray();
        }

        public IEnumerable<JobTaskModel> GetJobTasks(DateTime startDate, DateTime endDate, string filter, bool includeInactive)
        {
            var li = new List<JobTaskModel>();
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new JobTaskModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var jobTask in dbContext.GetJobTasksWithPropertiesForDateRange(startDate, endDate, filter, includeInactive))
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var jobTask = dbContext.GetJobTaskById(jobTaskModel.Id);
                dbContext.RemoveJobTask(jobTask);
                dbContext.SaveChanges();
            }
        }

        public void DeleteExpense(ExpenseModel expenseModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var expense = dbContext.GetExpenseById(expenseModel.Id);
                dbContext.RemoveExpense(expense);
                dbContext.SaveChanges();
            }
        }

        public void DeleteLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var localAuthority = dbContext.GetLocalAuthorityById(localAuthorityModel.Id);
                dbContext.RemoveLocalAuthority(localAuthority);
                dbContext.SaveChanges();
            }
        }

        public void DeleteQuotation(QuotationModel quotationModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var quotation = dbContext.GetQuotationById(quotationModel.Id);
                dbContext.RemoveQuotation(quotation);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<CurrentApplicationModel> GetCurrentApplications(string filter, bool includeInactive)
        {
            var li = new List<CurrentApplicationModel>();
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new CurrentApplicationModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var application in dbContext.GetApplicationsWithJobs(filter, includeInactive))
                {
                    var currentApplicationModel = new CurrentApplicationModel();
                    _modelEntityMapper.Mapper.Map(application.Application, currentApplicationModel);
                    currentApplicationModel.JobAddressFirstLine = application.Job.AddressFirstLine;
                    li.Add(currentApplicationModel);
                }
            }
            return li;
        }

        public int MakeJobTaskBillable(JobTaskModel jobTaskModel)
        {
            var billableHourModel = new BillableHourModel
            {
                State = BillableState.Active,
                CreationDate = DateTime.Today,
                Description = jobTaskModel.Description,
                HoursWorked = 0,
                NetAmount = 0,
                Notes = jobTaskModel.Notes,
                JobId = jobTaskModel.JobId,
                ClientId = jobTaskModel.ClientId,
                InvoiceId = null
            };
            DeleteJobTask(jobTaskModel);
            SaveBillableHour(billableHourModel);
            return billableHourModel.BillableHourId;
        }

        public IEnumerable<InvoiceModel> GetInvoices(Company? company, DateTime startDate, DateTime endDate, string filter, bool includeInactive)
        {
            var li = new List<InvoiceModel>();
            if (!DeadfileContextAbstraction.HasConnectionString())
                return new InvoiceModel[0];
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                li.AddRange(from invoice in dbContext.GetOrderedInvoices(new BrowserSettings { FilterText = filter, IncludeInactiveEnabled = includeInactive }) where company == null || company.Value.Equals(invoice.Company) where startDate <= invoice.CreatedDate where endDate >= invoice.CreatedDate select _modelEntityMapper.Mapper.Map<Invoice, InvoiceModel>(invoice));
            }
            return li;
        }

        public void SaveLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                if (localAuthorityModel.LocalAuthorityId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.AddLocalAuthority(_modelEntityMapper.Mapper.Map<LocalAuthorityModel, LocalAuthority>(localAuthorityModel));
                }
                else
                {
                    // Edit
                    var localAuthority = dbContext.GetLocalAuthorityById(localAuthorityModel.LocalAuthorityId);
                    _modelEntityMapper.Mapper.Map<LocalAuthorityModel, LocalAuthority>(localAuthorityModel, localAuthority);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveApplication(ApplicationModel applicationModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                if (applicationModel.ApplicationId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.AddApplication(_modelEntityMapper.Mapper.Map<ApplicationModel, Application>(applicationModel));
                }
                else
                {
                    // Edit
                    var application = dbContext.GetApplicationById(applicationModel.ApplicationId);
                    _modelEntityMapper.Mapper.Map<ApplicationModel, Application>(applicationModel, application);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveExpense(ExpenseModel expenseModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                if (expenseModel.ExpenseId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.AddExpense(_modelEntityMapper.Mapper.Map<ExpenseModel, Expense>(expenseModel));
                }
                else
                {
                    // Edit
                    var expense = dbContext.GetExpenseById(expenseModel.ExpenseId);
                    _modelEntityMapper.Mapper.Map<ExpenseModel, Expense>(expenseModel, expense);
                }
                dbContext.SaveChanges();
            }
        }

        public void SaveBillableHour(BillableHourModel billableHourModel)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                BillableHour billableHour;
                if (billableHourModel.BillableHourId == ModelBase.NewModelId)
                {
                    // Add
                    billableHour = _modelEntityMapper.Mapper.Map<BillableHourModel, BillableHour>(billableHourModel);
                    dbContext.AddBillableHour(billableHour);
                }
                else
                {
                    // Edit
                    billableHour = dbContext.GetBillableHourById(billableHourModel.BillableHourId);
                    _modelEntityMapper.Mapper.Map<BillableHourModel, BillableHour>(billableHourModel, billableHour);
                }
                dbContext.SaveChanges();
                billableHourModel.BillableHourId = billableHour.BillableHourId;
            }
        }

        public BrowserModel GetBrowserClientById(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                var client = dbContext.GetClientById(clientId);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            var jobIdSet = new HashSet<int>();
            var li = new List<BrowserJob>();
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var jobId in dbContext.GetJobIdsForInvoiceExpenses(invoiceId))
                {
                    jobIdSet.Add(jobId);
                }
                foreach (var jobId in dbContext.GetJobIdsForInvoiceBillableHours(invoiceId))
                {
                    jobIdSet.Add(jobId);
                }
                foreach (var jobId in jobIdSet)
                {
                    var jobEntity = dbContext.GetJobById(jobId);
                    var job =
                        new BrowserJob
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
                                    : ", " + jobEntity.AddressPostCode),
                            Status = jobEntity.Status,
                            JobNumber = jobEntity.JobNumber
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
            ref double totalHours,
            ref double includedHours,
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
                    totalHours += billable.Hours;
                    totalAmount += billable.NetAmount;
                    break;
                default:
                    hasIncluded = true;
                    includedHours += billable.Hours;
                    totalHours += billable.Hours;
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                Job job;
                if (jobModel.JobId == ModelBase.NewModelId)
                {
                    // Add
                    job = _modelEntityMapper.Mapper.Map<JobModel, Job>(jobModel);
                    dbContext.AddJob(job);
                }
                else
                {
                    // Edit
                    job = dbContext.GetJobById(jobModel.JobId);
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
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var job in (from job in dbContext.GetOrderedJobs(clientId, new BrowserSettings())
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
                double includedHours = 0;
                double totalHours = 0;
                double includedAmount = 0;
                double totalAmount = 0;

                // Find all expenses for this job.
                AddExpensesForJob(invoiceId, job, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount, ref includedAmount);

                // Find all billable hours for this job.
                AddBillableHoursForJob(invoiceId, job, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalHours, ref includedHours);

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
                job.Hours = includedHours;
                job.TotalPossibleHours = totalHours;
                li.Add(job);
            }
            return li;
        }

        private void AddBillableHoursForJob(int invoiceId, BillableJob job,
            ref bool hasClaimed, ref bool hasExcluded, ref bool hasIncluded, ref double totalHours,
            ref double includedHours)
        {
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            var totalAmount = 0.0;
            var includedAmount = 0.0;
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var billableHour in (from billableHour in dbContext.GetBillableHoursForJob(job.JobId, null)
                    where billableHour.JobId == job.JobId
                    select new BillableBillableHour()
                    {
                        Hours = billableHour.HoursWorked,
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
                        ref totalHours,
                        ref includedHours,
                        ref totalAmount,
                        ref includedAmount);
                }
            }
        }

        private void AddExpensesForJob(int invoiceId, BillableJob job, ref bool hasClaimed,
            ref bool hasExcluded, ref bool hasIncluded, ref double totalAmount, ref double includedAmount)
        {
            var totalHours = 0.0;
            var includedHours = 0.0;
            if (!DeadfileContextAbstraction.HasConnectionString())
                throw new ApplicationException("Log in corrupted");
            using (var dbContext = _deadfileContextAbstractionFactory.GetAbstraction())
            {
                foreach (var expense in (from expense in dbContext.GetExpensesForJob(job.JobId, null)
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
                    AddBillableToJob(job, expense, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalHours,
                        ref includedHours,
                        ref totalAmount,
                        ref includedAmount);
                }
            }
        }
    }
}
