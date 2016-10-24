using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Job
    {
        public int JobId { get; set; }

        public string AddressFirstLine { get; set; }

        public string AddressSecondLine { get; set; }

        public string AddressThirdLine { get; set; }

        public string AddressPostCode { get; set; }

        public JobStatus Status { get; set; }

        public string Notes { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
