using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// Represents whether this Client is still active or inactive.
    /// </summary>
    /// <remarks>
    /// This can be used to provide a filtered view to the user showing only active Clients.
    /// </remarks>
    public enum ClientStatus
    {
        Active,
        Inactive
    }
}
