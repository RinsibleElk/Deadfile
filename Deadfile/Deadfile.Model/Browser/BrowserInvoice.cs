using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model.Browser
{
    public sealed class BrowserInvoice : BrowserModel
    {
        private int _invoiceReference;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set { SetProperty(ref _invoiceReference, value); }
        }

        public BrowserInvoice() : base(true)
        {
            Id = ModelBase.NewModelId;
        }

        private IDeadfileRepository _repository;
        internal void SetRepository(IDeadfileRepository repository)
        {
            _repository = repository;
        }

        protected override void LoadChildren()
        {
            throw new NotImplementedException();
        }

        public override BrowserModelType ModelType
        {
            get { return BrowserModelType.Invoice; }
        }
    }
}
