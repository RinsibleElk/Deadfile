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
                cfg.CreateMap<Job, JobModel>();
                cfg.CreateMap<JobModel, Job>();
                cfg.CreateMap<Invoice, InvoiceModel>();
                cfg.CreateMap<InvoiceModel, Invoice>();
                cfg.CreateMap<Quotation, QuotationModel>();
                cfg.CreateMap<QuotationModel, Quotation>();
            });
            Mapper = config.CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
