using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab.Clients
{
    [Flags]
    internal enum ClientsPageState
    {
        CanSave = 1,
        CanEdit = 2,
        CanDelete = 4,
        CanDiscard = 8,
        UnderEdit = 16
    }
}
