using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;

namespace Deadfile.Tab.Invoices
{
    class InvoicesActionsPadDesignTimeViewModel : ActionsPadDesignTimeViewModel, IInvoicesActionsPadViewModel
    {
        public bool CanPrintItem { get; } = true;
        public bool CanPaidItem { get; } = true;
    }
}
