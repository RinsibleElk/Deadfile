using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for an Invoice.
    /// </summary>
    public class InvoiceModel : ParentModelBase<InvoiceItemModel>
    {
        public override int Id
        {
            get { return InvoiceId; }
            set { InvoiceId = value; }
        }

        private int _invoiceId = ModelBase.NewModelId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }

        private DateTime _createdDate = DateTime.Today;
        [Required(ErrorMessage = "An Invoice must have a creation date.")]
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { SetProperty(ref _createdDate, value); }
        }

        private double _grossAmount = 0;
        public double GrossAmount
        {
            get { return _grossAmount; }
            set
            {
                // Undo and validation not supported
                if (object.Equals((object)_grossAmount, (object)value)) return;
                _grossAmount = value;
                OnPropertyChanged(nameof(GrossAmount));
            }
        }

        private double _netAmount = 0;
        public double NetAmount
        {
            get { return _netAmount; }
            set
            {
                // Undo and validation not supported
                if (object.Equals((object)_netAmount, (object)value)) return;
                _netAmount = value;
                OnPropertyChanged(nameof(NetAmount));
                VatValue = VatRate * NetAmount / 100;
                GrossAmount = NetAmount + VatValue;
            }
        }

        private InvoiceStatus _status;
        [Required(ErrorMessage = "The Invoice requires a Status.")]
        public InvoiceStatus Status
        {
            get { return _status; }
            set
            {
                // revalidate the invoice reference
                if (SetProperty(ref _status, value))
                    ValidateProperty(nameof(InvoiceReference), _invoiceReference);
            }
        }

        private int _invoiceReference = 0;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set
            {
                if (SetProperty(ref _invoiceReference, value))
                    _invoiceReferenceString = value.ToString();
            }
        }

        private string _invoiceReferenceString = "0";
        [CustomValidation(typeof(InvoiceModelInvoiceReferenceValidator), nameof(InvoiceModelInvoiceReferenceValidator.InvoiceReferenceIsValid)),
         RegularExpression("[1-9][0-9]*", ErrorMessage = "The invoice reference should be a number")]
        public string InvoiceReferenceString
        {
            get { return _invoiceReferenceString; }
            set
            {
                int intValue;
                if (SetProperty(ref _invoiceReferenceString, value) && Int32.TryParse(value, out intValue))
                {
                    DisableUndoTracking = true;
                    InvoiceReference = intValue;
                    DisableUndoTracking = false;
                }
            }
        }

        public IDeadfileRepository Repository { get; set; } = null;

        internal bool InvoiceReferenceIsUniqueForCompany()
        {
            // Just creating.
            if (Repository == null)
                return true;
            return Repository.HasUniqueInvoiceReference(this);
        }

        private Company _company = Company.Imagine3DLtd;
        [Required(ErrorMessage = "An Invoice requires a Company that it has been issued for.")]
        public Company Company
        {
            get { return _company; }
            set
            {
                // revalidate the invoice reference
                if (SetProperty(ref _company, value))
                {
                    ValidateProperty(nameof(InvoiceReference), _invoiceReference);
                    DisableUndoTracking = true;
                    VatRate = (_company == Company.PaulSamsonCharteredSurveyorLtd) ? 20 : 0;
                    DisableUndoTracking = false;
                }
            }
        }

        private InvoiceCreationState _creationState = InvoiceCreationState.DefineBillables;
        public InvoiceCreationState CreationState
        {
            get { return _creationState; }
            set { SetProperty(ref _creationState, value); }
        }

        private string _clientName;
        [Required(ErrorMessage = "An Invoice requires a Client Name."),
         MinLength(1, ErrorMessage = "Client Name must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Name must be at most 100 characters long.")]
        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

        private string _clientAddressFirstLine;
        /// <summary>
        /// First line of the Client's contact address. This is required.
        /// </summary>
        [Required(ErrorMessage = "You must provide an address for this Client."),
         MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressFirstLine
        {
            get { return _clientAddressFirstLine; }
            set { SetProperty(ref _clientAddressFirstLine, value); }
        }

        private string _clientAddressSecondLine;
        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressSecondLine
        {
            get { return _clientAddressSecondLine; }
            set { SetProperty(ref _clientAddressSecondLine, value); }
        }

        private string _clientAddressThirdLine;
        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string ClientAddressThirdLine
        {
            get { return _clientAddressThirdLine; }
            set { SetProperty(ref _clientAddressThirdLine, value); }
        }

        private string _clientAddressPostCode;
        /// <summary>
        /// Post code for the Client's contact address.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string ClientAddressPostCode
        {
            get { return _clientAddressPostCode; }
            set { SetProperty(ref _clientAddressPostCode, value); }
        }

        private string _project;
        [Required(ErrorMessage = "An Invoice must have a Project."),
         MinLength(1, ErrorMessage = "A Project must be at least 1 character long."),
         MaxLength(200, ErrorMessage = "A Project must be at most 200 characters long.")]
        public string Project
        {
            get { return _project; }
            set { SetProperty(ref _project, value); }
        }

        private string _description;
        [Required(ErrorMessage = "An Invoice must have a Description."),
         MaxLength(200, ErrorMessage = "A Description must be at most 200 characters long.")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private double _vatRate;
        public double VatRate
        {
            get { return _vatRate; }
            set
            {
                if (SetProperty(ref _vatRate, value))
                {
                    VatValue = VatRate*NetAmount/100;
                    GrossAmount = NetAmount + VatValue;
                }
            }
        }

        private double _vatValue;
        public double VatValue
        {
            get { return _vatValue; }
            set
            {
                // Undo and validation not supported
                if (object.Equals((object)_vatValue, (object)value)) return;
                _vatValue = value;
                OnPropertyChanged(nameof(VatValue));
                SetProperty(ref _vatValue, value);
            }
        }

        private int _clientId;
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        public override List<InvoiceItemModel> ChildrenList { get; set; } = new List<InvoiceItemModel>();
        public bool IsNewInvoice { get; set; }

        public override void ChildrenUpdated()
        {
            base.ChildrenUpdated();

            foreach (var child in ChildrenList)
            {
                child.PropertyChanged -= ChildPropertyChanged;
                child.PropertyChanged += ChildPropertyChanged;
            }

            RecalculateNetAmount();
        }

        private void RecalculateNetAmount()
        {
            var netAmount = 0.0;
            foreach (var invoiceItemModel in ChildrenList)
            {
                if (!invoiceItemModel.DeletePending)
                {
                    netAmount += invoiceItemModel.NetAmount;
                }
            }
            NetAmount = netAmount;
        }

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InvoiceItemModel.NetAmount))
            {
                RecalculateNetAmount();
            }
        }
    }
}
