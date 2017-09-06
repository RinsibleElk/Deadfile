using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deadfile.Entity;
using Deadfile.Model.Browser;

namespace Deadfile.Model.Interfaces
{
    /// <summary>
    /// Abstraction onto DeadfileContext giving all of the possible queries.
    /// </summary>
    public interface IDeadfileContextAbstraction : IDisposable
    {
        IEnumerable<Application> GetApplicationsForJob(int jobId, string filter);
        IEnumerable<BillableHour> GetBillableHoursForJob(int jobId, string filter);
        IEnumerable<Expense> GetExpensesForJob(int jobId, string filter);
        IEnumerable<LocalAuthority> GetLocalAuthorities(string filter);
        IEnumerable<Client> GetClients(string filter);
        IEnumerable<Client> GetOrderedClients(BrowserSettings settings);
        IEnumerable<Job> GetOrderedJobs(int clientId, BrowserSettings settings);
        IEnumerable<Invoice> GetOrderedInvoices(BrowserSettings settings);
        IEnumerable<int> GetInvoiceIdsForJobExpenses(int jobId);
        IEnumerable<int> GetInvoiceIdsForJobBillableHours(int jobId);
        IEnumerable<int> GetJobIdsForInvoiceExpenses(int invoiceId);
        IEnumerable<int> GetJobIdsForInvoiceBillableHours(int invoiceId);
        Invoice GetInvoiceById(int invoiceId);
        IEnumerable<Quotation> GetQuotations(string filter);
        Client GetClientById(int clientId);
        Job GetJobById(int jobId);
        IEnumerable<InvoiceItem> GetInvoiceItems(int invoiceId);
        Invoice GetFirstActiveInvoiceForClient(int clientId);
        void AddClient(Client client);
        void SaveChanges();
        void AddInvoice(Invoice invoice);
        void AddInvoiceItem(InvoiceItem invoiceItem);
        InvoiceItem GetInvoiceItemById(int invoiceItemId);
        void RemoveInvoiceItem(InvoiceItem invoiceItem);
        Expense GetExpenseById(int expenseId);
        BillableHour GetBillableHourById(int billableHourId);
        bool HasUniqueInvoiceReference(Company company, int invoiceId, int invoiceReference);
        IEnumerable<int> GetUsedInvoiceReferencesForCompany(Company company);
        IEnumerable<Job> GetJobs(string filter);
        IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForJob(int jobId, string filter);
        void AddJobTask(JobTask jobTask);
        JobTask GetJobTaskById(int jobTaskId);
        void AddQuotation(Quotation quotation);
        Quotation GetQuotationById(int quotationId);
        IEnumerable<ExpenseWithJobAndClient> GetActiveExpenses(string filter);
        IEnumerable<BillableHourWithJobAndClient> GetActiveBillableHours(string filter);
        IEnumerable<JobTaskWithProperty> GetJobTasksWithPropertiesForDateRange(DateTime startDate, DateTime endDate, string filter, bool includeInactive);
        void RemoveJobTask(JobTask jobTask);
        void RemoveExpense(Expense expense);
        LocalAuthority GetLocalAuthorityById(int localAuthorityId);
        void RemoveLocalAuthority(LocalAuthority localAuthority);
        void RemoveQuotation(Quotation quotation);
        IEnumerable<ApplicationWithJob> GetApplicationsWithJobs(string filter, bool includeInactive);
        void AddLocalAuthority(LocalAuthority localAuthority);
        void AddApplication(Application application);
        Application GetApplicationById(int applicationId);
        void AddExpense(Expense expense);
        void AddBillableHour(BillableHour billableHour);
        void AddJob(Job job);
    }
}
