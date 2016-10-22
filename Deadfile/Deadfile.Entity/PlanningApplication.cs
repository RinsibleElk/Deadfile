using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class PlanningApplication
    {
        public int PlanningApplicationId { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
