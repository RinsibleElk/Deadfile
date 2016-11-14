using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "An Invoice must have a creation date.")]
        public DateTime CreatedDate { get; set; }

        public double GrossAmount { get; set; }

        public double NetAmount { get; set; }

        [Required(ErrorMessage = "The Invoice requires a Status.")]
        public InvoiceStatus Status { get; set; }

        public int InvoiceReference { get; set; }

        public int ClientId { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
