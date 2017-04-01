using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.JobChildren;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Jobs
{
    internal class JobsPageViewModel : EditableItemViewModel<ClientAndJobNavigationKey, JobModel, JobsPageState>, IJobsPageViewModel
    {
        private readonly TabIdentity _tabIdentity;
        private readonly INavigationService _navigationService;
        private readonly IDeadfileRepository _repository;
        private JobChildExperience _selectedJobChild = JobChildExperience.Empty;
        private ISimpleEditableItemViewModel _jobChildViewModel;
        private SubscriptionToken _jobChildNavigateEventSubscriptionToken;

        public static readonly List<JobChildExperience> AllJobChildExperiences = new List<JobChildExperience>(new[]
        {
            JobChildExperience.JobTasks,
            JobChildExperience.Applications,
            JobChildExperience.Expenses,
            JobChildExperience.BillableHours
        });

        public JobsPageViewModel(TabIdentity tabIdentity,
            INavigationService navigationService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDeadfileDialogCoordinator dialogCoordinator) : base(tabIdentity, eventAggregator, dialogCoordinator, new UndoTracker<JobModel>())
        {
            _tabIdentity = tabIdentity;
            _navigationService = navigationService;
            _repository = repository;
        }

        internal override bool CanEdit
        {
            get { return State.HasFlag(JobsPageState.CanEdit); }
            set
            {
                if (value)
                    State = State | JobsPageState.CanEdit;
                else
                    State = State & ~JobsPageState.CanEdit;
            }
        }

        internal override bool CanDelete
        {
            get { return State.HasFlag(JobsPageState.CanDelete); }
            set
            {
                if (value)
                    State = State | JobsPageState.CanDelete;
                else
                    State = State & ~JobsPageState.CanDelete;
            }
        }

        internal override bool CanSave
        {
            get { return State.HasFlag(JobsPageState.CanSave); }
            set
            {
                if (value)
                    State = State | JobsPageState.CanSave;
                else
                    State = State & ~JobsPageState.CanSave;
            }
        }

        internal override bool UnderEdit
        {
            get { return State.HasFlag(JobsPageState.UnderEdit); }
            set
            {
                if (value)
                    State = State | JobsPageState.UnderEdit;
                else
                    State = State & ~JobsPageState.UnderEdit;
            }
        }

        protected override JobModel GetModel(ClientAndJobNavigationKey clientAndJobNavigationKey)
        {
            JobModel jobModel;
            if (clientAndJobNavigationKey.Equals(default(ClientAndJobNavigationKey)) || clientAndJobNavigationKey.JobId == 0 || clientAndJobNavigationKey.JobId == ModelBase.NewModelId)
            {
                jobModel = new JobModel
                {
                    ClientId = clientAndJobNavigationKey.ClientId,
                    JobNumber = _repository.GetNextSuggestedJobNumber()
                };
                DisplayName = "New Job";
            }
            else
            {
                jobModel = _repository.GetJobById(clientAndJobNavigationKey.JobId);
                DisplayName = jobModel.JobId == ModelBase.NewModelId ? "New Job" : jobModel.AddressFirstLine;
            }
            Logger.Info("Event|DisplayNameEvent|Send|{0}|{1}", _tabIdentity.TabIndex, DisplayName);
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
            // These two are both used to prevent moving the selection around while the parent job is editable.
            _jobChildViewModel.ParentEditable = editable;
            ChildIsEditable = editable;
            State = editable ? State | JobsPageState.CanDiscard : State & ~JobsPageState.CanDiscard;
        }

        private async Task<bool> ActuallySave()
        {
            try
            {
                // If deleting or completing, but there are active incomplete job children, raise a dialog.
                var isBeingCompleted = SelectedItem.IsBeingCompleted();
                var isBeingDeleted = SelectedItem.IsBeingDeleted();
                var actuallySave = true;
                if (isBeingDeleted || isBeingCompleted)
                {
                    var activeExpense =
                    (from expense in _repository.GetExpensesForJob(SelectedItem.JobId, "")
                        where (expense.State == BillableState.Active || expense.State == BillableState.Billed)
                        select expense).FirstOrDefault();
                    var activeChildDetails =
                        activeExpense == null
                            ? null
                            : (activeExpense.State == BillableState.Active
                                ? "Unbilled expense: " + activeExpense.Description
                                : "Unpaid expense: " + activeExpense.Description);
                    if (activeChildDetails == null)
                    {
                        var activeBillableHour =
                        (from billableHour in _repository.GetBillableHoursForJob(SelectedItem.JobId, "")
                            where
                            (billableHour.State == BillableState.Active || billableHour.State == BillableState.Billed)
                            select billableHour).FirstOrDefault();
                        activeChildDetails =
                            activeBillableHour == null
                                ? null
                                : (activeBillableHour.State == BillableState.Active
                                    ? "Unbilled expense: " + activeBillableHour.Description
                                    : "Unpaid expense: " + activeBillableHour.Description);
                    }
                    if (activeChildDetails == null)
                    {
                        var activeJobTask =
                        (from jobTask in _repository.GetJobTasksForJob(SelectedItem.JobId, "")
                            where jobTask.State == JobTaskState.Active
                            select jobTask).FirstOrDefault();
                        activeChildDetails =
                            activeJobTask == null
                                ? null
                                : ("Active job task: " + activeJobTask.Description);
                    }
                    if (activeChildDetails == null)
                    {
                        var activeApplication =
                        (from application in _repository.GetApplicationsForJob(SelectedItem.JobId, "")
                            where application.State == ApplicationState.Current
                            select application).FirstOrDefault();
                        activeChildDetails =
                            activeApplication == null
                                ? null
                                : "Current application " + activeApplication.LocalAuthorityReference;
                    }
                    if (activeChildDetails != null)
                    {
                        var result =
                            await
                                DialogCoordinator.ConfirmDeleteAsync(this,
                                    (isBeingCompleted ? "Complete" : "Delete") + " job with active children?",
                                    "The job " + SelectedItem.AddressFirstLine + " has active children - for example " +
                                    activeChildDetails + ". Are you sure you want to proceed?");
                        actuallySave = result == MessageDialogResult.Affirmative;
                    }
                }

                // If deleting, give the user a courtesy are you sure?
                if (isBeingDeleted && actuallySave)
                {
                    var result =
                        await
                            DialogCoordinator.ConfirmDeleteAsync(this,
                                "Are you sure?",
                                "Do you want to delete " + SelectedItem.AddressFirstLine + "?");
                    actuallySave = result == MessageDialogResult.Affirmative;
                }

                // Finally, save the job and handle any related stuffs.
                if (!actuallySave) return false;
                // Now save the job.
                _repository.SaveJob(SelectedItem);

                // If the job is now in an inactive state, check if the client no longer has any active jobs and make it inactive.
                var firstJob = _repository.GetBrowserJobsForClient(BrowserMode.Client, false, SelectedItem.ClientId).FirstOrDefault();
                var clientModel = _repository.GetClientById(SelectedItem.ClientId);
                if (firstJob == null && clientModel.Status == ClientStatus.Active)
                {
                    clientModel.Status = ClientStatus.Inactive;
                    _repository.SaveClient(clientModel);
                }
                else if (firstJob != null && clientModel.Status == ClientStatus.Inactive)
                {
                    clientModel.Status = ClientStatus.Active;
                    _repository.SaveClient(clientModel);
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while saving {0}, {1}, {2}, {3}", _tabIdentity, SelectedItem, e, e.StackTrace);
                await DialogCoordinator.ShowMessageAsync(this, "Failed to save", "Failed to save due to exception: " + e.Message);
                return false;
            }
        }

        protected override async Task PerformSave(bool andPrint)
        {
            await ActuallySave();
        }

        protected override async Task<bool> PerformDelete()
        {
            try
            {
                SelectedItem.Status = JobStatus.Cancelled;
                var saved = await ActuallySave();
                if (!saved)
                    SelectedItem.ResetStatus();
                return saved;
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while deleting {0}, {1}, {2}, {3}", _tabIdentity, SelectedItem, e, e.StackTrace);
                throw;
            }
        }

        public override Experience Experience { get; } = Experience.Jobs;

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
                    _navigationService.RequestNavigate(this, nameof(JobChildViewModel), _selectedJobChild + JobChildKeys.JobChildKey, new ClientAndJobNavigationKey(SelectedItem.ClientId, SelectedItem.JobId));
            }
        }

        private bool _jobChildIsEditable;
        public bool JobChildIsEditable
        {
            get { return _jobChildIsEditable; }
            set
            {
                if (value == _jobChildIsEditable) return;
                _jobChildIsEditable = value;
                NotifyOfPropertyChange(() => JobChildIsEditable);

                ChildIsEditable = value;

                if (_jobChildIsEditable)
                {
                    CanEdit = false;
                    CanDelete = false;
                }
                else
                {
                    CanEdit = true;
                    CanDelete = true;
                }
            }
        }

        public bool ChildIsEditable
        {
            get { return _childIsEditable; }
            set
            {
                if (value == _childIsEditable) return;
                _childIsEditable = value;
                NotifyOfPropertyChange(() => ChildIsEditable);
            }
        }

        public override void ActivateUndoTracker<TObjectUnderEdit>(UndoTracker<TObjectUnderEdit> newActiveUndoTracker, TObjectUnderEdit objectUnderEdit)
        {
            base.ActivateUndoTracker(newActiveUndoTracker, objectUnderEdit);

            if (!object.ReferenceEquals(newActiveUndoTracker, UndoTracker))
                JobChildIsEditable = true;
        }

        public override void DeactivateUndoTracker()
        {
            base.DeactivateUndoTracker();

            JobChildIsEditable = false;
        }

        private int _clientId;
        private bool _childIsEditable;

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

            _jobChildNavigateEventSubscriptionToken =
                EventAggregator.GetEvent<JobChildNavigateEvent>().Subscribe(NavigateToJobChild);

            // Select JobTasks - this should take care of setting up the view model for the JobChildViewModel control.
            SelectedJobChild = JobChildExperience.JobTasks;
        }

        private void NavigateToJobChild(JobChildNavigateMessage navigateMessage)
        {
            SelectedJobChild = navigateMessage.Experience;
            JobChildViewModel.NavigateToModel(navigateMessage.ModelId);
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            EventAggregator.GetEvent<JobChildNavigateEvent>().Unsubscribe(_jobChildNavigateEventSubscriptionToken);

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
