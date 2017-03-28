using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.JobChildren.BillableHours;
using Deadfile.Tab.Jobs;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;

namespace Deadfile.Tab.JobChildren.JobTasks
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the job tasks for that job.
    /// </summary>
    class JobTasksJobChildViewModel : SimpleEditableItemViewModel<JobTaskModel>, IJobTasksJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand _togglePriorityCommand;
        private readonly DelegateCommand _makeJobTaskBillableCommand;

        public JobTasksJobChildViewModel(IDeadfileDispatcherTimerService timerService,
            IDeadfileDialogCoordinator dialogCoordinator,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(timerService, dialogCoordinator, eventAggregator)
        {
            _repository = repository;
            _togglePriorityCommand = new DelegateCommand(TogglePriority);
            _makeJobTaskBillableCommand = new DelegateCommand(MakeJobTaskBillable);
        }

        private async void MakeJobTaskBillable()
        {
            var result = await DialogCoordinator.ConfirmDeleteAsync(this, $"Make \"{SelectedItem.Description}\" billable?", "Are you sure? This action is permanent.");
            // Open a dialog to ask the user if they are sure.
            if (result == MessageDialogResult.Affirmative)
            {
                var billableHourId = PerformMakeJobTaskBillable();

                // Refresh the observable collection.
                Populate();

                // Navigate to the new billable hour.
                EventAggregator.GetEvent<JobChildNavigateEvent>().Publish(new JobChildNavigateMessage(JobChildExperience.BillableHours, billableHourId));
            }
        }

        private int PerformMakeJobTaskBillable()
        {
            return _repository.MakeJobTaskBillable(SelectedItem);
        }

        protected override void PerformDelete()
        {
            _repository.DeleteJobTask(SelectedItem);
        }

        protected override void PerformSave()
        {
            _repository.SaveJobTask(SelectedItem);
        }

        public override IEnumerable<JobTaskModel> GetModelsForJobId(int jobId, string filter)
        {
            return _repository.GetJobTasksForJob(jobId, filter);
        }

        public ICommand TogglePriorityCommand => _togglePriorityCommand;
        public ICommand MakeJobTaskBillableCommand { get { return _makeJobTaskBillableCommand; } }

        private void TogglePriority()
        {
            switch (SelectedItem.Priority)
            {
                case JobTaskPriority.Low:
                    SelectedItem.Priority = JobTaskPriority.Medium;
                    break;
                case JobTaskPriority.Medium:
                    SelectedItem.Priority = JobTaskPriority.High;
                    break;
                case JobTaskPriority.High:
                    SelectedItem.Priority = JobTaskPriority.Low;
                    break;
            }
        }
    }
}
