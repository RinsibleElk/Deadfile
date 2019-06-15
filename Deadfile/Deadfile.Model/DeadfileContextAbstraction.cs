using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public class DeadfileContextAbstraction : IDeadfileContextAbstraction
    {
        private static string _connectionString = null;
        private readonly DeadfileContext _deadfileContext;
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
                IntegratedSecurity = true,
                // Old way of doing this.
                //IntegratedSecurity = false,
                //Authentication = SqlAuthenticationMethod.SqlPassword,
                //UserID = UserId,
                //Password = Password,
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

        public static void TestOnlyFakeConnectionString()
        {
            _connectionString = "Some fake connection string";
        }

        public static bool HasConnectionString()
        {
            if (String.IsNullOrEmpty(_connectionString))
                return MakeConnectionString();
            return true;
        }

        public DeadfileContextAbstraction()
        {
            _deadfileContext = new DeadfileContext(_connectionString);
        }
        public void Dispose()
        {
            _deadfileContext.Dispose();
        }

        public IEnumerable<Client> GetClients(string filter)
        {
            return (from client in _deadfileContext.Clients
                where (filter == null || filter == "" || (client.FirstName + " " + client.LastName).Contains(filter))
                select client);
        }

        public IEnumerable<Client> GetOrderedClients(BrowserSettings settings)
        {
            return (from client in _deadfileContext.Clients
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
            return (from job in _deadfileContext.Jobs
                where (clientId == ModelBase.NewModelId || job.ClientId == clientId)
                where ((settings.FilterText == null || settings.FilterText == "") ||
                     job.AddressFirstLine.Contains(settings.FilterText) || job.JobNumber.ToString().Contains(settings.FilterText))
                where
                ((settings.IncludeInactiveEnabled) || job.Status == JobStatus.Active)
                orderby job.AddressFirstLine
                select job);
        }

        public IEnumerable<Invoice> GetOrderedInvoices(BrowserSettings settings)
        {
            return (from invoice in _deadfileContext.Invoices
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
            return (from billable in _deadfileContext.Expenses
                where billable.InvoiceId.HasValue
                where billable.JobId == jobId
                select billable.InvoiceId.Value);
        }

        public IEnumerable<int> GetInvoiceIdsForJobBillableHours(int jobId)
        {
            return (from billable in _deadfileContext.BillableHours
                where billable.InvoiceId.HasValue
                where billable.JobId == jobId
                select billable.InvoiceId.Value);
        }

        public IEnumerable<int> GetJobIdsForInvoiceExpenses(int invoiceId)
        {
            return (from billable in _deadfileContext.Expenses
                where billable.InvoiceId.HasValue
                where billable.InvoiceId.Value == invoiceId
                select billable.JobId);
        }

        public IEnumerable<int> GetJobIdsForInvoiceBillableHours(int invoiceId)
        {
            return (from billable in _deadfileContext.BillableHours
                where billable.InvoiceId.HasValue
                where billable.InvoiceId.Value == invoiceId
                select billable.JobId);
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            return _deadfileContext.Invoices.Find(new object[1] {invoiceId});
        }

        public IEnumerable<Quotation> GetQuotations(string filter)
        {
            return (from quotation in _deadfileContext.Quotations
                where
                (filter == null || filter == "" || quotation.Author.Contains(filter) || quotation.Phrase.Contains(filter))
                select quotation);
        }

        public Client GetClientById(int clientId)
        {
            return _deadfileContext.Clients.Find(new object[1] {clientId});
        }

        public Job GetJobById(int jobId)
        {
            return _deadfileContext.Jobs.Find(new object[1] {jobId});
        }

        public IEnumerable<InvoiceItem> GetInvoiceItems(int invoiceId)
        {
            return (from invoiceItem in _deadfileContext.InvoiceItems
                where invoiceItem.InvoiceId == invoiceId
                select invoiceItem);
        }

        public Invoice GetFirstActiveInvoiceForClient(int clientId)
        {
            return (from invoice in _deadfileContext.Invoices
                where invoice.ClientId == clientId
                where invoice.Status == InvoiceStatus.Created
                orderby invoice.CreatedDate
                select invoice).FirstOrDefault();
        }

        public void AddClient(Client client)
        {
            _deadfileContext.Clients.Add(client);
        }

        public void SaveChanges()
        {
            _deadfileContext.SaveChanges();
        }

        public void AddInvoice(Invoice invoice)
        {
            _deadfileContext.Invoices.Add(invoice);
        }

        public void AddInvoiceItem(InvoiceItem invoiceItem)
        {
            _deadfileContext.InvoiceItems.Add(invoiceItem);
        }

        public InvoiceItem GetInvoiceItemById(int invoiceItemId)
        {
            return _deadfileContext.InvoiceItems.Find(new object[1] {invoiceItemId});
        }

        public void RemoveInvoiceItem(InvoiceItem invoiceItem)
        {
            _deadfileContext.InvoiceItems.Remove(invoiceItem);
        }

        public Expense GetExpenseById(int expenseId)
        {
            return _deadfileContext.Expenses.Find(new object[] {expenseId});
        }

        public BillableHour GetBillableHourById(int billableHourId)
        {
            return _deadfileContext.BillableHours.Find(new object[] {billableHourId});
        }

        public bool HasUniqueInvoiceReference(Company company, int invoiceId, int invoiceReference)
        {
            return !(from invoice in _deadfileContext.Invoices
                     where invoice.Company == company
                     where invoice.InvoiceReference == invoiceReference
                     where (invoiceId == ModelBase.NewModelId || invoiceId != invoice.InvoiceId)
                     select invoice.InvoiceId).Any();
        }

        public IEnumerable<int> GetUsedInvoiceReferencesForCompany(Company company)
        {
            return (from invoice in _deadfileContext.Invoices
                where invoice.Company == company
                where invoice.InvoiceReference != 0
                orderby invoice.InvoiceReference
                select invoice.InvoiceReference);
        }

        public IEnumerable<Job> GetJobs(string filter)
        {
            return _deadfileContext.Jobs;
        }

        public IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForJob(int jobId, string filter)
        {
            return (from jobTask in _deadfileContext.JobTasks
                    where jobTask.JobId == jobId
                    where (jobTask.Description == null || filter == null || jobTask.Description.Contains(filter))
                    join job in _deadfileContext.Jobs on jobTask.JobId equals job.JobId
                    join client in _deadfileContext.Clients on jobTask.ClientId equals client.ClientId
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

        public IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForDateRange(DateTime startDate, DateTime endDate, string filter, bool includeInactive)
        {
            return (from jobTask in _deadfileContext.JobTasks
                    where (includeInactive || jobTask.State == JobTaskState.Active)
                    where (jobTask.State == JobTaskState.Active || jobTask.DueDate >= startDate)
                    where jobTask.DueDate <= endDate
                    where (filter == null || filter == "" || jobTask.Description.Contains(filter))
                    join job in _deadfileContext.Jobs on jobTask.JobId equals job.JobId
                    join client in _deadfileContext.Clients on jobTask.ClientId equals client.ClientId
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
            _deadfileContext.JobTasks.Remove(jobTask);
        }

        public void RemoveExpense(Expense expense)
        {
            _deadfileContext.Expenses.Remove(expense);
        }

        public LocalAuthority GetLocalAuthorityById(int localAuthorityId)
        {
            return _deadfileContext.LocalAuthorities.Find(new object[1] {localAuthorityId});
        }

        public void RemoveLocalAuthority(LocalAuthority localAuthority)
        {
            _deadfileContext.LocalAuthorities.Remove(localAuthority);
        }

        public void RemoveQuotation(Quotation quotation)
        {
            _deadfileContext.Quotations.Remove(quotation);
        }

        public IEnumerable<ApplicationWithJob> GetApplicationsWithJobs(string filter, bool includeInactive)
        {
            return (from application in _deadfileContext.Applications
                where (includeInactive || application.State == ApplicationState.Current)
                where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                orderby application.EstimatedDecisionDate descending
                join job in _deadfileContext.Jobs on application.JobId equals job.JobId
                select new ApplicationWithJob {Job = job, Application = application});
        }

        public void AddLocalAuthority(LocalAuthority localAuthority)
        {
            _deadfileContext.LocalAuthorities.Add(localAuthority);
        }

        public void AddApplication(Application application)
        {
            _deadfileContext.Applications.Add(application);
        }

        public Application GetApplicationById(int applicationId)
        {
            return _deadfileContext.Applications.Find(new object[1] {applicationId});
        }

        public void AddExpense(Expense expense)
        {
            _deadfileContext.Expenses.Add(expense);
        }

        public void AddBillableHour(BillableHour billableHour)
        {
            _deadfileContext.BillableHours.Add(billableHour);
        }

        public void AddJob(Job job)
        {
            _deadfileContext.Jobs.Add(job);
        }

        public void AddJobTask(JobTask jobTask)
        {
            _deadfileContext.JobTasks.Add(jobTask);
        }

        public JobTask GetJobTaskById(int jobTaskId)
        {
            return _deadfileContext.JobTasks.Find(new object[1] {jobTaskId});
        }

        public void AddQuotation(Quotation quotation)
        {
            _deadfileContext.Quotations.Add(quotation);
        }

        public Quotation GetQuotationById(int quotationId)
        {
            return _deadfileContext.Quotations.Find(new object[1] {quotationId});
        }

        public IEnumerable<ExpenseWithJobAndClient> GetActiveExpenses(string filter)
        {
            return (from expense in _deadfileContext.Expenses
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
            return (from expense in _deadfileContext.BillableHours
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

        public IEnumerable<Application> GetApplicationsForJob(int jobId, string filter)
        {
            return (from application in _deadfileContext.Applications
                where application.JobId == jobId
                where (filter == null || filter == "" || application.LocalAuthorityReference.Contains(filter))
                orderby application.CreationDate
                select application);
        }

        public IEnumerable<BillableHour> GetBillableHoursForJob(int jobId, string filter)
        {
            return (from billableHour in _deadfileContext.BillableHours
                where billableHour.JobId == jobId
                select billableHour);
        }

        public IEnumerable<Expense> GetExpensesForJob(int jobId, string filter)
        {
            return (from expense in _deadfileContext.Expenses
                where expense.JobId == jobId
                select expense);
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities(string filter)
        {
            return (from localAuthority in _deadfileContext.LocalAuthorities
                where (filter == null || filter == "" || localAuthority.Name.Contains(filter))
                orderby localAuthority.Name
                select localAuthority);
        }
    }
}
