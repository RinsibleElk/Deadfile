using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.JobChildren;
using MahApps.Metro.Controls.Dialogs;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Jobs
{
    class JobsPageViewModel : EditableItemViewModel<ClientAndJobNavigationKey, JobModel>, IJobsPageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDeadfileRepository _repository;
        private JobChildExperience _selectedJobChild = JobChildExperience.Empty;
        private ISimpleEditableItemViewModel _jobChildViewModel;
        private int _clientId;

        public static readonly List<JobChildExperience> AllJobChildExperiences = new List<JobChildExperience>(new[]
        {
            JobChildExperience.Applications,
            JobChildExperience.Expenses,
            JobChildExperience.BillableHours
        });

        public JobsPageViewModel(INavigationService navigationService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator) : base(eventAggregator, dialogCoordinator, new UndoTracker<JobModel>())
        {
            _navigationService = navigationService;
            _repository = repository;
        }

        protected override JobModel GetModel(ClientAndJobNavigationKey clientAndJobNavigationKey)
        {
            JobModel jobModel;
            if (clientAndJobNavigationKey.Equals(default(ClientAndJobNavigationKey)) || clientAndJobNavigationKey.JobId == 0 || clientAndJobNavigationKey.JobId == ModelBase.NewModelId)
            {
                jobModel = new JobModel();
                DisplayName = "New Job";
            }
            else
            {
                jobModel = _repository.GetJobById(clientAndJobNavigationKey.JobId);
                if (jobModel.JobId == ModelBase.NewModelId)
                    DisplayName = "New Job";
                else
                    DisplayName = jobModel.AddressFirstLine;
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return jobModel;
        }

        protected override bool ShouldEditOnNavigate(ClientAndJobNavigationKey clientAndJobNavigationKey)
        {
            return clientAndJobNavigationKey.JobId == ModelBase.NewModelId;
        }

        protected override ClientAndJobNavigationKey GetLookupParameters()
        {
            return new ClientAndJobNavigationKey(SelectedItem.ClientId, SelectedItem.JobId);
        }

        public override void EditingStatusChanged(bool editable)
        {
        }

        public override void PerformSave()
        {
            try
            {
                _repository.SaveJob(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public override void PerformDelete()
        {
            try
            {
                _repository.DeleteJob(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public Experience Experience { get; } = Experience.Jobs;
        public bool ShowActionsPad { get; } = true;

        public List<JobChildExperience> JobChildren { get; } = AllJobChildExperiences;

        public JobChildExperience SelectedJobChild
        {
            get { return _selectedJobChild; }
            set
            {
                // Set the value.
                if (value == _selectedJobChild) return;
                _selectedJobChild = value;
                NotifyOfPropertyChange(() => SelectedJobChild);

                // Navigate the control to the view.
                if (_selectedJobChild == JobChildExperience.Empty)
                    _navigationService.RequestDeactivate(this, nameof(JobChildViewModel));
                else
                    _navigationService.RequestNavigate(this, nameof(JobChildViewModel), _selectedJobChild + JobChildKeys.JobChildKey, SelectedItem.JobId);
            }
        }

        public int ClientId
        {
            get { return _clientId; }
            set
            {
                if (value == _clientId) return;
                _clientId = value;
                NotifyOfPropertyChange(() => ClientId);
            }
        }

        public override void OnNavigatedTo(ClientAndJobNavigationKey clientAndJobNavigationKey)
        {
            ClientId = clientAndJobNavigationKey.ClientId;

            base.OnNavigatedTo(clientAndJobNavigationKey);

            // Select Applications - this should take care of setting up the view model for the JobChildViewModel control.
            SelectedJobChild = JobChildExperience.Applications;
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            // Free up resources and disconnect the child.
            SelectedJobChild = JobChildExperience.Empty;
        }

        /// <summary>
        /// The job child currently being displayed.
        /// </summary>
        public ISimpleEditableItemViewModel JobChildViewModel
        {
            get { return _jobChildViewModel; }
            set
            {
                if (_jobChildViewModel == value) return;
                _jobChildViewModel = value;
                NotifyOfPropertyChange(() => JobChildViewModel);

                // Tell the child about this entity, in order to allow them to activate undo/redo tracking.
                _jobChildViewModel?.RegisterUndoTrackerActivatable(this);
            }
        }
    }
}
