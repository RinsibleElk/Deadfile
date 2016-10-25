using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Events
{
    /// <summary>
    /// Raised to communicate from ContentRegion to NavigationRegion to indicate that the page has changed. This influences the Navigation bar's
    /// buttons. Passes the journal so that the NavigationBar can update its view on whether or not to support navigation in this way.
    /// </summary>
    internal class NavigationEvent : PubSubEvent<IRegionNavigationJournal>
    {
    }
}
