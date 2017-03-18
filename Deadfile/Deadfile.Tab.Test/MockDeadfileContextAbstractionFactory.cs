using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;

namespace Deadfile.Tab.Test
{
    class MockDeadfileContextAbstractionFactory : IDeadfileContextAbstractionFactory
    {
        public List<Client> Clients = new List<Client>();
        public List<Job> Jobs = new List<Job>();
        public List<Invoice> Invoices = new List<Invoice>();
        public List<InvoiceItem> InvoiceItems = new List<InvoiceItem>();
        public List<Expense> Expenses = new List<Expense>();
        public List<BillableHour> BillableHours = new List<BillableHour>();
        public List<JobTask> JobTasks = new List<JobTask>();
        public List<Quotation> Quotations = new List<Quotation>();
        public List<LocalAuthority> LocalAuthorities = new List<LocalAuthority>();
        public List<Application> Applications = new List<Application>();

        public IDeadfileContextAbstraction GetAbstraction()
        {
            return new MockDeadfileContextAbstraction(this);
        }
    }

    internal class MockDeadfileContextAbstraction : IDeadfileContextAbstraction
    {
        private readonly MockDeadfileContextAbstractionFactory _factory;

        public MockDeadfileContextAbstraction(MockDeadfileContextAbstractionFactory factory)
        {
            _factory = factory;
        }

        public void Dispose()
        {
        }

        public IEnumerable<Application> GetApplicationsForJob(int jobId, string filter)
        {
            return (from application in _factory.Applications
                    where application.JobId == jobId
                    where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                    orderby application.CreationDate
                    select application);
        }

        public IEnumerable<BillableHour> GetBillableHoursForJob(int jobId, string filter)
        {
            return (from billableHour in _factory.BillableHours
                    where billableHour.JobId == jobId
                    select billableHour);
        }

