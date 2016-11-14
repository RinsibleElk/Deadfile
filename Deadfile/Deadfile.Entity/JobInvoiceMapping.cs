using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// Entity model for a mapping between jobs and associated invoices.
    /// </summary>
    public class JobInvoiceMapping
    {
        public int JobInvoiceMappingId { get; set; }

        public int JobId { get; set; }

        public int InvoiceId { get; set; }
    }
}
