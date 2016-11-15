using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// All the types of billable items, including the parent job.
    /// </summary>
    public enum BillableModelType
    {
        Job,
        Application,
        Expense,
        BillableHour
    }
}
