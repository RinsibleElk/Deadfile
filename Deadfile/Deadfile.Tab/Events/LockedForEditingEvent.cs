using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    public struct LockedForEditingMessage
    {
        public bool IsLocked { get; set; }
        public object NewParameters { get; set; }
    }

    public class LockedForEditingEvent : PubSubEvent<LockedForEditingMessage>
    {
        
    }
}
