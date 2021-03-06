﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Deadfile.Entity
{
    /// <summary>
    /// Entity model for a Client.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Unique identifier for this <see cref="Client"/> in the database.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Title, e.g. Mr, Mrs, Dr.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A Client's title must be less than 20 characters long.")]
        public string Title { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's first name must be less than 50 characters long.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Middle names.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's middle names must be less than 50 characters long.")]
        public string MiddleNames { get; set; }

        /// <summary>
        /// Last name. This is a required field. When the full name is not yet known, the last name should be used.
        /// </summary>
        [Required(ErrorMessage = "You must provide at least a last name for this Client."),
         MaxLength(50, ErrorMessage = "A Client's last name must be less than 50 characters long.")]
        public string LastName { get; set; }

        /// <summary>
        /// Company.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A Client's Company must be 100 characters or fewer.")]
        public string Company { get; set; }

        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        [Required(ErrorMessage = "You must provide an address for this Client."),
         MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressFirstLine { get; set; }

        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressSecondLine { get; set; }

        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressThirdLine { get; set; }

        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string AddressPostCode { get; set; }

        /// <summary>
        /// 1st Phone Number.
        /// </summary>
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// 2nd Phone Number.
        /// </summary>
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber2 { get; set; }

        /// <summary>
        /// 3rd Phone Number.
        /// </summary>
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber3 { get; set; }

        /// <summary>
        /// Client's E-mail Address.
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid e-mail address given for this Client.")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// A Client is never deleted from the database. They are only ever <see cref="ClientStatus.Active"/> or <see cref="ClientStatus.Inactive"/>.
        /// </summary>
        [Required(ErrorMessage = "Every Client must have a status.")]
        public ClientStatus Status { get; set; }

        /// <summary>
        /// Free form notes for the client.
        /// </summary>
        [MaxLength(500, ErrorMessage = "The free notes for a Client must be less than 500 characters long.")]
        public string Notes { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}

