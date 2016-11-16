using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// The name is for consistency. This can represent multiple items.
    /// </summary>
    public class BillableHour
    {
        public int BillableHourId { get; set; }

        [Required(ErrorMessage = "A BillableHour must have an amount associated")]
        public double NetAmount { get; set; }

        public int JobId { get; set; }
        public int? InvoiceId { get; set; }
    }
}
