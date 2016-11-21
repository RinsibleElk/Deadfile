using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public sealed class DeadfileRepository : IDeadfileRepository
    {
        private readonly IRandomNumberGenerator _rng;
        private readonly IModelEntityMapper _modelEntityMapper;

        public DeadfileRepository(IModelEntityMapper modelEntityMapper, IRandomNumberGenerator rng)
        {
            _modelEntityMapper = modelEntityMapper;
            _rng = rng;
        }

        public IEnumerable<ClientModel> GetClients()
        {
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            switch (settings.Mode)
            {
                case BrowserMode.Client:
                    var clients = new List<BrowserClient>();
                    using (var dbContext = new DeadfileContext())
                    {
                        foreach (var client in (from client in dbContext.Clients
                                                where
                                                    ((settings.FilterText == null || settings.FilterText == "" || client.FirstName == null ||
                                                      client.FirstName == "")
                                                        ? client.Title + " " + client.LastName
                                                        : client.FirstName + " " + client.LastName).Contains(settings.FilterText)
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
                                                            ? client.Title + " " + client.LastName
                                                            : client.FirstName + " " + client.LastName)
                                                    }))
                        {
                            client.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            clients.Add(client);
                        }
                    }
                    return clients;
                case BrowserMode.Job:
                    var jobs = new List<BrowserJob>();
                    using (var dbContext = new DeadfileContext())
                    {
                        foreach (var job in (from job in dbContext.Jobs
                                             where ((settings.FilterText == null || settings.FilterText == "") || job.AddressFirstLine.Contains(settings.FilterText))
                                             orderby job.AddressFirstLine
                                             select
                                                new BrowserJob()
                                                {
                                                    Id = job.JobId,
                                                    ParentId = job.ClientId,
                                                    FullAddress = job.AddressFirstLine
                                                }))
                        {
                            job.SetRepository(settings.Mode, settings.IncludeInactiveEnabled, false, this);
                            jobs.Add(job);
                        }
                    }
                    return jobs;
                case BrowserMode.Invoice:
                    var invoices = new List<BrowserInvoice>();
                    using (var dbContext = new DeadfileContext())
                    {
                        foreach (var invoice in (from invoice in dbContext.Invoices
                                                 where ((settings.FilterText == null || settings.FilterText == "") || invoice.InvoiceReference.ToString().StartsWith(settings.FilterText))
                                                 orderby (settings.Sort == BrowserSort.InvoiceCreationDate ? -(invoice.CreatedDate.Year * 10000 + invoice.CreatedDate.Month * 100 + invoice.CreatedDate.Day) : invoice.InvoiceReference)
                                                 select
                                                    new BrowserInvoice()
                                                    {
                                                        Id = invoice.InvoiceId,
                                                        ParentId = invoice.ClientId,
                                                        InvoiceReference = invoice.InvoiceReference
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

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            var li = new List<BrowserJob>();
            using (var dbContext = new DeadfileContext())
            {
                foreach (var job in (from job in dbContext.Jobs
                    where job.ClientId == clientId
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
                                : ", " + job.AddressPostCode)
                    }))
                {
                    job.SetRepository(mode, includeInactiveEnabled, false, this);
                    li.Add(job);
                }
            }
            return li;
        }

        public IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(BrowserMode mode, bool includeInactiveEnabled, int jobId)
        {
            var invoiceIdSet = new HashSet<int>();
            var li = new List<BrowserInvoice>();
            using (var dbContext = new DeadfileContext())
            {
                foreach (var invoiceId in (from billable in dbContext.Applications
                                           where billable.InvoiceId.HasValue
                                           where billable.JobId == jobId
                                           select billable.InvoiceId.Value))
                {
                    invoiceIdSet.Add(invoiceId);
                }
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
                        };
                    invoice.SetRepository(mode, includeInactiveEnabled, true, this);
                    li.Add(invoice);
                }
            }
            return li;
        }

        public void SetUpFakeData()
        {
            var addInvoices = false;
            using (var dbContext = new DeadfileContext())
            {
                if ((from client in dbContext.Clients select client).FirstOrDefault() == null)
                {
                    addInvoices = true;
                    foreach (var clientModel in FakeData.GetFakeClients())
                    {
                        dbContext.Clients.Add(_modelEntityMapper.Mapper.Map<Client>(clientModel));
                    }
                    FakeData.AddFakeQuotations(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeJobs(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeLocalAuthorities(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeApplications(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeExpenses(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeBillableHours(dbContext);
                    dbContext.SaveChanges();
                }
            }
            if (addInvoices)
                FakeData.AddFakeInvoices();
        }

        public QuotationModel GetRandomQuotation()
        {
            using (var dbContext = new DeadfileContext())
            {
                var quotations = (from quotation in dbContext.Quotations select quotation).ToArray();
                var index = _rng.Next(quotations.Length);
                return _modelEntityMapper.Mapper.Map<QuotationModel>(quotations[index]);
            }
        }

        public ClientModel GetClientById(int clientId)
        {
            using (var dbContext = new DeadfileContext())
            {
                var client = dbContext.Clients.Find(new object[1] { clientId });
                return _modelEntityMapper.Mapper.Map<ClientModel>(client);
            }
        }

        public JobModel GetJobById(int jobId)
        {
            using (var dbContext = new DeadfileContext())
            {
                var job = dbContext.Jobs.Find(new object[1] { jobId });
                return _modelEntityMapper.Mapper.Map<JobModel>(job);
            }
        }

        public InvoiceModel GetInvoiceById(int invoiceId)
        {
            // Get the invoice model.
            InvoiceModel invoiceModel;
            using (var dbContext = new DeadfileContext())
            {
                var invoice = dbContext.Invoices.Find(invoiceId);
                invoiceModel = _modelEntityMapper.Mapper.Map<InvoiceModel>(invoice);
            }

            // Get the active items.
            using (var dbContext = new DeadfileContext())
            {
                foreach (var invoiceItem in (from invoiceItem in dbContext.InvoiceItems
                                             where invoiceItem.InvoiceId == invoiceId
                                             select invoiceItem))
                {
                    invoiceModel.InvoiceItemModels.Add(_modelEntityMapper.Mapper.Map<InvoiceItemModel>(invoiceItem));
                }
            }

            return invoiceModel;
        }

        public void SaveClient(ClientModel clientModel)
        {
            using (var dbContext = new DeadfileContext())
            {
                if (clientModel.ClientId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.Clients.Add(_modelEntityMapper.Mapper.Map<ClientModel, Client>(clientModel));
                }
                else
                {
                    // Edit
                    var client = dbContext.Clients.Find(clientModel.ClientId);
                    _modelEntityMapper.Mapper.Map<ClientModel, Client>(clientModel, client);
                }
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Save changes to an invoice (or add a new one).
        /// </summary>
        /// <param name="invoiceModel"></param>
        public void SaveInvoice(InvoiceModel invoiceModel)
        {
            if (invoiceModel.InvoiceId == ModelBase.NewModelId)
            {
                // Add
                int invoiceId;
                using (var dbContext = new DeadfileContext())
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
                // Edit
                using (var dbContext = new DeadfileContext())
                {
                    var invoice = dbContext.Invoices.Find(new object[1] {invoiceModel.InvoiceId});
                    _modelEntityMapper.Mapper.Map<InvoiceModel, Invoice>(invoiceModel, invoice);
                    dbContext.SaveChanges();
                }
            }

            // Finally add/edit the active invoice items.
            foreach (var invoiceItemModel in invoiceModel.InvoiceItemModels)
            {
                using (var dbContext = new DeadfileContext())
                {
                    if (invoiceItemModel.InvoiceItemId == ModelBase.NewModelId)
                    {
                        if (!invoiceItemModel.MarkedForDeletion)
                            dbContext.InvoiceItems.Add(_modelEntityMapper.Mapper.Map<InvoiceItemModel, InvoiceItem>(invoiceItemModel));
                    }
                    else
                    {
                        var invoiceItem = dbContext.InvoiceItems.Find(new object[1] {invoiceItemModel.InvoiceItemId});
                        if (invoiceItemModel.MarkedForDeletion)
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

        public void SaveLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
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
            using (var dbContext = new DeadfileContext())
            {
                var client = dbContext.Clients.Find(new object[1] { clientId });
                var model = new BrowserClient()
                    {
                        Id = client.ClientId,
                        FullName =
                        ((client.FirstName == null || client.FirstName == "")
                            ? client.Title + " " + client.LastName
                            : client.FirstName + " " + client.LastName)
                    };
                model.SetRepository(mode, includeInactiveEnabled, true, this);
                return model;
            }
        }

        public IEnumerable<BrowserModel> GetBrowserJobsForInvoice(BrowserMode mode, bool includeInactiveEnabled, int invoiceId)
        {
            var jobIdSet = new HashSet<int>();
            var li = new List<BrowserJob>();
            using (var dbContext = new DeadfileContext())
            {
                foreach (var jobId in (from billable in dbContext.Applications
                                       where billable.InvoiceId.HasValue
                                       where billable.InvoiceId.Value == invoiceId
                                       select billable.JobId))
                {
                    jobIdSet.Add(jobId);
                }
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

        public void SaveJob(JobModel jobModel)
        {
            using (var dbContext = new DeadfileContext())
            {
                if (jobModel.JobId == ModelBase.NewModelId)
                {
                    // Add
                    dbContext.Jobs.Add(_modelEntityMapper.Mapper.Map<JobModel, Job>(jobModel));
                }
                else
                {
                    // Edit
                    var job = dbContext.Jobs.Find(jobModel.JobId);
                    _modelEntityMapper.Mapper.Map<JobModel, Job>(jobModel, job);
                }
                dbContext.SaveChanges();
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
            using (var dbContext = new DeadfileContext())
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

                // Find all applications for this job.
                AddApplicationsForJob(invoiceId, job, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount, ref includedAmount);

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
            using (var dbContext = new DeadfileContext())
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
                                        : BillableModelState.Claimed))
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
            using (var dbContext = new DeadfileContext())
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
                                        : BillableModelState.Claimed))
                    }))
                {
                    AddBillableToJob(job, expense, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount,
                        ref includedAmount);
                }
            }
        }

        private static void AddApplicationsForJob(int invoiceId, BillableJob job, ref bool hasClaimed,
            ref bool hasExcluded, ref bool hasIncluded, ref double totalAmount, ref double includedAmount)
        {
            using (var dbContext = new DeadfileContext())
            {
                foreach (var application in (from application in dbContext.Applications
                    where application.JobId == job.JobId
                    select new BillableApplication()
                    {
                        ApplicationId = application.ApplicationId,
                        LocalAuthorityReference = application.LocalAuthorityReference,
                        NetAmount = application.NetAmount,
                        State =
                            (invoiceId == ModelBase.NewModelId)
                                ? (application.InvoiceId == null
                                    ? BillableModelState.Excluded
                                    : BillableModelState.Claimed)
                                : (application.InvoiceId == null
                                    ? BillableModelState.Excluded
                                    : (application.InvoiceId.Value == invoiceId
                                        ? BillableModelState.FullyIncluded
                                        : BillableModelState.Claimed))
                    }))
                {
                    AddBillableToJob(job, application, ref hasClaimed, ref hasExcluded, ref hasIncluded, ref totalAmount,
                        ref includedAmount);
                }
            }
        }
    }
}
