using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Utility class to broadcast the DisplayName to the tab header.
    /// </summary>
    public static class DisplayNameBroadcaster
    {
        public static void BroadcastDisplayName(IEventAggregator eventAggregator, Experience experience)
        {
            eventAggregator.GetEvent<DisplayNameEvent>().Publish(Regex.Replace(experience.ToString(), @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1"));
        }
    }
}
