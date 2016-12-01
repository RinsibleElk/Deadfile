using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    public class BillableJob : BillableModel, IBillableModelContainer
    {
        public override BillableModelType ModelType
        {
            get { return BillableModelType.Job; }
        }

        private int _jobId;
        public int JobId
        {
            get { return _jobId; }
            set { SetProperty(ref _jobId, value); }
        }

        private string _fullAddress;
        public string FullAddress
        {
            get { return _fullAddress; }
            set
            {
                if (SetProperty(ref _fullAddress, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        private double _totalPossibleNetAmount;
        public double TotalPossibleNetAmount
        {
            get { return _totalPossibleNetAmount; }
            set
            {
                if (SetProperty(ref _totalPossibleNetAmount, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        public override string Text { get { return FullAddress + " (" + NetAmount + "/" + TotalPossibleNetAmount + ")"; } }

        public override int Id
        {
            get { return JobId; }
            set { JobId = value; }
        }

        public bool AutomaticEditingInProgress { get; set; } = false;
        public void StateChanged(int index)
        {
            NetAmount = 0;
            foreach (var child in Children)
            {
                if (child.State == BillableModelState.FullyIncluded)
                {
                    NetAmount += child.NetAmount;
                    Parent?.NetAmountChanged(Index);
                }
            }
            if (!AutomaticEditingInProgress)
            {
                AutomaticEditingInProgress = true;
                var hasClaimed = false;
                var hasExcluded = false;
                var hasIncluded = false;
                foreach (var child in Children)
                {
                    hasClaimed |= child.State == BillableModelState.Claimed;
                    hasExcluded |= child.State == BillableModelState.Excluded;
                    hasIncluded |= child.State == BillableModelState.FullyIncluded;
                    if (hasClaimed && hasExcluded && hasIncluded) break;
                }
                // Calculate the job state based on the billable items that are included.
                if (hasIncluded)
                {
                    // Don't care about claimed if there are any excluded or included.
                    State = hasExcluded
                        ? BillableModelState.PartiallyIncluded
                        : BillableModelState.FullyIncluded;
                }
                else if (hasClaimed)
                {
                    State = hasExcluded ? BillableModelState.Excluded : BillableModelState.Claimed;
                }
                else
                {
                    // Includes the case where there are not any billables at all.
                    State = BillableModelState.Excluded;
                }
                AutomaticEditingInProgress = false;
            }
        }

        public void NetAmountChanged(int index)
        {
        }
    }
}
