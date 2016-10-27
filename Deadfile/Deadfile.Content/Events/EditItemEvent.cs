using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Content.Events
{
    internal enum EditAction
    {
        Add,
        StartEditing,
        EndEditing
    }
    internal class EditItemEvent : PubSubEvent<EditAction>
    {
    }
}
