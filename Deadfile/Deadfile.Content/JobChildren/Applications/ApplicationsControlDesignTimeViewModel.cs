using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.JobChildren.Applications
{
    class ApplicationsControlDesignTimeViewModel : IApplicationsControlViewModel
    {
        public ApplicationsControlDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            var models = new List<ApplicationModel>(repository.GetApplicationsForJob(0));
            Models = new ObservableCollection<ApplicationModel>(models);
            SelectedModel = models[0];
            ApplicationTypes =
                new List<ApplicationType>(new ApplicationType[]
                    {ApplicationType.BuildingControl, ApplicationType.Planning});
        }

        public List<ApplicationType> ApplicationTypes { get; }
        public ApplicationModel SelectedModel { get; set; }
        public ObservableCollection<ApplicationModel> Models { get; set; }
    }
}
