using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}
