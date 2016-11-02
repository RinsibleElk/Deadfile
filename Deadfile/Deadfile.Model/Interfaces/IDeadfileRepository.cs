using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <returns></returns>
        IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId);

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
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEnumerable<BrowserJob> GetBrowserJobsForClient(int clientId);

        /// <summary>
        /// Get Invoices for a given job for use in Browser.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(int jobId);

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
        /// Save changes to a client.
        /// </summary>
        /// <param name="selectedClient"></param>
        void SaveClient(ClientModel selectedClient);
    }
}
