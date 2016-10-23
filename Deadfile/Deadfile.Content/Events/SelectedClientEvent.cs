using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Prism.Events;

namespace Deadfile.Content.Events
{
    /// <summary>
    /// Used to communicate between ContentRegion and BrowserRegion in the Clients experience.
    /// </summary>
    public class SelectedClientEvent : PubSubEvent<ClientModel>
    {
    }
}
