using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// The name is for consistency. This can represent multiple items.
    /// </summary>
    public class BillableHour
    {
        public int BillableHourId { get; set; }

        [Required(ErrorMessage = "A BillableHour must be given a short description"),
         MaxLength(100, ErrorMessage = "A BillableHour must have at most 100 characters.")]
        public string Description { get; set; }

        [MaxLength(100, ErrorMessage = "The person that worked these hours must be at most 100 characters.")]
        public string Person { get; set; }

        [Required(ErrorMessage = "You must supply the number of hours worked.")]
        public int HoursWorked { get; set; } = 0;

        [Required(ErrorMessage = "A BillableHour must have an amount associated")]
        public double NetAmount { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a BillableHour must be less than 500 characters long.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "A BillableHour must have a creation date.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "A BillableHour must have a State")]
        public BillableState State { get; set; } = BillableState.Active;

        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        public int? InvoiceId { get; set; }
    }
}
