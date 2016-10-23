using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Quotation
    {
        public int QuotationId { get; set; }
        [MaxLength(200, ErrorMessage = "Quotation phrases must be no more than 200 characters."), Required(ErrorMessage = "Quotations must have a phrase.")]
        public string Phrase { get; set; }
        [MaxLength(32, ErrorMessage = "Quotation authors must be no more than 32 characters."), Required(ErrorMessage = "Quotations must have an author.")]
        public string Author { get; set; }
    }
}
