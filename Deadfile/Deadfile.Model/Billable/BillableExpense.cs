using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// Leaf billable item representing an expense for a particular job.
    /// </summary>
    public class BillableExpense : BillableModel
    {
        private int _expenseId;
        public int ExpenseId
        {
            get { return _expenseId; }
            set { SetProperty(ref _expenseId, value); }
        }

        public override BillableModelType ModelType => BillableModelType.Expense;

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (SetProperty(ref _description, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        private ExpenseType _type;
        public ExpenseType Type
        {
            get { return _type; }
            set
            {
                if (SetProperty(ref _type, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        public override string Text => $"{Type} - {Description} ({NetAmount:C})";

        public override int Id
        {
            get { return ExpenseId; }
            set { ExpenseId = value; }
        }
    }
}
