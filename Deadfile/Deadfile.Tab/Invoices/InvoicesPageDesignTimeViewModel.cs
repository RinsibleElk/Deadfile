using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageDesignTimeViewModel : PageDesignTimeViewModel, IInvoicesPageViewModel
    {
        public InvoicesPageDesignTimeViewModel()
        {
            // Interesting fake data to look at.
            SelectedItem = new InvoiceModel();
            SelectedItem.Company = Company.Imagine3DLtd;
            SelectedItem.CreatedDate = new DateTime(2016, 11, 15);
            SelectedItem.GrossAmount = 100;
            SelectedItem.NetAmount = 100;
            SelectedItem.InvoiceId = 1357;
            SelectedItem.Id = 1357;
            SelectedItem.InvoiceReference = 53;
            SelectedItem.Status = InvoiceStatus.Created;

            Jobs = new ObservableCollection<BillableModel>();
            var opr = new BillableJob()
            {
                FullAddress = "112 Old Park Road",
                JobId = 1314,
                State = BillableModelState.Excluded
            };
            opr.Children.Add(new BillableApplication() {});
            Jobs.Add(opr);
            Jobs.Add(new BillableJob()
            {
                FullAddress = "61 Grange Park Avenue",
                JobId = 5256,
                State = BillableModelState.PartiallyIncluded
            });
            Jobs.Add(new BillableJob()
            {
                FullAddress = "87 The Chine",
                JobId = 223,
                State = BillableModelState.FullyIncluded
            });
        }

        public InvoiceModel SelectedItem { get; set; }

        public bool Editable { get; } = true;
        public List<string> Errors { get; } = new List<string>();
        public string FilterText { get; set; } = "";
        public ObservableCollection<BillableModel> Jobs { get; set; }
        public void SetCompany()
        {
            throw new NotImplementedException();
        }
        public void SetBillableItems()
        {
            throw new NotImplementedException();
        }

        public bool CanSetBillableItems { get; } = false;
        public bool InvoiceEditable { get; } = false;
        public double NetAmount { get; set; } = 365.0;
        public ICommand AddItemCommand { get; } = null;

        public ObservableCollection<int> SuggestedInvoiceReferences { get; set; } = new ObservableCollection<int>(new int[] {57, 65});
    }
}
