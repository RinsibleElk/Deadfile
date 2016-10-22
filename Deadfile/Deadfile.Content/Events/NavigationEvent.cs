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
    /// buttons.
    /// </summary>
    public class NavigationEvent : PubSubEvent<IRegionNavigationJournal>
    {
    }
}
