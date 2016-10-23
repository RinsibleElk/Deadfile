using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for a Quotation.
    /// </summary>
    public sealed class QuotationModel : BindableBase
    {
        private string _phrase;
        public string Phrase
        {
            get { return _phrase; }
            set { SetProperty(ref _phrase, value); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }
    }
}
