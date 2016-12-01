using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model.DesignTime
{
    public class DeadfileDesignTimeRepository : IDeadfileRepository
    {
        private readonly IModelEntityMapper _modelEntityMapper;
        public DeadfileDesignTimeRepository()
        {
            _modelEntityMapper = new ModelEntityMapper();
        }

        public IEnumerable<ClientModel> GetClients()
        {
            // Is called as is also used as a lame way to seed the database with some junk.
            var entityClients = FakeData.GetFakeClients();
            return entityClients.Select(_modelEntityMapper.Mapper.Map<ClientModel>);
        }

        public IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId, string filter)
        {
            var applications = FakeData.GetFakeApplications();
            return applications.Select(_modelEntityMapper.Mapper.Map<ApplicationModel>);
        }

        public IEnumerable<ExpenseModel> GetExpensesForJob(int jobId, string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BillableHourModel> GetBillableHoursForJob(int jobId, string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LocalAuthorityModel> GetLocalAuthorities(string filter)
        {
            var localAuthorities = FakeData.GetFakeLocalAuthorities();
            return localAuthorities.Select(_modelEntityMapper.Mapper.Map<LocalAuthorityModel>);
        }

        public IEnumerable<ClientModel> GetFilteredClients(string filter)
        {
            // Should never get called as this implementation is only used in design time.
            throw new NotImplementedException();
        }

        public void SetUpFakeData()
        {
            // Should never get called as this implementation is only used in design time.
            throw new NotImplementedException();
        }

        public QuotationModel GetRandomQuotation()
        {
            // Should never get called as this implementation is only used in design time.
            throw new NotImplementedException();
        }

        public ClientModel GetClientById(int clientId)
        {
            // Should never get called as this implementation is only used in design time.
            throw new NotImplementedException();
        }

        public IEnumerable<BrowserModel> GetBrowserItems(BrowserSettings settings)
        {
            return
                from client in GetClients()
                select new BrowserClient() { Id = client.ClientId, FullName = client.FullName };
        }

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(BrowserMode mode, bool includeInactiveEnabled, int jobId)
        {
            throw new NotImplementedException();
        }

        public JobModel GetJobById(int jobId)
        {
            throw new NotImplementedException();
        }

        public InvoiceModel GetInvoiceById(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public void SaveClient(ClientModel selectedClient)
        {
            throw new NotImplementedException();
        }

        public void SaveJob(JobModel selectedJob)
        {
            throw new NotImplementedException();
        }

        public void SaveInvoice(InvoiceModel invoiceModel, IEnumerable<BillableJob> billableJobs)
        {
            throw new NotImplementedException();
        }

        public bool HasUniqueInvoiceReference(InvoiceModel invoiceModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(ClientModel selectedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteInvoice(InvoiceModel invoiceModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteJob(JobModel jobModel)
        {
            throw new NotImplementedException();
        }

        public int[] GetSuggestedInvoiceReferenceIdsForCompany(Company company)
        {
            throw new NotImplementedException();
        }

        public void SaveLocalAuthority(LocalAuthorityModel localAuthorityModel)
        {
            throw new NotImplementedException();
        }

        public void SaveApplication(ApplicationModel applicationModel)
        {
            throw new NotImplementedException();
        }

        public void SaveExpense(ExpenseModel expenseModel)
        {
            throw new NotImplementedException();
        }

        public void SaveBillableHour(BillableHourModel billableHour)
        {
            throw new NotImplementedException();
        }

        public BrowserModel GetBrowserClientById(BrowserMode mode, bool includeInactiveEnabled, int clientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BrowserModel> GetBrowserJobsForInvoice(BrowserMode mode, bool includeInactiveEnabled, int invoiceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BillableModel> GetBillableModelsForClientAndInvoice(int clientId, int invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}
