using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
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
            var entityClients = Client.GenerateFakeData();
            return entityClients.Select(_modelEntityMapper.Mapper.Map<ClientModel>);
        }

        public void SetUpFakeData()
        {
            // Should never get called as this implementation is only used in design time and for seeding the real repo with data.
            throw new NotImplementedException();
        }

        public QuotationModel GetRandomQuotation()
        {
            // Should never get called as this implementation is only used in design time.
            throw new NotImplementedException();
        }
    }
}
