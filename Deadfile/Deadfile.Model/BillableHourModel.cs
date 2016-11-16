using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public class BillableHourModel : JobChildModelBase
    {
        // No need to report changes or validate.
        public override int Id
        {
            get { return BillableHourId; }
            set { BillableHourId = value; }
        }

        private int _billableHourId = ModelBase.NewModelId;
        public int BillableHourId
        {
            get { return _billableHourId; }
            set { SetProperty(ref _billableHourId, value); }
        }

        private string _description;
        [Required(ErrorMessage = "A BillableHour must be given a short description"),
         MinLength(5, ErrorMessage = "A BillableHour must have at least 5 characters"),
         MaxLength(30, ErrorMessage = "A BillableHour must have at most 30 characters")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private double _netAmount;
        [Required(ErrorMessage = "A BillableHour must have an amount associated")]
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for a BillableHour must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private DateTime _creationDate = DateTime.Today;
        [Required(ErrorMessage = "A BillableHour must have a creation date.")]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }
    }
}
