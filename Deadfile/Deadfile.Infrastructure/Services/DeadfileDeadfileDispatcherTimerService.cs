using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    public class DeadfileDeadfileDispatcherTimerService : IDeadfileDispatcherTimerService
    {
        public IDeadfileDispatcherTimer CreateTimer(TimeSpan period, Action actionToTake)
        {
            return new DeadfileDeadfileDispatcherTimer(period, actionToTake);
        }
    }
}
