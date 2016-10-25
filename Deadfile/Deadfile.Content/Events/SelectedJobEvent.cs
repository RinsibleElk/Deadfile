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
    /// Used to communicate between ContentRegion and BrowserRegion.
    /// </summary>
    public class SelectedJobEvent : PubSubEvent<int>
    {
    }
}
