using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Prism.Events;

namespace Deadfile.Tab.JobChildren.Expenses
{
    /// <summary>
    /// A sub-control viewable in the Jobs page, that displays information about the expenses for that job.
    /// </summary>
    class ExpensesJobChildViewModel : SimpleEditableItemViewModel<ExpenseModel>, IExpensesJobChildViewModel
    {
        private readonly IDeadfileRepository _repository;

        public ExpensesJobChildViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        protected override void PerformSave()
        {
            _repository.SaveExpense(SelectedItem);
        }

        public override IEnumerable<ExpenseModel> GetModelsForJobId(int jobId, string filter)
        {
            return _repository.GetExpensesForJob(jobId, filter);
        }
    }
}
