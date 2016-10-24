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
    public class ClientModel : ValidatableBindableBase
    {
        /// <summary>
        /// Deliberately invalid id to represent to the EditClient experience that a new client is required.
        /// </summary>
        public const int NewClientId = Int32.MinValue;

        private int _clientId;
        /// <summary>
        /// This is Entity's model for this Client, or if it is not in the database yet then default.
        /// </summary>
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        private string _title;
        /// <summary>
        /// Title, e.g. Mr, Mrs, Dr.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A Client's title must be less than 20 characters long.")]
        public string Title
        {
            get { return _title; }
            set
            {
                OnPropertyChanging(nameof(FullName));
                OnPropertyChanging(nameof(FullNameWithTitle));
                SetProperty(ref _title, value);
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string _firstName;
        /// <summary>
        /// First name.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's first name must be less than 50 characters long.")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                OnPropertyChanging(nameof(FullName));
                OnPropertyChanging(nameof(FullNameWithTitle));
                SetProperty(ref _firstName, value);
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string _middleNames;
        /// <summary>
        /// Middle names.
        /// </summary>
        [MaxLength(50, ErrorMessage = "A Client's middle names must be less than 50 characters long.")]
        public string MiddleNames
        {
            get { return _middleNames; }
            set
            {
                OnPropertyChanging(nameof(FullNameWithTitle));
                SetProperty(ref _middleNames, value);
                OnPropertyChanged(nameof(FullNameWithTitle));
            }
        }

        private string _lastName;
        /// <summary>
        /// Last name. This is a required field. When the full name is not yet known, the last name should be used.
        /// </summary>
        [Required(ErrorMessage = "You must require at least a last name for this Client."),
         MaxLength(50, ErrorMessage = "A Client's last name must be less than 50 characters long.")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                OnPropertyChanging(nameof(FullName));
                OnPropertyChanging(nameof(FullNameWithTitle));
                SetProperty(ref _lastName, value);
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

        public string FullAddress
        {
            get
            {
                return string.Join(Environment.NewLine,
                    new string[] {AddressFirstLine, AddressSecondLine, AddressThirdLine, AddressPostCode}.Where(
                        (s) => !String.IsNullOrWhiteSpace(s)));
            }
        }

        private string _addressFirstLine;
        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        public string AddressFirstLine
        {
            get { return _addressFirstLine; }
            set
            {
                OnPropertyChanging(nameof(FullAddress));
                SetProperty(ref _addressFirstLine, value);
                OnPropertyChanged(nameof(FullAddress));
            }
        }

        private string _addressSecondLine;
        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        public string AddressSecondLine
        {
            get { return _addressSecondLine; }
            set
            {
                OnPropertyChanging(nameof(FullAddress));
                SetProperty(ref _addressSecondLine, value);
                OnPropertyChanged(nameof(FullAddress));
            }
        }

        private string _addressThirdLine;
        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        public string AddressThirdLine
        {
            get { return _addressThirdLine; }
            set
            {
                OnPropertyChanging(nameof(FullAddress));
                SetProperty(ref _addressThirdLine, value);
                OnPropertyChanged(nameof(FullAddress));
            }
        }

        private string _addressPostCode;
        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        public string AddressPostCode
        {
            get { return _addressPostCode; }
            set
            {
                OnPropertyChanging(nameof(FullAddress));
                SetProperty(ref _addressPostCode, value);
                OnPropertyChanged(nameof(FullAddress));
            }
        }

        public string PhoneNumbers
        {
            get
            {
                return string.Join(Environment.NewLine,
                    new string[] {PhoneNumber1, PhoneNumber2, PhoneNumber3}.Where(
                        (s) => !String.IsNullOrWhiteSpace(s)));
            }
        }

        private string _phoneNumber1;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client."),
         Required(ErrorMessage = "Client requires a phone number.")]
        public string PhoneNumber1
        {
            get { return _phoneNumber1; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber1, value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _phoneNumber2;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber2
        {
            get { return _phoneNumber2; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber2, value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _phoneNumber3;
        [MinLength(8, ErrorMessage = "The minimum length for a Client's phone number is 8 characters."),
         MaxLength(20, ErrorMessage = "The maximum length for a Client's phone number is 8 characters."),
         Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber3
        {
            get { return _phoneNumber3; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber3, value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _emailAddress;
        [EmailAddress(ErrorMessage = "Invalid e-mail address given for this Client.")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }

        private ClientStatus _status;
        [Required(ErrorMessage = "Every Client must have a status.")]
        public ClientStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for a Client must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }
    }
}