using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public class BillableHourInvoiceMappingModel
    {
        public int BillableHourInvoiceMappingId { get; set; }
        public int BillableHourId { get; set; }
        public int InvoiceId { get; set; }
    }
}
