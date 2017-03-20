using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;
using Prism.Events;

namespace Deadfile.Tab.Jobs
{
    class JobsActionsPadViewModel : ActionsPadViewModel<JobsPageState>, IJobsActionsPadViewModel
    {
        public JobsActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator) : base(tabIdentity, eventAggregator)
        {
        }
        public override bool CanSaveItem => PageState.HasFlag(JobsPageState.CanSave);
        public override bool SaveItemIsVisible => PageState.HasFlag(JobsPageState.UnderEdit);
        public override bool CanDeleteItem => PageState.HasFlag(JobsPageState.CanDelete);
        public override bool DeleteItemIsVisible => !PageState.HasFlag(JobsPageState.UnderEdit);
        public override bool CanEditItem => PageState.HasFlag(JobsPageState.CanEdit);
        public override bool EditItemIsVisible => !PageState.HasFlag(JobsPageState.UnderEdit);
        public override bool CanDiscardItem => PageState.HasFlag(JobsPageState.CanDiscard);
        public override bool DiscardItemIsVisible => PageState.HasFlag(JobsPageState.UnderEdit);
        protected override void PageStateChanged(JobsPageState state)
        {
        }
    }
}
