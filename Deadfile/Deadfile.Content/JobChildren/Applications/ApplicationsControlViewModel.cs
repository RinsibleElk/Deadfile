using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Events;
using Deadfile.Content.ViewModels;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;

namespace Deadfile.Content.JobChildren.Applications
{
    class ApplicationsControlViewModel : ViewModelBase, IApplicationsControlViewModel
    {
        private readonly IDeadfileRepository _repository;
        public ApplicationsControlViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            eventAggregator.GetEvent<CurrentJobEvent>().Subscribe(CurrentJobAction);
            _repository = repository;
            ApplicationTypes =
                new List<ApplicationType>(new ApplicationType[]
                    {ApplicationType.BuildingControl, ApplicationType.Planning});
        }

        public List<ApplicationType> ApplicationTypes { get; }
        public ApplicationModel SelectedModel { get; set; } = new ApplicationModel();

        private void CurrentJobAction(int jobId)
        {
            Models = new ObservableCollection<ApplicationModel>(_repository.GetApplicationsForJob(jobId));
        }

        private ObservableCollection<ApplicationModel> _models = new ObservableCollection<ApplicationModel>();
        public ObservableCollection<ApplicationModel> Models
        {
            get { return _models; }
            set { SetProperty(ref _models, value); }
        }
    }
}
