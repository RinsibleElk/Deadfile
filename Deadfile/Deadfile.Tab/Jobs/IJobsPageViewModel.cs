using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;
using Deadfile.Tab.JobChildren;

namespace Deadfile.Tab.Jobs
{
    interface IJobsPageViewModel : IEditableItemViewModel<JobModel>, IPageViewModel
    {
        List<JobChildExperience> JobChildren { get; }
        JobChildExperience SelectedJobChild { get; set; }
    }
}
