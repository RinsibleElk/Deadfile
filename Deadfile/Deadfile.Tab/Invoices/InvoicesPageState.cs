using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab.Invoices
{
    [Flags]
    internal enum InvoicesPageState
    {
        CanSave = 1,
        CanDelete = 2,
        CanEdit = 4,
        CanDiscard = 8,
        UnderEdit = 16
    }
}
