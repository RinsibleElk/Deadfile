using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Prism.Events;

namespace Deadfile.Tab.JobChildren.JobTasks
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the job tasks for that job.
    /// </summary>
    class JobTasksJobChildViewModel : SimpleEditableItemViewModel<JobTaskModel>, IJobTasksJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public JobTasksJobChildViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
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
