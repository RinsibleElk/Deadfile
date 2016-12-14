using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// The priority of a job task. Default is medium. This affects ordering and coloring in the TodoReport.
    /// </summary>
    public enum JobTaskPriority
    {
        Low,
        Medium,
        High
    }
}
