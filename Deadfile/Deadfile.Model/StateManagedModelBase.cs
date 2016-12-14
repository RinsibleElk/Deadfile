using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    /// <summary>
    /// Represents models that cannot be deleted permanently - they are managed by their state.
    /// </summary>
    public abstract class StateManagedModelBase : ModelBase
    {
        /// <summary>
        /// Whether the state should be considered active.
        /// </summary>
        public abstract bool StateIsActive { get; }
    }
}
