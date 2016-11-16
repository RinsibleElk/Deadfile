using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.JobChildren.Applications
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the planning applications for that job.
    /// </summary>
    class ApplicationsJobChildViewModel : SimpleEditableItemViewModel<ApplicationModel>, IApplicationsJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public ApplicationsJobChildViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        protected override void PerformSave()
        {
            _repository.SaveApplication(SelectedItem);
        }

        public override IEnumerable<ApplicationModel> GetModelsForJobId(int jobId, string filter)
        {
            return _repository.GetApplicationsForJob(jobId, filter);
        }

        public override void OnNavigatedTo(int jobId)
        {
            base.OnNavigatedTo(jobId);
            LocalAuthorities = new ObservableCollection<string>(_repository.GetLocalAuthorities(null).Select((la) => la.Name));
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
            LocalAuthorities = new ObservableCollection<string>();
        }

        private ObservableCollection<string> _localAuthorities;
        public ObservableCollection<string> LocalAuthorities
        {
            get { return _localAuthorities; }
            set
            {
                if (Equals(value, _localAuthorities)) return;
                _localAuthorities = value;
                NotifyOfPropertyChange(() => LocalAuthorities);
            }
        }
    }
}
