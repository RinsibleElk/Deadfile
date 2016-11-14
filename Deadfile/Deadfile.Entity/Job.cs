using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Job
    {
        public int JobId { get; set; }

        [Required(ErrorMessage = "You must provide an address for this Job."),
         MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string AddressFirstLine { get; set; }

        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string AddressSecondLine { get; set; }

        [MaxLength(100, ErrorMessage = "A line of an address must be less than 100 characters long.")]
        public string AddressThirdLine { get; set; }

        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string AddressPostCode { get; set; }

        public JobStatus Status { get; set; }

        [MaxLength(500, ErrorMessage = "The free notes for a Job must be less than 500 characters long.")]
        public string Notes { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<JobInvoiceMapping> JobInvoiceMappings { get; set; }
    }
}
