using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Reporting
{
    /// <summary>
    /// Representation of an expense or billable hours.
    /// </summary>
    public class UnbilledItemModel : ModelBase
    {
        private int _id;
        public override int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int ExpenseId { get { return Id; } set { Id = value; } }

        public int BillableHourId { get { return Id; } set { Id = value; } }

        private int _hoursWorked = 0;
        public int HoursWorked
        {
            get { return _hoursWorked; }
            set { SetProperty(ref _hoursWorked, value); }
        }

        private double _netAmount = 0.0;
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }
    }
}
