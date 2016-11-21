using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for an Invoice.
    /// </summary>
    public class InvoiceModel : ModelBase
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
            set { SetProperty(ref _grossAmount, value); }
        }

        private double _netAmount = 0;
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private InvoiceStatus _status;
        [Required(ErrorMessage = "The Invoice requires a Status.")]
        public InvoiceStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private int _invoiceReference = ModelBase.NewModelId;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set { SetProperty(ref _invoiceReference, value); }
        }

        private Company _company = Company.PaulSamsonCharteredSurveyorLtd;
        [Required(ErrorMessage = "An Invoice requires a Company that it has been issued for.")]
        public Company Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }

        private InvoiceCreationState _creationState = InvoiceCreationState.DefineCompany;
        public InvoiceCreationState CreationState
        {
            get { return _creationState; }
            set { SetProperty(ref _creationState, value); }
        }

        private string _clientName;
        [Required(ErrorMessage = "An Invoice requires a Client Name."),
         MinLength(1, ErrorMessage = "Client Name must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Name must be at most 100 character long.")]
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
         MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string ClientAddressFirstLine
        {
            get { return _clientAddressFirstLine; }
            set { SetProperty(ref _clientAddressFirstLine, value); }
        }

        private string _clientAddressSecondLine;
        /// <summary>
        /// Second line of the Client's contact address.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string ClientAddressSecondLine
        {
            get { return _clientAddressSecondLine; }
            set { SetProperty(ref _clientAddressSecondLine, value); }
        }

        private string _clientAddressThirdLine;
        /// <summary>
        /// Third line of the Client's contact address.
        /// </summary>
        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
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
         MaxLength(100, ErrorMessage = "A Project must be at most 100 characters long.")]
        public string Project
        {
            get { return _project; }
            set { SetProperty(ref _project, value); }
        }

        private string _description;
        [MaxLength(100, ErrorMessage = "A Description must be at most 100 characters long.")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private ObservableCollection<InvoiceItemModel> _invoiceItemModels = new ObservableCollection<InvoiceItemModel>();
        public ObservableCollection<InvoiceItemModel> InvoiceItemModels
        {
            get { return _invoiceItemModels; }
            set { SetProperty(ref _invoiceItemModels, value); }
        }

        private ObservableCollection<JobInvoiceMappingModel> _jobInvoiceMappingModels = new ObservableCollection<JobInvoiceMappingModel>();
        public ObservableCollection<JobInvoiceMappingModel> JobInvoiceMappingModels
        {
            get { return _jobInvoiceMappingModels; }
            set { SetProperty(ref _jobInvoiceMappingModels, value); }
        }
    }
}
