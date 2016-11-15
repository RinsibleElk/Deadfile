using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;

namespace Deadfile.Model.Interfaces
{
    /// <summary>
    /// Repository for accessing data from the DeadfileContext.
    /// </summary>
    public interface IDeadfileRepository
    {
        /// <summary>
        /// Get all clients.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClientModel> GetClients();

        /// <summary>
        /// Get the applications for a given job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId, string filter);

        /// <summary>
        /// Get the expenses for a given job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<ExpenseModel> GetExpensesForJob(int jobId, string filter);

        /// <summary>
        /// Get the billable hours for a given job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<BillableHourModel> GetBillableHoursForJob(int jobId, string filter);

        /// <summary>
        /// Get all the local authorities.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<LocalAuthorityModel> GetLocalAuthorities(string filter);

        /// <summary>
        /// Naughty method to seed the repository with fake data. Run at initialization time and only changes the database if it's totally empty.
        /// </summary>
        /// <remarks>
        /// This is for use in testing. It means that users can drop the whole database, run the application, and it will have put some data there.
        /// </remarks>
        void SetUpFakeData();

        /// <summary>
        /// Run on a 30 second timer, used to retrieve a random quotation and display it to the user for a bit of fun.
        /// </summary>
        /// <returns></returns>
        QuotationModel GetRandomQuotation();

        /// <summary>
        /// Synchronously fetch a <see cref="ClientModel"/>, given the unique id in the database.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientModel GetClientById(int clientId);

        /// <summary>
        /// Get items for use in Browser.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEnumerable<BrowserModel> GetBrowserItems(BrowserSettings settings);

        /// <summary>
        /// Get Jobs for a given client for use in Browser.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="includeInactiveEnabled"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEnumerable<BrowserJob> GetBrowserJobsForClient(BrowserMode mode, bool includeInactiveEnabled, int clientId);

        /// <summary>
        /// Get Invoices for a given job for use in Browser.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="includeInactiveEnabled"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(BrowserMode mode, bool includeInactiveEnabled, int jobId);

        /// <summary>
        /// Synchronously fetch a <see cref="JobModel"/>, given the unique id in the database.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        JobModel GetJobById(int jobId);

        /// <summary>
        /// Synchronously fetch a <see cref="InvoiceModel"/>, given the unique id in the database.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        InvoiceModel GetInvoiceById(int invoiceId);

        /// <summary>
        /// Save changes to a client (or create a new one).
        /// </summary>
        /// <param name="clientModel"></param>
        void SaveClient(ClientModel clientModel);

        /// <summary>
        /// Save changes to a local authority (or create a new one).
        /// </summary>
        /// <param name="localAuthorityModel"></param>
        void SaveLocalAuthority(LocalAuthorityModel localAuthorityModel);

        /// <summary>
        /// Save changes to an application (or create a new one).
        /// </summary>
        /// <param name="applicationModel"></param>
        void SaveApplication(ApplicationModel applicationModel);

        /// <summary>
        /// Save changes to an expense (or create a new one).
        /// </summary>
        /// <param name="expenseModel"></param>
        void SaveExpense(ExpenseModel expenseModel);

        /// <summary>
        /// Save changes to a billable hour (or create a new one).
        /// </summary>
        /// <param name="billableHour"></param>
        void SaveBillableHour(BillableHourModel billableHour);

        /// <summary>
        /// Get the Browser client for a given client id.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="includeInactiveEnabled"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        BrowserModel GetBrowserClientById(BrowserMode mode, bool includeInactiveEnabled, int clientId);

        /// <summary>
        /// Get the browser jobs for a given invoice.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="includeInactiveEnabled"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IEnumerable<BrowserModel> GetBrowserJobsForInvoice(BrowserMode mode, bool includeInactiveEnabled, int invoiceId);

        /// <summary>
        /// Get the billable models for a given client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IEnumerable<BillableModel> GetBillableModelsForClient(int clientId, int invoiceId);
    }
}
