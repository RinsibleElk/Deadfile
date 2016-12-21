using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Reports.TodoReport
{
    interface ITodoReportPageViewModel : IReportPageViewModel<JobTaskModel>
    {
        ICommand NavigateToClient { get; }
        ICommand NavigateToJob { get; }
    }
}
