using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Content.JobChildren;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.Jobs
{
    class JobsPageDesignTimeViewModel : IJobsPageViewModel
    {
        public JobsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            var clientsList = new List<JobModel>();
            SelectedItem = new JobModel();
            Title = "Jobs";
        }

        public JobModel SelectedItem { get; set; }

        public Experience Experience { get { return Experience.Jobs; } }

        public string Title { get; set; }

        public bool Editable { get; } = false;
        public List<string> Errors { get; } = new List<string>();

        public List<JobChildExperience> JobChildren { get; } = new List<JobChildExperience>(new[]
        {
            JobChildExperience.Applications,
            JobChildExperience.Expenses,
            JobChildExperience.Payments
        });
        public JobChildExperience SelectedJobChild { get; set; } = JobChildExperience.Applications;
    }
}
