﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.JobChildren.BillableHours
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the billable hours for that job.
    /// </summary>
    class BillableHoursJobChildViewModel : SimpleEditableItemViewModel<BillableHourModel>, IBillableHoursJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public BillableHoursJobChildViewModel(IDeadfileRepository repository)
        {
            _repository = repository;
        }

        protected override void PerformSave()
        {
            _repository.SaveBillableHour(SelectedItem);
        }

        public override IEnumerable<BillableHourModel> GetModelsForJobId(int jobId, string filter)
        {
            return _repository.GetBillableHoursForJob(jobId, filter);
        }
    }
}