using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageViewModel : EditableItemViewModel<ClientAndInvoice, InvoiceModel>, IInvoicesPageViewModel
    {
        private readonly IDeadfileRepository _repository;

        public InvoicesPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        public override InvoiceModel GetModel(ClientAndInvoice clientAndInvoice)
        {
            InvoiceModel invoiceModel;
            if (clientAndInvoice.Equals(default(ClientAndInvoice)) || clientAndInvoice.InvoiceId == 0 || clientAndInvoice.InvoiceId == ModelBase.NewModelId)
            {
                invoiceModel = new InvoiceModel();
                DisplayName = "New Invoice";
                Editable = true;
            }
            else
            {
                invoiceModel = _repository.GetInvoiceById(clientAndInvoice.InvoiceId);
                if (invoiceModel.InvoiceId == ModelBase.NewModelId)
                    DisplayName = "New Invoice";
                else
                    DisplayName = invoiceModel.Company + " " + invoiceModel.InvoiceReference.ToString();
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return invoiceModel;
        }

        public override void EditingStatusChanged(bool editable)
        {
        }

        public override void PerformSave()
        {
        }

        public Experience Experience { get; } = Experience.Invoices;
        public bool ShowActionsPad { get; } = true;
    }
}
