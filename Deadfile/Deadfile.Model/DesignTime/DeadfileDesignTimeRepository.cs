﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
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

        public IEnumerable<BrowserJob> GetBrowserJobsForClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BrowserInvoice> GetBrowserInvoicesForJob(int jobId)
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
    }
}
