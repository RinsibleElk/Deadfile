using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.JobChildren.Applications
{
    /// <summary>
    /// Simple management of Planning Applications.
    /// </summary>
    interface IApplicationsJobChildViewModel : ISimpleEditableItemViewModel<ApplicationModel>
    {
        ObservableCollection<string> LocalAuthorities { get; set; }
    }
}
