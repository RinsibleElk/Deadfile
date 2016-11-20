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

        private string _clientAddress;

        [Required(ErrorMessage = "An Invoice requires a Client Address."),
         MinLength(1, ErrorMessage = "Client Address must be at least 1 character long."),
         MaxLength(100, ErrorMessage = "Client Address must be at most 100 character long.")]
        public string ClientAddress
        {
            get { return _clientAddress; }
            set { SetProperty(ref _clientAddress, value); }
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

        private ObservableCollection<InvoiceItemModel> _activeItems = new ObservableCollection<InvoiceItemModel>();
        public ObservableCollection<InvoiceItemModel> ActiveItems
        {
            get { return _activeItems; }
            set { SetProperty(ref _activeItems, value); }
        }

        private ObservableCollection<InvoiceItemModel> _inactiveItems = new ObservableCollection<InvoiceItemModel>();
        public ObservableCollection<InvoiceItemModel> InactiveItems
        {
            get { return _inactiveItems; }
            set { SetProperty(ref _inactiveItems, value); }
        }
    }
}
