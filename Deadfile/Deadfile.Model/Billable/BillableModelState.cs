using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    public enum BillableModelState
    {
        /// <summary>
        /// Included in some other invoice.
        /// </summary>
        Claimed,

        /// <summary>
        /// Not included in any invoice.
        /// </summary>
        Excluded,

        /// <summary>
        /// Partially included in this invoice.
        /// </summary>
        PartiallyIncluded,

        /// <summary>
        /// Fully included in this invoice.
        /// </summary>
        FullyIncluded
    }
}
