using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab
{
    /// <summary>
    /// Tab identity, passed around and used in logging.
    /// </summary>
    public class TabIdentity
    {
        private static int _tabIndex = 0;

        /// <summary>
        /// Created when a new tab is created.
        /// </summary>
        public TabIdentity() : this(_tabIndex++)
        {
        }

        internal TabIdentity(int tabIndex)
        {
            TabIndex = tabIndex;
        }

        /// <summary>
        /// The identity of this tab.
        /// </summary>
        public int TabIndex { get; }
    }
}
