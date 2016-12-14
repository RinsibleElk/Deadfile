using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Reporting;

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

        public bool HasUniqueInvoiceReference(InvoiceModel invoiceModel, int invoiceReference)
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

        public int GetNextSuggestedJobNumber()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobTaskModel> GetJobTasksForJob(int jobId, string filter)
        {
            throw new NotImplementedException();
        }

        public void SaveJobTask(JobTaskModel jobTaskModel)
        {
            throw new NotImplementedException();
        }

        public void SaveQuotation(QuotationModel quotationModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnbilledClientModel> GetUnbilledClients(string filterText)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobTaskModel> GetJobTasks(DateTime endDate, string filter)
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

        public IEnumerable<QuotationModel> GetQuotations(string filterText)
        {

            var li = new List<QuotationModel>();

            var homerSimpsonQuotations = new string[]
            {
                "Weaseling out of things is important to learn. It's what separates us from the animals... Except the weasel.",
                "Books are useless! I only ever read one book, To Kill A Mockingbird, and it gave me absolutely no insight on how to kill mockingbirds!",
                "Fame was like a drug. But what was even more like a drug were the drugs.",
                "Son, when you participate in sporting events, it’s not whether you win or lose: it’s how drunk you get.",
                "You don’t like your job, you don’t strike. You go in every day and do it really half-assed. That’s the American way.",
                "Facts are meaningless. You could use facts to prove anything that’s even remotely true!"
            };
            foreach (var homerSimpsonQuotation in homerSimpsonQuotations)
            {
                li.Add(new QuotationModel
                {
                    Author = "Homer Simpson",
                    Phrase = homerSimpsonQuotation
                });
            }
            var ericCartmanQuotations = new string[]
            {
                "It's a man's obligation to stick his boneration in a woman's separation, this sort of penetration will increase the population of the younger generation.",
                "I'm not just sure. I'm HIV positive.",
                "Don't you know the first rule of physics? Anything that's fun costs at least eight dollars.",
                "Stan, your dog is a gay homosexual!"
            };
            foreach (var ericCartmanQuotation in ericCartmanQuotations)
            {
                li.Add(new QuotationModel
                {
                    Author = "Eric Cartman",
                    Phrase = ericCartmanQuotation
                });
            }
            return li;
        }
    }
}
