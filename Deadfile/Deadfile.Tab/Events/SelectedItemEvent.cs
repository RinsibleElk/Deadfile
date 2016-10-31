using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Browser;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    internal struct SelectedItemPacket
    {
        public SelectedItemPacket(BrowserModelType type, int id)
        {
            Type = type;
            Id = id;
        }
        public BrowserModelType Type { get; private set; }
        public int Id { get; private set; }
    }
    /// <summary>
    /// Used to communicate between ContentRegion and BrowserRegion.
    /// </summary>
    internal class SelectedItemEvent : PubSubEvent<SelectedItemPacket>
    {
    }
}
