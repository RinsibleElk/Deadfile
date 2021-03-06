﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        void SetBillableItems();
        bool CanSetBillableItems { get; }
        bool InvoiceEditable { get; }
        double NetAmount { get; set; }
        double Hours { get; set; }
        ICommand AddItemCommand { get; }
        ObservableCollection<string> SuggestedInvoiceReferences { get; set; }
        bool VatRateEditable { get; }
    }
}
