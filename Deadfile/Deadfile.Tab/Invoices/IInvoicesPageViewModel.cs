﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Invoices
{
    interface IInvoicesPageViewModel : IEditableItemViewModel<InvoiceModel>, IPageViewModel
    {
    }
}
