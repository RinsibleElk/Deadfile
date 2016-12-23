﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.Reporting;

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
        /// Get all quotations that satisfy a filter (which may be null).
        /// </summary>
        /// <param name="filterText"></param>
        /// <returns></returns>
        IEnumerable<QuotationModel> GetQuotations(string filterText);

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
        /// Get the billable models for a given client and invoice.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IEnumerable<BillableModel> GetBillableModelsForClientAndInvoice(int clientId, int invoiceId);

        /// <summary>
        /// Save changes to a job (or create a new one).
        /// </summary>
        /// <param name="jobModel"></param>
        void SaveJob(JobModel jobModel);

        /// <summary>
        /// Save changes to an invoice (or create a new one).
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <param name="billableJobs"></param>
        void SaveInvoice(InvoiceModel invoiceModel, IEnumerable<BillableJob> billableJobs);

        /// <summary>
        /// Validate that the invoice reference is unique (for the specified company).
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <param name="invoiceReference"></param>
        /// <returns></returns>
        bool HasUniqueInvoiceReference(InvoiceModel invoiceModel, int invoiceReference);

        /// <summary>
        /// Delete the specified client.
        /// </summary>
        /// <remarks>
        /// This will simply set the specified client to inactive.
        /// </remarks>
        /// <param name="clientModel"></param>
        void DeleteClient(ClientModel clientModel);

        /// <summary>
        /// Delete the specified invoice.
        /// </summary>
        /// <remarks>
        /// This will simply cancel the invoice.
        /// </remarks>
        /// <param name="invoiceModel"></param>
        void DeleteInvoice(InvoiceModel invoiceModel);

        /// <summary>
        /// Delete the specified job.
        /// </summary>
        /// <remarks>
        /// This will simply set the status to inactive.
        /// </remarks>
        /// <param name="jobModel"></param>
        void DeleteJob(JobModel jobModel);

        /// <summary>
        /// Get the set of suggested invoice reference ids for a given company.
        /// </summary>
        /// <remarks>
        /// It is allowed, on cancellation of an <see cref="Invoice"/>, to vacate a used invoice reference. Therefore, the invoice reference ids are a sparse collection
        /// of integers. In general, we should aim to offer the lowest (up to) 2 unused values and the next available value.
        /// </remarks>
        /// <param name="company"></param>
        /// <returns></returns>
        int[] GetSuggestedInvoiceReferenceIdsForCompany(Company company);

        /// <summary>
        /// Get the next available job number.
        /// </summary>
        /// <returns></returns>
        int GetNextSuggestedJobNumber();

        /// <summary>
        /// Get the tasks for a job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<JobTaskModel> GetJobTasksForJob(int jobId, string filter);

        /// <summary>
        /// Save a task for a job.
        /// </summary>
        /// <param name="jobTaskModel"></param>
        void SaveJobTask(JobTaskModel jobTaskModel);

        /// <summary>
        /// Save a quotation.
        /// </summary>
        /// <param name="quotationModel"></param>
        void SaveQuotation(QuotationModel quotationModel);

        /// <summary>
        /// Get the unbilled jobs.
        /// </summary>
        /// <param name="filterText"></param>
        /// <returns></returns>
        IEnumerable<UnbilledJobModel> GetUnbilledJobs(string filterText);

        /// <summary>
        /// Get the job tasks that expire before the specified end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="filter"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        IEnumerable<JobTaskModel> GetJobTasks(DateTime startDate, DateTime endDate, string filter, bool includeInactive);

        /// <summary>
        /// Permanently delete a job task.
        /// </summary>
        /// <param name="jobTaskModel"></param>
        void DeleteJobTask(JobTaskModel jobTaskModel);

        /// <summary>
        /// Permanently delete an expense.
        /// </summary>
        /// <param name="expenseModel"></param>
        void DeleteExpense(ExpenseModel expenseModel);

        /// <summary>
        /// Permanently delete a local authority.
        /// </summary>
        /// <param name="localAuthorityModel"></param>
        void DeleteLocalAuthority(LocalAuthorityModel localAuthorityModel);

        /// <summary>
        /// Permanently delete a quotation.
        /// </summary>
        /// <param name="quotationModel"></param>
        void DeleteQuotation(QuotationModel quotationModel);
    }
}
