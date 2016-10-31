using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    public class SaveMessage
    {
        public static readonly SaveMessage Instance = new SaveMessage();
    }

    public class SaveEvent : PubSubEvent<SaveMessage>
    {
        
    }
}
