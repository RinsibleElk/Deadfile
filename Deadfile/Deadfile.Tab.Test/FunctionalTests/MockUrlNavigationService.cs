using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockUrlNavigationService : IUrlNavigationService
    {
        public void Navigate(string url)
        {
        }

        public void SendEmail(string emailAddress)
        {
        }
    }
}
