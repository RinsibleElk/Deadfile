using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    /// <summary>
    /// Service to navigate the user to a URL in their default browser.
    /// </summary>
    public class UrlNavigationService : IUrlNavigationService
    {
        /// <summary>
        /// Perform the navigation.
        /// </summary>
        /// <param name="url"></param>
        public void Navigate(string url)
        {
            var uri = new Uri(url);
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }

        /// <summary>
        /// Send an e-mail.
        /// </summary>
        /// <param name="emailAddress"></param>
        public void SendEmail(string emailAddress)
        {
            var uri = new Uri("mailto:" + emailAddress);
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
    }
}
