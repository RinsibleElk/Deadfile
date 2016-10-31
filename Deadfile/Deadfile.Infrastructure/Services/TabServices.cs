using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Prism.Events;

namespace Deadfile.Infrastructure.Services
{
    public sealed class TabServices : ITabServices
    {
        private static int Count = 0;

        private int _identity;

        public TabServices(IEventAggregator eventAggregator)
        {
            _identity = Count++;
        }

        public string Identity
        {
            get { return _identity.ToString(); }
        }
    }
}
