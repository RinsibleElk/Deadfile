using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    public class NavigateMessage
    {
        public NavigateMessage(Experience experience)
        {
            Experience = experience;
        }
        public Experience Experience { get; set; }
    }

    public class NavigateEvent : PubSubEvent<NavigateMessage>
    {
        
    }
}
