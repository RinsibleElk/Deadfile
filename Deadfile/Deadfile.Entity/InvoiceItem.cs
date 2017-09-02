using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }

        [Required(ErrorMessage = "An InvoiceItem requires a Description"),
         MinLength(5, ErrorMessage = "Description must be at least 5 characters long"),
         MaxLength(200, ErrorMessage = "Description must be at most 200 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "An Invoice Item must have a value associated")]
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "An Invoice Item must have a VAT value associated")]
        public double VatValue { get; set; }

        [Required(ErrorMessage = "An Invoice Item must have a VAT rate associated")]
        public double VatRate { get; set; }

        [Required(ErrorMessage = "An Invoice Item requires Include VAT to be defined")]
        public bool IncludeVat { get; set; }

        public int InvoiceId { get; set; }
    }
}
