using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.JobChildren.JobTasks
{
    class JobTasksJobChildDesignTimeViewModel : ManagementPageDesignTimeViewModel<JobTaskModel>, IJobTasksJobChildViewModel
    {
        public JobTasksJobChildDesignTimeViewModel()
        {
            SelectedItem = null;
            var li = new List<JobTaskModel>();
            var clients = new string[2]
            {
                "Mr Joseph Stalin",
                "Mr Adolf Hitler"
            };
            var jobs = new string[3]
            {
                "1 Yemen Road",
                "2A Egypt Lane",
                "3 Sullivan Street"
            };
            for (int i = 0; i < 5; i++)
            {
                var jobTaskModel = new JobTaskModel();
                jobTaskModel.ClientId = i % 2;
                jobTaskModel.JobId = i % 3;
                jobTaskModel.JobTaskId = i + 1;
                jobTaskModel.ClientFullName = clients[i % 2];
                jobTaskModel.Property = jobs[i % 3];
                jobTaskModel.DueDate = new DateTime(2016, 10, 21).AddDays(i);
                jobTaskModel.State = (i % 2 == 0) ? JobTaskState.Active : JobTaskState.Completed;
                li.Add(jobTaskModel);
            }
            Items = new ObservableCollection<JobTaskModel>(li);
        }

        public ICommand TogglePriorityCommand { get; } = null;
        public ICommand BillCommand { get; } = null;
    }
}
