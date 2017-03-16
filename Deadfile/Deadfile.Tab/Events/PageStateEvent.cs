using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    class PageStateEvent<TPageState> : PubSubEvent<TPageState> where TPageState : struct
    {
    }
}
