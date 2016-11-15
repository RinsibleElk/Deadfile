using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageDesignTimeViewModel : PageDesignTimeViewModel, IInvoicesPageViewModel
    {
        public InvoiceModel SelectedItem { get; set; } = new InvoiceModel(); //TODO get some interesting design time data
        public bool Editable { get; } = true;
        public List<string> Errors { get; } = new List<string>();
    }
}
