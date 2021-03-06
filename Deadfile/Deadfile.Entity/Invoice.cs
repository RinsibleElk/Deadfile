﻿using System;
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

        [Required(ErrorMessage = "An Invoice must have a gross amount associated")]
        public double GrossAmount { get; set; }

        [Required(ErrorMessage = "An Invoice must have a net amount associated")]
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "An Invoice must have a VAT rate associated"),
         Range(0, 100)]
        public double VatRate { get; set; }

        [Required(ErrorMessage = "An Invoice must have a VAT value associated")]
        public double VatValue { get; set; }

        [Required(ErrorMessage = "The Invoice requires a Status.")]
        public InvoiceStatus Status { get; set; }

        public int InvoiceReference { get; set; }

        [Required(ErrorMessage = "An Invoice requires a Company that it has been issued for.")]
        public Company Company { get; set; }

        [Required(ErrorMessage = "An Invoice requires a Client Name."),
         MinLength(1, ErrorMessage = "Client Name must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Name must be at most 100 characters long.")]
        public string ClientName { get; set; }

        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        [Required(ErrorMessage = "You must provide an address for this Client."),
         MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressFirstLine { get; set; }

        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressSecondLine { get; set; }

        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressThirdLine { get; set; }

        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string ClientAddressPostCode { get; set; }

        [Required(ErrorMessage = "An Invoice must have a Project."),
         MinLength(1, ErrorMessage = "A Project must be at least 1 character long."),
         MaxLength(200, ErrorMessage = "A Project must be at most 200 characters long.")]
        public string Project { get; set; }

        [Required(ErrorMessage = "An Invoice must have a Description."),
         MaxLength(200, ErrorMessage = "A Description must be at most 200 characters long.")]
        public string Description { get; set; }

        public int ClientId { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
