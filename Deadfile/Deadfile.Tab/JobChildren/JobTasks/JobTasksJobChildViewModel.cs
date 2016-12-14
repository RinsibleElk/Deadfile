using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace Deadfile.Tab.JobChildren.JobTasks
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the job tasks for that job.
    /// </summary>
    class JobTasksJobChildViewModel : SimpleEditableItemViewModel<JobTaskModel>, IJobTasksJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public JobTasksJobChildViewModel(IDeadfileDispatcherTimerService timerService,
            IDialogCoordinator dialogCoordinator,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(timerService, dialogCoordinator, eventAggregator)
        {
            _repository = repository;
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
    }
}
