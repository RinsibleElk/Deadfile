using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class JobTask
    {
        public int JobTaskId { get; set; }

        [Required(ErrorMessage = "A JobTask must be given a short description"),
         MaxLength(100, ErrorMessage = "A JobTask must have at most 100 characters")]
        public string Description { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a JobTask must be less than 500 characters long.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "A JobTask must have a due date.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "A Job Task requires a state.")]
        public JobTaskState State { get; set; }

        [Required(ErrorMessage = "A Job Task requires a priority.")]
        public JobTaskPriority Priority { get; set; }

        public int ClientId { get; set; }

        public int JobId { get; set; }
    }
}
