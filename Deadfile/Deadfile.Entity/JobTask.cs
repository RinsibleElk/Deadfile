﻿using System;
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
         MinLength(5, ErrorMessage = "A JobTask must have at least 5 characters"),
         MaxLength(30, ErrorMessage = "A JobTask must have at most 30 characters")]
        public string Description { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a JobTask must be less than 500 characters long.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "A JobTask must have a due date.")]
        public DateTime DueDate { get; set; }

        public int JobId { get; set; }
    }
}
