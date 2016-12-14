using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Deadfile.Entity;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for a Client.
    /// </summary>
    public class ClientModel : StateManagedModelBase
    {
        public override int Id
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        private int _clientId = ModelBase.NewModelId;
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
        [Required(ErrorMessage = "You must provide at least a last name for this Client."),
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
                    if (String.IsNullOrWhiteSpace(Title)) return LastName;
                    else return $"{Title} {LastName}";
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

        private string _company;
        /// <summary>
        /// Company.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A Client's Company must be 100 characters or fewer.")]
        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
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
        [Required(ErrorMessage = "You must provide an address for this Client."),
         MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
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
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
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
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
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
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
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
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber1
        {
            get { return _phoneNumber1; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber1, String.IsNullOrEmpty(value) ? null : value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _phoneNumber2;
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber2
        {
            get { return _phoneNumber2; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber2, String.IsNullOrEmpty(value) ? null : value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _phoneNumber3;
        [Phone(ErrorMessage = "Not a valid phone number for Client.")]
        public string PhoneNumber3
        {
            get { return _phoneNumber3; }
            set
            {
                OnPropertyChanging(nameof(PhoneNumbers));
                SetProperty(ref _phoneNumber3, String.IsNullOrEmpty(value) ? null : value);
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private string _emailAddress;
        [EmailAddress(ErrorMessage = "Invalid e-mail address given for this Client.")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                // This is to get cleared e-mails to be free of validation.
                var valueToSet = (value == "") ? null : value;
                SetProperty(ref _emailAddress, valueToSet);
            }
        }

        private ClientStatus _status;
        [Required(ErrorMessage = "Every Client must have a status.")]
        public ClientStatus Status
        {
            get { return _status; }
            set
            {
                if (SetProperty(ref _status, value))
                    OnPropertyChanged(nameof(StateIsActive));
            }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for a Client must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public override bool StateIsActive => Status == ClientStatus.Active;
    }
}