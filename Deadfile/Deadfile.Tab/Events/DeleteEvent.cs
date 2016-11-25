using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    public class DeleteMessage
    {
        public object Window { get; }
        public DeleteMessage(object window)
        {
            Window = window;
        }
    }

    public class DeleteEvent : PubSubEvent<DeleteMessage>
    {

    }
}
