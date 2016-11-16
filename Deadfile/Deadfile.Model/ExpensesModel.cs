using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private string _description;
        [Required(ErrorMessage = "An Expense must be given a short description"),
         MinLength(5, ErrorMessage = "An Expense must have at least 5 characters"),
         MaxLength(30, ErrorMessage = "An Expense must have at most 30 characters")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private double _netAmount;
        [Required(ErrorMessage = "An Expense must have an amount associated")]
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for an Expense must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }
    }
}
