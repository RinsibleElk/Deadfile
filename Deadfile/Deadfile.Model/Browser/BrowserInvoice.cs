using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Browser
{
    public sealed class BrowserInvoice : BrowserModel
    {
        private string _invoiceReference;
        public string InvoiceReference
        {
            get { return _invoiceReference; }
            set { SetProperty(ref _invoiceReference, value); }
        }

        public BrowserInvoice() : base(true)
        {
            Id = InvoiceModel.NewInvoiceId;
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
