using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Core;
using Prism.Mvvm;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for a Quotation.
    /// </summary>
    public sealed class QuotationModel : ModelBase
    {
        private int _quotationId = ModelBase.NewModelId;
        public int QuotationId
        {
            get { return _quotationId; }
            set { SetProperty(ref _quotationId, value); }
        }

        private string _phrase;
        [MaxLength(200, ErrorMessage = "Quotation phrases must be no more than 200 characters."), Required(ErrorMessage = "Quotations must have a phrase.")]
        public string Phrase
        {
            get { return _phrase; }
            set { SetProperty(ref _phrase, value); }
        }

        private string _author;
        [MaxLength(32, ErrorMessage = "Quotation authors must be no more than 32 characters."), Required(ErrorMessage = "Quotations must have an author.")]
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }

        public override int Id
        {
            get { return QuotationId; }
            set { QuotationId = value; }
        }
    }
}
