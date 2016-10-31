using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;

namespace Deadfile.Tab.Quotes
{
    interface IQuotesBarViewModel
    {
        QuotationModel Quotation { get; set; }
        ICommand NextQuotationCommand { get; }
    }
}
