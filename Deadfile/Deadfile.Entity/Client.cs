using System;
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
        [Required(ErrorMessage = "You must require at least a last name for this Client."),
         MaxLength(50, ErrorMessage = "A Client's last name must be less than 50 characters long.")]
        public string LastName { get; set; }

        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        public string AddressFirstLine { get; set; }

        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        public string AddressSecondLine { get; set; }

        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        public string AddressThirdLine { get; set; }

        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        public string AddressPostCode { get; set; }

        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 20 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client."),
         Required(ErrorMessage = "Client requires a phone number.")]
        public string PhoneNumber1 { get; set; }

        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber2 { get; set; }

        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber3 { get; set; }

        [EmailAddress(ErrorMessage = "Invalid e-mail address given for this Client.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Every Client must have a status.")]
        public ClientStatus Status { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a Client must be less than 500 characters long.")]
        public string Notes { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}

