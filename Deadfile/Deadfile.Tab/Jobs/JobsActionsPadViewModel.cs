using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;
using Prism.Events;

namespace Deadfile.Tab.Jobs
{
    class JobsActionsPadViewModel : ActionsPadViewModel, IJobsActionsPadViewModel
    {
        public JobsActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator) : base(tabIdentity, eventAggregator)
        {
        }
    }
}
