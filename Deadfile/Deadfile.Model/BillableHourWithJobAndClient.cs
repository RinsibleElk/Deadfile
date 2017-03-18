using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    public class BillableHourWithJobAndClient
    {
        public BillableHour BillableHour { get; set; }
        public Job Job { get; set; }
        public Client Client { get; set; }
    }
}
