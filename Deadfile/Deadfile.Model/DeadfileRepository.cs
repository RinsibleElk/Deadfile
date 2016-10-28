using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
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

        public IEnumerable<ApplicationModel> GetApplicationsForJob(int jobId)
        {
            using (var dbContext = new DeadfileContext())
            {
                var li = new List<ApplicationModel>();
                foreach (var application in (from application in dbContext.Applications
                                             where application.JobId == jobId
                                             orderby application.CreationDate
                                             select application))
                {
                    li.Add(_modelEntityMapper.Mapper.Map<ApplicationModel>(application));
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
            var li = new List<BrowserClient>();
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
                        ? ((client.FirstName == null || client.FirstName == "") ? client.Title : client.FirstName)
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
                    client.SetRepository(this);
                    li.Add(client);
                }
            }
            return li;
        }

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(int clientId)
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
                    job.SetRepository(this);
                    li.Add(job);
                }
            }
            return li;
        }

        public IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(int jobId)
        {
            using (var dbContext = new DeadfileContext())
            {
                return new List<BrowserInvoice>(from invoice in dbContext.Invoices
                    where invoice.Jobs.FirstOrDefault((job) => job.JobId == jobId) != null
                    select
                    new BrowserInvoice()
                    {
                        Id = invoice.InvoiceId,
                        InvoiceReference = invoice.InvoiceReference.ToString()
                    });
            }
        }

        public void SetUpFakeData()
        {
            using (var dbContext = new DeadfileContext())
            {
                if ((from client in dbContext.Clients select client).FirstOrDefault() == null)
                {
                    foreach (var clientModel in FakeData.GetFakeClients())
                    {
                        dbContext.Clients.Add(_modelEntityMapper.Mapper.Map<Client>(clientModel));
                    }
                    FakeData.AddFakeQuotations(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeJobs(dbContext);
                    dbContext.SaveChanges();
                    FakeData.AddFakeApplications(dbContext);
                    dbContext.SaveChanges();
                }
            }
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
            using (var dbContext = new DeadfileContext())
            {
                var invoice = dbContext.Invoices.Find(invoiceId);
                return _modelEntityMapper.Mapper.Map<InvoiceModel>(invoice);
            }
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
    }
}
