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
            set { SetProperty(ref _netAmount, value); }
        }

        private int _invoiceId = ModelBase.NewModelId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }

        private bool _deletePending;
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
