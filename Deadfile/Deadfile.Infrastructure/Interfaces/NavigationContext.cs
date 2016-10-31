using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    internal class NavigationContext
    {
        public NavigationContext(object host, string hostKey, object target, object parameters)
        {
            Host = host;
            HostKey = hostKey;
            TargetObject = target;
            Parameters = parameters;
        }
        public object Host { get; set; }
        public string HostKey { get; set; }
        public object TargetObject { get; }
        public object Parameters { get; }
    }
}
