using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public class JobInvoiceMappingModel : ModelBase
    {
        public int JobInvoiceMappingId { get; set; }
        public int JobId { get; set; }
        public int InvoiceId { get; set; }
        public override int Id
        {
            get { return JobInvoiceMappingId; }
            set { JobInvoiceMappingId = value; }
        }
    }
}
