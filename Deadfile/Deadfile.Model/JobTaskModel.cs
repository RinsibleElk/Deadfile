using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public class JobTaskModel : JobChildModelBase
    {
        public override int Id
        {
            get { return JobTaskId; }
            set { JobTaskId = value; }
        }

        private int _jobTaskId;
        public int JobTaskId
        {
            get { return _jobTaskId; }
            set { SetProperty(ref _jobTaskId, value); }
        }

        private string _description;
        [Required(ErrorMessage = "A JobTask must be given a short description"),
         MinLength(5, ErrorMessage = "A JobTask must have at least 5 characters"),
         MaxLength(30, ErrorMessage = "A JobTask must have at most 30 characters")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (SetProperty(ref _description, value))
                    OnPropertyChanged(nameof(HeaderText));
            }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for a JobTask must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private DateTime _dueDate = DateTime.Today;
        [Required(ErrorMessage = "A JobTask must have a due date.")]
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (SetProperty(ref _dueDate, value))
                    OnPropertyChanged(nameof(HeaderText));
            }
        }

        public string HeaderText
        {
            get
            {
                if (Description == null)
                {
                    return "Add New...";
                }
                return $"{Description} ({DueDate:dd/MM/yyyy})";
            }
        }
    }
}
