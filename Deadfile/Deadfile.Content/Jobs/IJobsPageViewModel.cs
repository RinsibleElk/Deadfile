using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.JobChildren;
using Deadfile.Content.ViewModels;
using Deadfile.Model;

namespace Deadfile.Content.Jobs
{
    public interface IJobsPageViewModel : IDeadfileViewModel, IEditableItemViewModel<JobModel>
    {
        List<JobChildExperience> JobChildren { get; }
        JobChildExperience SelectedJobChild { get; set; }
    }
}
