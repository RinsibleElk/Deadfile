using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Service to navigate the user to their default browser.
    /// </summary>
    public interface IUrlNavigationService
    {
        /// <summary>
        /// Perform the navigation.
        /// </summary>
        /// <param name="url"></param>
        void Navigate(string url);
    }
}
