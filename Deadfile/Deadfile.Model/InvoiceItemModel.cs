using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace Deadfile.Model
{
    public class InvoiceItemModel : ChildModelBase
    {
        public InvoiceItemModel()
        {
            DeleteCommand = new DelegateCommand(Delete);
        }

        public override int Id
        {
            get { return InvoiceItemId; }
            set { InvoiceItemId = value; }
        }

        public override int ParentId
        {
            get { return InvoiceId; }
            set { InvoiceId = value; }
        }

        private int _invoiceItemId = ModelBase.NewModelId;
        public int InvoiceItemId
        {
            get { return _invoiceItemId; }
            set { SetProperty(ref _invoiceItemId, value); }
        }

        private string _description;
        [Required(ErrorMessage = "An InvoiceItem requires a Description"),
         MinLength(5, ErrorMessage = "Description must be at least 5 characters long"),
         MaxLength(200, ErrorMessage = "Description must be at most 200 characters long")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private double _netAmount;
        [Required(ErrorMessage = "An Invoice Item must have a value associated")]
        public double NetAmount
        {
            get { return _netAmount; }
            set
            {
                if (SetProperty(ref _netAmount, value))
                {
                    VatValue = (_includeVat ? (_vatRate * _netAmount / 100) : 0);
                }
            }
        }

        private double _vatValue = 0;
        [Required(ErrorMessage = "An Invoice Item must have a VAT value associated")]
        public double VatValue
        {
            get { return _vatValue; }
            set
            {
                // Undo and validation not supported
                if (object.Equals((object)_vatValue, (object)value)) return;
                _vatValue = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "An Invoice Item must have a VAT rate associated")]
        public double VatRate
        {
            get { return _vatRate; }
            set
            {
                if (SetProperty(ref _vatRate, value))
                {
                    VatValue = (_includeVat ? (_vatRate * _netAmount / 100) : 0);
                }
            }
        }

        [Required(ErrorMessage = "An Invoice Item requires Include VAT to be defined")]
        public bool IncludeVat
        {
            get { return _includeVat; }
            set
            {
                if (SetProperty(ref _includeVat, value))
                {
                    VatValue = (_includeVat ? (_vatRate * _netAmount / 100) : 0);
                }
            }
        }

        private int _invoiceId = ModelBase.NewModelId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }

        private bool _deletePending;
        private double _vatRate;
        private bool _includeVat;

        public override bool DeletePending
        {
            get { return _deletePending; }
            set { SetProperty(ref _deletePending, value); }
        }

        public ICommand DeleteCommand { get; }

        private void Delete()
        {
            DeletePending = true;
        }
    }
}
