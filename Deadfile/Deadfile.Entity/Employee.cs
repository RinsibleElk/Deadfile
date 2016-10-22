using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "This Employee requires a status.")]
        public EmployeeStatus Status { get; set; }
    }
}
