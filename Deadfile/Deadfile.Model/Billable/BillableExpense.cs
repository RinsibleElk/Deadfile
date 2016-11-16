using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// Leaf billable item representing an expense for a particular job.
    /// </summary>
    public class BillableExpense : BillableModel
    {
        public override BillableModelType ModelType
        {
            get { return BillableModelType.Expense; }
        }

        public override string Text { get { return "Expense (" + NetAmount + ")"; } }
    }
}
