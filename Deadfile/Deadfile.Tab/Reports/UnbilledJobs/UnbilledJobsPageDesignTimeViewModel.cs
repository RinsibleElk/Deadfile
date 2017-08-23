using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Reporting;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Reports.UnbilledJobs
{
    class UnbilledJobsPageDesignTimeViewModel :
        ReportPageDesignTimeViewModel<UnbilledJobModel>,
        IUnbilledJobsPageViewModel
    {
        public UnbilledJobsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = null;
            Items = new ObservableCollection<UnbilledJobModel>();
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.UnbilledJobs;
        public ICommand NavigateToClient { get; } = null;
        public ICommand NavigateToJob { get; } = null;
        public void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        public DataGrid DataGrid { get; } = null;
    }
}
