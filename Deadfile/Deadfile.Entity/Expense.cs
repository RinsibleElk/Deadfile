using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for an Expense must be less than 500 characters long.")]
        public string Notes { get; set; }

        public int JobId { get; set; }
        public int? InvoiceId { get; set; }
    }
}