        public IEnumerable<Expense> GetExpensesForJob(int jobId, string filter)
        {
            return (from expense in _factory.Expenses
                    where expense.JobId == jobId
                    select expense);
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities(string filter)
        {
            return (from localAuthority in _factory.LocalAuthorities
                    where (filter == null || filter == "" || localAuthority.Name.Contains(filter))
                    orderby localAuthority.Name
                    select localAuthority);
        }

        public IEnumerable<Client> GetClients(string filter)
        {
            return (from client in _factory.Clients
                    where (filter == null || filter == "" || (client.FirstName + " " + client.LastName).Contains(filter))
                    select client);
        }

        public IEnumerable<Client> GetOrderedClients(BrowserSettings settings)
        {
            return (from client in _factory.Clients
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
                    select client);
        }

        public IEnumerable<Job> GetOrderedJobs(int clientId, BrowserSettings settings)
        {
            return (from job in _factory.Jobs
                    where (clientId == ModelBase.NewModelId || job.ClientId == clientId)
                    where ((settings.FilterText == null || settings.FilterText == "") ||
                         job.AddressFirstLine.Contains(settings.FilterText))
                    where
                    ((settings.IncludeInactiveEnabled) || job.Status == JobStatus.Active)
                    orderby job.AddressFirstLine
                    select job);
        }

        public IEnumerable<Invoice> GetOrderedInvoices(BrowserSettings settings)
        {
            return (from invoice in _factory.Invoices
                    where
                    ((settings.FilterText == null ||
                      settings.FilterText == "") ||
                     invoice.InvoiceReference.ToString().Contains(settings.FilterText) ||
                     invoice.ClientName.Contains(settings.FilterText) ||
                     invoice.Project.Contains(settings.FilterText))
                    where
                    ((settings.IncludeInactiveEnabled) || invoice.Status == InvoiceStatus.Created)
                    select invoice);
        }

        public IEnumerable<int> GetInvoiceIdsForJobExpenses(int jobId)
        {
            return (from billable in _factory.Expenses
                    where billable.InvoiceId.HasValue
                    where billable.JobId == jobId
                    select billable.InvoiceId.Value);
        }

        public IEnumerable<int> GetInvoiceIdsForJobBillableHours(int jobId)
        {
            return (from billable in _factory.BillableHours
                    where billable.InvoiceId.HasValue
                    where billable.JobId == jobId
                    select billable.InvoiceId.Value);
        }

        public IEnumerable<int> GetJobIdsForInvoiceExpenses(int invoiceId)
        {
            return (from billable in _factory.Expenses
                    where billable.InvoiceId.HasValue
                    where billable.InvoiceId.Value == invoiceId
                    select billable.JobId);
        }

        public IEnumerable<int> GetJobIdsForInvoiceBillableHours(int invoiceId)
        {
            return (from billable in _factory.BillableHours
                    where billable.InvoiceId.HasValue
                    where billable.InvoiceId.Value == invoiceId
                    select billable.JobId);
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            return _factory.Invoices.FirstOrDefault((invoice) => invoice.InvoiceId == invoiceId);
        }

        public IEnumerable<Quotation> GetQuotations(string filter)
        {
            return (from quotation in _factory.Quotations
                    where
                    (filter == null || filter == "" || quotation.Author.Contains(filter) || quotation.Phrase.Contains(filter))
                    select quotation);
        }

        public Client GetClientById(int clientId)
        {
            return _factory.Clients.FirstOrDefault((client) => client.ClientId == clientId);
        }

        public Job GetJobById(int jobId)
        {
            return _factory.Jobs.FirstOrDefault((job) => job.JobId == jobId);
        }

        public IEnumerable<InvoiceItem> GetInvoiceItems(int invoiceId)
        {
            return (from invoiceItem in _factory.InvoiceItems
                    where invoiceItem.InvoiceId == invoiceId
                    select invoiceItem);
        }

        public Invoice GetFirstActiveInvoiceForClient(int clientId)
        {
            return (from invoice in _factory.Invoices
                    where invoice.ClientId == clientId
                    where invoice.Status == InvoiceStatus.Created
                    orderby invoice.CreatedDate
                    select invoice).FirstOrDefault();
        }

        public void AddClient(Client client)
        {
            client.ClientId = _factory.Clients.Count;
            _factory.Clients.Add(client);
        }

        public void SaveChanges()
        {
        }

        public void AddInvoice(Invoice invoice)
        {
            invoice.InvoiceId = _factory.Invoices.Count;
            _factory.Invoices.Add(invoice);
        }

        public void AddInvoiceItem(InvoiceItem invoiceItem)
        {
            invoiceItem.InvoiceItemId = _factory.InvoiceItems.Count;
            _factory.InvoiceItems.Add(invoiceItem);
        }

        public InvoiceItem GetInvoiceItemById(int invoiceItemId)
        {
            return _factory.InvoiceItems.FirstOrDefault((invoiceItem) => invoiceItem.InvoiceItemId == invoiceItemId);
        }

        public void RemoveInvoiceItem(InvoiceItem invoiceItem)
        {
            _factory.InvoiceItems.Remove(invoiceItem);
        }

        public Expense GetExpenseById(int expenseId)
        {
            return _factory.Expenses.FirstOrDefault((expense) => expense.ExpenseId == expenseId);
        }

        public BillableHour GetBillableHourById(int billableHourId)
        {
            return _factory.BillableHours.FirstOrDefault((billableHour) => billableHour.BillableHourId == billableHourId);
        }

        public bool HasUniqueInvoiceReference(Company company, int invoiceId, int invoiceReference)
        {
            return !(from invoice in _factory.Invoices
                     where invoice.Company == company
                     where invoice.InvoiceReference == invoiceReference
                     where (invoiceId == ModelBase.NewModelId || invoiceId != invoice.InvoiceId)
                     select invoice.InvoiceId).Any();
        }

        public IEnumerable<int> GetUsedInvoiceReferencesForCompany(Company company)
        {
            return (from invoice in _factory.Invoices
                    where invoice.Company == company
                    where invoice.InvoiceReference != 0
                    orderby invoice.InvoiceReference
                    select invoice.InvoiceReference);
        }

        public IEnumerable<Job> GetJobs(string filter)
        {
            return _factory.Jobs;
        }

        public IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForJob(int jobId, string filter)
        {
            return (from jobTask in _factory.JobTasks
                    where jobTask.JobId == jobId
                    where (jobTask.Description == null || filter == null || jobTask.Description.Contains(filter))
                    join job in _factory.Jobs on jobTask.JobId equals job.JobId
                    join client in _factory.Clients on jobTask.ClientId equals client.ClientId
                    orderby jobTask.DueDate descending
                    select
                    new JobTaskWithProperty
                    {
                        FullName =
                        ((client.FirstName == null || client.FirstName == "")
                            ? ((client.Title == null || client.Title == "")
                                ? client.LastName
                                : (client.Title + " " + client.LastName))
                            : client.FirstName + " " + client.LastName),
                        Property = job.AddressFirstLine,
                        JobTask = jobTask
                    });
        }

        public void AddJobTask(JobTask jobTask)
        {
            jobTask.JobTaskId = _factory.JobTasks.Count;
            _factory.JobTasks.Add(jobTask);
        }

        public JobTask GetJobTaskById(int jobTaskId)
        {
            return _factory.JobTasks.FirstOrDefault((jobTask) => jobTask.JobTaskId == jobTaskId);
        }

        public void AddQuotation(Quotation quotation)
        {
            quotation.QuotationId = _factory.Quotations.Count;
            _factory.Quotations.Add(quotation);
        }

        public Quotation GetQuotationById(int quotationId)
        {
            return _factory.Quotations.FirstOrDefault((quotation) => quotation.QuotationId == quotationId);
        }

        public IEnumerable<ExpenseWithJobAndClient> GetActiveExpenses(string filter)
        {
            return (from expense in _factory.Expenses
                    where expense.Job.Status == JobStatus.Active
                    where (filter == null || filter == "" || expense.Job.AddressFirstLine.Contains(filter))
                    where expense.State == BillableState.Active
                    where expense.NetAmount > 0
                    select new ExpenseWithJobAndClient
                    {
                        Expense = expense,
                        Job = expense.Job,
                        Client = expense.Job.Client
                    });
        }

        public IEnumerable<BillableHourWithJobAndClient> GetActiveBillableHours(string filter)
        {
            return (from expense in _factory.BillableHours
                    where expense.Job.Status == JobStatus.Active
                    where (filter == null || filter == "" || expense.Job.AddressFirstLine.Contains(filter))
                    where expense.State == BillableState.Active
                    where expense.HoursWorked > 0
                    select new BillableHourWithJobAndClient
                    {
                        BillableHour = expense,
                        Job = expense.Job,
                        Client = expense.Job.Client
                    });
        }

        public IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForDateRange(DateTime startDate, DateTime endDate, string filter,
            bool includeInactive)
        {
            return (from jobTask in _factory.JobTasks
                    where (includeInactive || jobTask.State == JobTaskState.Active)
                    where (jobTask.State == JobTaskState.Active || jobTask.DueDate >= startDate)
                    where jobTask.DueDate <= endDate
                    where (filter == null || filter == "" || jobTask.Description.Contains(filter))
                    join job in _factory.Jobs on jobTask.JobId equals job.JobId
                    join client in _factory.Clients on jobTask.ClientId equals client.ClientId
                    orderby (jobTask.DueDate)
                    select
                       new JobTaskWithProperty
                       {
                           FullName =
                               ((client.FirstName == null || client.FirstName == "")
                                   ? ((client.Title == null || client.Title == "") ? client.LastName : (client.Title + " " + client.LastName))
                                   : client.FirstName + " " + client.LastName),
                           Property = job.AddressFirstLine,
                           JobTask = jobTask
                       });
        }

        public void RemoveJobTask(JobTask jobTask)
        {
            _factory.JobTasks.Remove(jobTask);
        }

        public void RemoveExpense(Expense expense)
        {
            _factory.Expenses.Remove(expense);
        }

        public LocalAuthority GetLocalAuthorityById(int localAuthorityId)
        {
            return _factory.LocalAuthorities.FirstOrDefault((localAuthority) => localAuthority.LocalAuthorityId == localAuthorityId);
        }

        public void RemoveLocalAuthority(LocalAuthority localAuthority)
        {
            _factory.LocalAuthorities.Remove(localAuthority);
        }

        public void RemoveQuotation(Quotation quotation)
        {
            _factory.Quotations.Remove(quotation);
        }

        public IEnumerable<ApplicationWithJob> GetApplicationsWithJobs(string filter)
        {
            return (from application in _factory.Applications
                    where application.State == ApplicationState.Current
                    where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                    orderby application.EstimatedDecisionDate descending
                    join job in _factory.Jobs on application.JobId equals job.JobId
                    select new ApplicationWithJob { Job = job, Application = application });
        }

        public void AddLocalAuthority(LocalAuthority localAuthority)
        {
            localAuthority.LocalAuthorityId = _factory.LocalAuthorities.Count;
            _factory.LocalAuthorities.Add(localAuthority);
        }

        public void AddApplication(Application application)
        {
            application.ApplicationId = _factory.Applications.Count;
            _factory.Applications.Add(application);
        }

        public Application GetApplicationById(int applicationId)
        {
            return _factory.Applications.FirstOrDefault((application) => application.ApplicationId == applicationId);
        }

        public void AddExpense(Expense expense)
        {
            expense.ExpenseId = _factory.Expenses.Count;
            _factory.Expenses.Add(expense);
        }

        public void AddBillableHour(BillableHour billableHour)
        {
            billableHour.BillableHourId = _factory.BillableHours.Count;
            _factory.BillableHours.Add(billableHour);
        }

        public void AddJob(Job job)
        {
            job.JobId = _factory.Jobs.Count;
            _factory.Jobs.Add(job);
        }
    }
}
