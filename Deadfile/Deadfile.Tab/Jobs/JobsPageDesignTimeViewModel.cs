using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.DesignTime;
using Deadfile.Tab.JobChildren;

namespace Deadfile.Tab.Jobs
{
    class JobsPageDesignTimeViewModel : DesignTimeViewModel, IJobsPageViewModel
    {
        public JobModel SelectedItem { get; set; } = new JobModel()
        {
            AddressFirstLine = "1 Yemen Road",
            AddressPostCode = "EN4 5QP"
        };
        public bool Editable { get; } = false;
        public List<string> Errors { get; } = new List<string>();
        public Experience Experience { get; } = Experience.Jobs;
        public List<JobChildExperience> JobChildren { get; } = JobsPageViewModel.AllJobChildExperiences;
        public JobChildExperience SelectedJobChild { get; set; } = JobChildExperience.Applications;
    }
}
