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
         MinLength(5, ErrorMessage = "A BillableHour must have at least 5 characters"),
         MaxLength(30, ErrorMessage = "A BillableHour must have at most 30 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A BillableHour must have an amount associated")]
        public double NetAmount { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a BillableHour must be less than 500 characters long.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "A BillableHour must have a creation date.")]
        public DateTime CreationDate { get; set; }

        public int JobId { get; set; }
        public int? InvoiceId { get; set; }
    }
}
