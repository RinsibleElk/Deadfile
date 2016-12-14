using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = null;
            Items = new ObservableCollection<JobTaskModel>();
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.TodoReport;

        public ICommand NavigateToJob
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7.0);
    }
}
