using System;
using System.Collections.Generic;
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

        public int JobId { get; set; }
    }
}
