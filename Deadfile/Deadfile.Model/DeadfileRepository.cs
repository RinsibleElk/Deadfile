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

        public void SetUpFakeData()
        {
            using (var dbContext = new DeadfileContext())
            {
                if ((from client in dbContext.Clients select client).FirstOrDefault() == null)
                {
                    var designTimeRepository = new DeadfileDesignTimeRepository();
                    foreach (var clientModel in designTimeRepository.GetClients())
                    {
                        dbContext.Clients.Add(_modelEntityMapper.Mapper.Map<Client>(clientModel));
                    }
                    try
                    {
                        AddQuotations(dbContext);
                        dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        private static void AddQuotations(DeadfileContext dbContext)
        {
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
                dbContext.Quotations.Add(new Quotation()
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
                dbContext.Quotations.Add(new Quotation()
                {
                    Author = "Eric Cartman",
                    Phrase = ericCartmanQuotation
                });
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
    }
}
