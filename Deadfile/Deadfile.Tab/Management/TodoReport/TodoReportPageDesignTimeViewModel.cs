using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Reporting;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Management.TodoReport
{
    class TodoReportPageDesignTimeViewModel :
        ManagementPageDesignTimeViewModel<JobTaskModel>,
        ITodoReportPageViewModel
    {
        public TodoReportPageDesignTimeViewModel()
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
                jobTaskModel.ClientFullName = clients[i%2];
                jobTaskModel.Property = jobs[i%3];
                jobTaskModel.DueDate = new DateTime(2016, 10, 21).AddDays(i);
                jobTaskModel.State = (i%2 == 0) ? JobTaskState.Active : JobTaskState.Completed;
                li.Add(jobTaskModel);
            }
            Items = new ObservableCollection<JobTaskModel>(li);
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.TodoReport;

        public ICommand NavigateToClient { get; } = null;

        public ICommand NavigateToJob { get; } = null;
        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7.0);
        public bool IncludeInactive { get; set; } = false;
        public void Print()
        {
            throw new NotImplementedException();
        }
    }
}
