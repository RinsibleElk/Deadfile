using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

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

        private string _clientFullName;
        public string ClientFullName
        {
            get { return _clientFullName; }
            set { SetProperty(ref _clientFullName, value); }
        }

        private string _property;
        public string Property
        {
            get { return _property; }
            set { SetProperty(ref _property, value); }
        }

        private string _description;
        [Required(ErrorMessage = "A JobTask must be given a short description"),
         MaxLength(100, ErrorMessage = "A JobTask must have at most 100 characters")]
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

        private JobTaskState _state;
        [Required(ErrorMessage = "A Job Task requires a state.")]
        public JobTaskState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private int _someRandomField = 0;
        public int SomeRandomField
        {
            get { return _someRandomField; }
            set { SetProperty(ref _someRandomField, value); }
        }

        private JobTaskPriority _priority = JobTaskPriority.Medium;
        [Required(ErrorMessage = "A Job Task requires a priority.")]
        public JobTaskPriority Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
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
