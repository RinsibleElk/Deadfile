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
        public bool CanPrintItem { get; set; } = true;
        public bool PrintItemIsVisible { get; set; } = true;
        public bool CanPaidItem { get; set; } = true;
        public bool PaidItemIsVisible { get; set; } = true;
    }
}
