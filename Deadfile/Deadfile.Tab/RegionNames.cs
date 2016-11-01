using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab
{
    /// <summary>
    /// Just names of areas on the screen. There is a convention here for how views and view models are named and how they are given
    /// to the IOC container to be injected in the right areas.
    /// </summary>
    public static class RegionNames
    {
        public const string Page = "Page";
        public const string ActionsPad = "ActionsPad";
        public const string BrowserPane = "BrowserPane";
        public const string NavigationBar = "NavigationBar";
        public const string QuotesBar = "QuotesBar";
    }
}
