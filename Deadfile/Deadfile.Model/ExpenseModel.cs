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
    public class ExpenseModel : JobChildModelBase
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
         MaxLength(100, ErrorMessage = "An Expense description must have at most 100 characters")]
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

        private ExpenseType _type;
        [Required(ErrorMessage = "An Expense must have a Type")]
        public ExpenseType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for an Expense must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private DateTime _creationDate = DateTime.Today;
        [Required(ErrorMessage = "An Expense must have a creation date.")]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }
    }
}
