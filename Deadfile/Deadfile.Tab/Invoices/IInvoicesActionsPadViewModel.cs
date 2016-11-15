using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;

namespace Deadfile.Tab.Invoices
{
    interface IInvoicesActionsPadViewModel : IActionsPadViewModel
    {
        bool CanPrintItem { get; set; }
        bool PrintItemIsVisible { get; set; }
    }
}
