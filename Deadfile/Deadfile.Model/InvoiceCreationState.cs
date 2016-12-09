using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Billable;

namespace Deadfile.Model
{
    /// <summary>
    /// During the set up of a new <see cref="InvoiceModel"/>, there is a linear state machine, that requires that the user define the
    /// <see cref="Company"/> first, then the collection of selected <see cref="BillableModel"/> items, then finally specify the details of the
    /// final invoice to be sent.
    /// </summary>
    public enum InvoiceCreationState
    {
        DefineBillables,
        DefineInvoice
    }
}
