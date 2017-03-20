using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockDeadfileDispatcherTimer : IDeadfileDispatcherTimer
    {
        private readonly TimeSpan _period;
        private readonly Action _actionToTake;
        private bool _isStarted = false;

        public MockDeadfileDispatcherTimer(TimeSpan period, Action actionToTake)
        {
            _period = period;
            _actionToTake = actionToTake;
        }

        public bool IsStarted => _isStarted;

        public void Start()
        {
            _isStarted = true;
        }

        public void Stop()
        {
            _isStarted = false;
        }
    }

    class MockDeadfileDispatcherTimerService : IDeadfileDispatcherTimerService
    {
        public readonly List<MockDeadfileDispatcherTimer> Timers = new List<MockDeadfileDispatcherTimer>();
        public IDeadfileDispatcherTimer CreateTimer(TimeSpan period, Action actionToTake)
        {
            var timer = new MockDeadfileDispatcherTimer(period, actionToTake);
            Timers.Add(timer);
            return timer;
        }
    }
}
