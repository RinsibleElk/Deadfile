using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.JobChildren;
using Prism.Events;

namespace Deadfile.Tab.Events
{
    internal struct JobChildNavigateMessage
    {
        public JobChildNavigateMessage(JobChildExperience experience, int modelId, bool edit)
        {
            Experience = experience;
            ModelId = modelId;
            Edit = edit;
        }
        public JobChildExperience Experience { get; }
        public int ModelId { get; }
        public bool Edit { get; }

        public override string ToString()
        {
            return $"[Experience={Experience}, ModelId={ModelId}]";
        }
    }

    class JobChildNavigateEvent : PubSubEvent<JobChildNavigateMessage>
    {
    }
}
