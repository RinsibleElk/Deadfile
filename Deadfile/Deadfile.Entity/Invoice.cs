using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public virtual List<Job> Jobs { get; set; }
    }
}
