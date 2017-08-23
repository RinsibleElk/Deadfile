using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Reports.UnbilledJobs
{
    interface IUnbilledJobsPageViewModel : IReportPageViewModel<UnbilledJobModel>, IDataGridPresenter
    {
        ICommand NavigateToClient { get; }
        ICommand NavigateToJob { get; }
        void ExportToExcel();
    }
}
