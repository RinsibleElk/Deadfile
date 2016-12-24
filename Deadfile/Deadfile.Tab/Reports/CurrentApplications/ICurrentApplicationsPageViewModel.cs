using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Reports.CurrentApplications
{
    interface ICurrentApplicationsPageViewModel : IReportPageViewModel<CurrentApplicationModel>
    {
        ICommand NavigateToJob { get; }
    }
}
