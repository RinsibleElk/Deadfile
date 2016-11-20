using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Invoices
{
    interface IInvoicesPageViewModel : IEditableItemViewModel<InvoiceModel>, IPageViewModel
    {
        string FilterText { get; set; }
        ObservableCollection<BillableModel> Jobs { get; set; }
        void SetCompany();
        bool CanSetCompany { get; }
        void SetBillableItems();
        bool CanSetBillableItems { get; }
        bool InvoiceEditable { get; }
    }
}
