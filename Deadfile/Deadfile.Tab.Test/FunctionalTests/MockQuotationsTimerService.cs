using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockQuotationsTimerService : IQuotationsTimerService
    {
        private EventHandler _callback = null;
        public void StartTimer(EventHandler callback)
        {
            _callback = callback;
        }

        public void FireCallback()
        {
            _callback.Invoke(this, EventArgs.Empty);
        }
    }
}
