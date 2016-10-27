using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "A Payment must have a creation date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A Payment must have a Gross amount.")]
        public double GrossAmount { get; set; }

        [Required(ErrorMessage = "A Payment must have a Net amount.")]
        public double NetAmount { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
