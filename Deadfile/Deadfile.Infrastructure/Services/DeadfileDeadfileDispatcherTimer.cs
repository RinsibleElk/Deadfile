using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    public class DeadfileDeadfileDispatcherTimer : IDeadfileDispatcherTimer
    {
        private readonly Action _actionToTake;
        private readonly DispatcherTimer _timer;
        private bool _isRunning = false;
        public DeadfileDeadfileDispatcherTimer(TimeSpan period, Action actionToTake)
        {
            _actionToTake = actionToTake;
            _timer = new DispatcherTimer(period, DispatcherPriority.ApplicationIdle, Callback, Dispatcher.CurrentDispatcher);
        }

        private void Callback(object sender, EventArgs eventArgs)
        {
            _actionToTake();
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _timer.Start();
                _isRunning = true;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _timer.Stop();
                _isRunning = false;
            }
        }
    }
}
