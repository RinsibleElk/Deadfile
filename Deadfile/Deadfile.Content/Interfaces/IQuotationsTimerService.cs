﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Content.Interfaces
{
    public interface IQuotationsTimerService
    {
        void StartTimer(EventHandler callback);
    }
}
