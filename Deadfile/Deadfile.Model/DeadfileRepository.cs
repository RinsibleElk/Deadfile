using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public sealed class DeadfileRepository : IDeadfileRepository
    {
        private readonly IModelEntityMapper _modelEntityMapper;

        public DeadfileRepository(IModelEntityMapper modelEntityMapper)
        {
            _modelEntityMapper = modelEntityMapper;
        }
        public IEnumerable<ClientModel> GetClients()
        {
            // One time only.
            // IMPORTANT:
            // - Ben, this is for testing, uncomment this _once_, run the application, navigate to Clients, _once_, then exit, then comment it, rebuild, rerun.
            //var designTimeRepository = new DeadfileDesignTimeRepository();
            //using (var dbContext = new DeadfileContext())
            //{
            //    foreach (var clientModel in designTimeRepository.GetClients())
            //    {
            //        dbContext.Clients.Add(_modelEntityMapper.Mapper.Map<Client>(clientModel));
            //    }
            //    try
            //    {
            //        dbContext.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {

            //    }
            //}

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
    }
}
