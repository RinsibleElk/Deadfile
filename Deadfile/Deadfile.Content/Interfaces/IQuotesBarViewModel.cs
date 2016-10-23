using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Content.Interfaces
{
    interface IQuotesBarViewModel
    {
        QuotationModel Quotation { get; set; }
    }
}
