using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// Represents the state of a planning application.
    /// </summary>
    public enum ApplicationState
    {
        Current,
        Refused,
        Approved,
        Withdrawn
    }
}
