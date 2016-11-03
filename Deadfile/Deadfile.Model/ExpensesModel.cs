using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    /// <summary>
    /// UI representation of an <see cref="Expense"/>.
    /// </summary>
    public class ExpenseModel : ModelBase
    {
        // No need to report changes or validate.
        public override int Id
        {
            get { return ExpenseId; }
            set { ExpenseId = value; }
        }

        private int _expenseId = ModelBase.NewModelId;
        public int ExpenseId
        {
            get { return _expenseId; }
            set { SetProperty(ref _expenseId, value); }
        }
    }
}
