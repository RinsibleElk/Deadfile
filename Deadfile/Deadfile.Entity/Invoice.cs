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

        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        [Required(ErrorMessage = "You must provide an address for this Client."),
         MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string ClientAddressFirstLine { get; set; }

        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string ClientAddressSecondLine { get; set; }

        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string ClientAddressThirdLine { get; set; }

        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string ClientAddressPostCode { get; set; }

        [Required(ErrorMessage = "An Invoice must have a Project."),
         MinLength(1, ErrorMessage = "A Project must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "A Project must be at most 100 characters long.")]
        public string Project { get; set; }
        [MaxLength(100, ErrorMessage = "A Description must be at most 100 characters long.")]
        public string Description { get; set; }
        public int ClientId { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
