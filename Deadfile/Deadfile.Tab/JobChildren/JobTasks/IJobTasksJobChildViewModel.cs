using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.JobChildren.JobTasks
{
    interface IJobTasksJobChildViewModel : ISimpleEditableItemViewModel<JobTaskModel>
    {
        ICommand TogglePriorityCommand { get; }
        ICommand MakeJobTaskBillableCommand { get; }
    }
}
