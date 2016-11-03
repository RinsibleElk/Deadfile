using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.JobChildren.Applications
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the planning applications for that job.
    /// </summary>
    class ApplicationsJobChildViewModel : SimpleEditableItemViewModel<ApplicationModel>, IApplicationsJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public ApplicationsJobChildViewModel(IDeadfileRepository repository)
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
    }
}
