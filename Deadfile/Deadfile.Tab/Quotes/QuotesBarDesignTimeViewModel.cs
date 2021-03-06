﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;

namespace Deadfile.Tab.Quotes
{
    class QuotesBarDesignTimeViewModel : IQuotesBarViewModel
    {
        public QuotationModel Quotation { get; set; } = new QuotationModel()
        {
            Author = "Homer Simpson",
            Phrase = "You tried your best and you failed miserably. The lesson is: Never try."
        };

        public ICommand NextQuotationCommand { get; } = null;
    }
}
