using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Deadfile.Entity;
using Prism.Mvvm;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for a Client.
    /// </summary>
    public class ClientModel : BindableBase
    {
        private int clientId;
        /// <summary>
        /// This is Entity's model for this Client, or if it is not in the database yet then default.
        /// </summary>
        public int ClientId
        {
            get { return clientId; }
            set { SetProperty(ref clientId, value); }
        }

        private string title;
        /// <summary>
        /// Title, e.g. Mr, Mrs, Dr.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A Client's title must be less than 20 characters long.")]
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string firstName;
        /// <summary>
        /// First name.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's first name must be less than 50 characters long.")]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                SetProperty(ref firstName, value);
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string middleNames;
        /// <summary>
        /// Middle names.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's middle names must be less than 50 characters long.")]
        public string MiddleNames
        {
            get { return middleNames; }
            set
            {
                SetProperty(ref middleNames, value);
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string lastName;
        /// <summary>
        /// Last name. This is a required field. When the full name is not yet known, the last name should be used.
        /// </summary>
        [Required(ErrorMessage = "You must require at least a last name for this Client."),
         MaxLength(50, ErrorMessage = "A Client's last name must be less than 50 characters long.")]
        public string LastName
        {
            get { return lastName; }
            set
            {
                SetProperty(ref lastName, value);
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        public string FullName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(FirstName))
                    return $"{Title} {LastName}";
                return $"{FirstName} {LastName}";
            }
        }

        public string FullNameWithTitle
        {
            get
            {
                return string.Join(" ",
                    new string[] {Title, FirstName, MiddleNames, LastName}.Where((s) => !String.IsNullOrWhiteSpace(s)));
            }
        }

        private string addressFirstLine;
        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        public string AddressFirstLine
        {
            get { return addressFirstLine; }
            set { SetProperty(ref addressFirstLine, value); }
        }

        private string addressSecondLine;
        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        public string AddressSecondLine
        {
            get { return addressSecondLine; }
            set { SetProperty(ref addressSecondLine, value); }
        }

        private string addressThirdLine;
        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        public string AddressThirdLine
        {
            get { return addressThirdLine; }
            set { SetProperty(ref addressThirdLine, value); }
        }

        private string addressPostCode;
        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        public string AddressPostCode
        {
            get { return addressPostCode; }
            set { SetProperty(ref addressPostCode, value); }
        }

        private string phoneNumber1;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client."),
         Required(ErrorMessage = "Client requires a phone number.")]
        public string PhoneNumber1
        {
            get { return phoneNumber1; }
            set { SetProperty(ref phoneNumber1, value); }
        }

        private string phoneNumber2;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber2
        {
            get { return phoneNumber2; }
            set { SetProperty(ref phoneNumber2, value); }
        }

        private string phoneNumber3;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber3
        {
            get { return phoneNumber3; }
            set { SetProperty(ref phoneNumber3, value); }
        }

        private string emailAddress;
        [EmailAddress(ErrorMessage = "Invalid e-mail address given for this Client.")]
        public string EmailAddress
        {
            get { return emailAddress; }
            set { SetProperty(ref emailAddress, value); }
        }

        private ClientStatus status;
        [Required(ErrorMessage = "Every Client must have a status.")]
        public ClientStatus Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private string notes;

        [MaxLength(500, ErrorMessage = "The free notes for a Client must be less than 500 characters long.")]
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
    }
}