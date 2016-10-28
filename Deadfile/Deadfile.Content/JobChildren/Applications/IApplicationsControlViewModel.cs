using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;

namespace Deadfile.Content.JobChildren.Applications
{
    interface IApplicationsControlViewModel
    {
        List<ApplicationType> ApplicationTypes { get; }
        ApplicationModel SelectedModel { get; set; }
        ObservableCollection<ApplicationModel> Models { get; set; }
    }
}
