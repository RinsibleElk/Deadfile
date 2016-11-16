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

        [Required(ErrorMessage = "An Invoice requires a Company that it has been issued for.")]
        public Company Company { get; set; }

        [Required(ErrorMessage = "An Invoice requires a Client Name."),
         MinLength(1, ErrorMessage = "Client Name must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Name must be at most 100 character long.")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "An Invoice requires a Client Address."),
         MinLength(1, ErrorMessage = "Client Address must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Address must be at most 100 character long.")]
        public string ClientAddress { get; set; }

        public int ClientId { get; set; }

        public virtual ICollection<JobInvoiceMapping> JobInvoiceMappings { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
