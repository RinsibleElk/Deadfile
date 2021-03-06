﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// A state tracking the status of an invoice that has been issued.
    /// </summary>
    public enum InvoiceStatus
    {
        Created,
        Cancelled,
        Paid
    }
}
