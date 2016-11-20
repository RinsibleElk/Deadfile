using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    public interface IBillableModelContainer
    {
        bool AutomaticEditingInProgress { get; set; }
        void StateChanged(int index);
    }
}
