﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    public interface IDeadfileProgressController
    {
        Task CloseAsync();
    }
}
