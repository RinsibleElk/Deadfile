using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// State tracking whether a given job is still active or whether it has completed its lifecycle, either by
    /// being cancelled or completed.
    /// </summary>
    public enum JobStatus
    {
        Active,
        Cancelled,
        Completed
    }
}
