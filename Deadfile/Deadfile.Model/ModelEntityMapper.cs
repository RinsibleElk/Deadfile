using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using AutoMapper;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Reporting;

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
                cfg.CreateMap<InvoiceItem, InvoiceItemModel>();
                cfg.CreateMap<InvoiceItemModel, InvoiceItem>();
                cfg.CreateMap<Quotation, QuotationModel>();
                cfg.CreateMap<QuotationModel, Quotation>();
                cfg.CreateMap<Application, ApplicationModel>();
                cfg.CreateMap<ApplicationModel, Application>();
                cfg.CreateMap<Application, CurrentApplicationModel>();
                cfg.CreateMap<CurrentApplicationModel, Application>();
                cfg.CreateMap<BillableHour, BillableHourModel>();
                cfg.CreateMap<BillableHourModel, BillableHour>();
                cfg.CreateMap<Expense, ExpenseModel>();
                cfg.CreateMap<ExpenseModel, Expense>();
                cfg.CreateMap<JobTask, JobTaskModel>();
                cfg.CreateMap<JobTaskModel, JobTask>();
                cfg.CreateMap<LocalAuthority, LocalAuthorityModel>();
                cfg.CreateMap<LocalAuthorityModel, LocalAuthority>();
            });
            Mapper = config.CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
