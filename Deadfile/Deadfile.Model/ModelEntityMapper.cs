using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using AutoMapper;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public sealed class ModelEntityMapper : IModelEntityMapper
    {
        public ModelEntityMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, ClientModel>();
                cfg.CreateMap<ClientModel, Client>();
            });
            Mapper = config.CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
