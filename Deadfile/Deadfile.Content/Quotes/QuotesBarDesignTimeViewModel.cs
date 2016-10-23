using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Model;

namespace Deadfile.Content.Quotes
{
    class QuotesBarDesignTimeViewModel : IQuotesBarViewModel
    {
        public QuotationModel Quotation { get; set; } = new QuotationModel()
        {
            Author = "Homer Simpson",
            Phrase = "You tried your best and you failed miserably. The lesson is: Never try."
        };
    }
}
