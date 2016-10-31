using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    /// <summary>
    /// Hide the specific use of <see cref="DispatcherTimer"/> to make things testable.
    /// </summary>
    public sealed class QuotationsTimerService : IQuotationsTimerService
    {
        public void StartTimer(EventHandler callback)
        {
            var timer =
                new DispatcherTimer(
                    TimeSpan.FromSeconds(30),
                    DispatcherPriority.ApplicationIdle,
                    callback,
                    Dispatcher.CurrentDispatcher);
            timer.Start();
        }
    }
}
