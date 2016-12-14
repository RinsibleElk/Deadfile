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
        public SelectedItemPacket(BrowserModelType type, int parentId, int id)
        {
            Type = type;
            ParentId = parentId;
            Id = id;
        }
        public BrowserModelType Type { get; }
        public int ParentId { get; }
        public int Id { get; }

        public override string ToString()
        {
            return $"[Type={Type}, ParentId={ParentId}, Id={Id}]";
        }
    }
    /// <summary>
    /// Used to communicate between ContentRegion and BrowserRegion.
    /// </summary>
    internal class SelectedItemEvent : PubSubEvent<SelectedItemPacket>
    {
    }
}
