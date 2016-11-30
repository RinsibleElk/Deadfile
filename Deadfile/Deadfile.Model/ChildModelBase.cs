using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public abstract class ChildModelBase : ModelBase
    {
        public int Context { get; set; }

        public abstract bool DeletePending { get; set; }
    }
}
