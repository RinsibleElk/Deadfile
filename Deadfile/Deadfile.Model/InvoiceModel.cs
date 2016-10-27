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
    public class InvoiceModel : ModelBase
    {
        public override int Id
        {
            get { return InvoiceId; }
            set { InvoiceId = value; }
        }

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
