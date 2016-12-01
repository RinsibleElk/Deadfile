﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    public enum RefreshBrowserMessage
    {
        Refresh
    }

    public class RefreshBrowserEvent : PubSubEvent<RefreshBrowserMessage>
    {
    }
}
