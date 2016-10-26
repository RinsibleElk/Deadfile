using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for an Invoice.
    /// </summary>
    public class InvoiceModel : ValidatableBindableBase
    {
        public InvoiceModel()
        {
            InvoiceId = NewInvoiceId;
        }

        /// <summary>
        /// Deliberately invalid id to represent to the EditInvoice experience that a invoice job is required.
        /// </summary>
        public const int NewInvoiceId = Int32.MinValue;

        private int _invoiceId;
        /// <summary>
        /// This is Entity's model for this Invoice, or if it is not in the database yet then default.
        /// </summary>
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }
    }
}
