using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.JobChildren;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Jobs
{
    class JobsPageViewModel : EditableItemViewModel<int, JobModel>, IJobsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        private JobChildExperience _selectedJobChild;

        public static readonly List<JobChildExperience> AllJobChildExperiences = new List<JobChildExperience>(new[]
        {
            JobChildExperience.Applications,
            JobChildExperience.Expenses,
            JobChildExperience.Payments
        });

        public JobsPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        public override JobModel GetModel(int id)
        {
            JobModel jobModel;
            if (id == 0 || id == ModelBase.NewModelId)
            {
                jobModel = new JobModel();
                DisplayName = "Jobs";
            }
            else
            {
                jobModel = _repository.GetJobById(id);
                if (jobModel.JobId == ModelBase.NewModelId)
                    DisplayName = "Jobs";
                else
                    DisplayName = jobModel.AddressFirstLine;
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return jobModel;
        }

        public override void EditingStatusChanged(bool editable)
        {
        }

        public override void PerformSave()
        {
            throw new NotImplementedException();
        }

        public Experience Experience { get; } = Experience.Jobs;
        public bool ShowActionsPad { get; } = true;

        public List<JobChildExperience> JobChildren { get; } = AllJobChildExperiences;

        public JobChildExperience SelectedJobChild
        {
            get { return _selectedJobChild; }
            set
            {
                if (value == _selectedJobChild) return;
                _selectedJobChild = value;
                NotifyOfPropertyChange(() => SelectedJobChild);
            }
        }
    }
}
