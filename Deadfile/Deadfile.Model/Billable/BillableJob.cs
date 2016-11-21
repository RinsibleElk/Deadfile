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
                var hasExcluded = false;
                var hasIncluded = false;
                foreach (var child in Children)
                {
                    hasExcluded |= child.State == BillableModelState.Excluded;
                    hasIncluded |= child.State == BillableModelState.FullyIncluded;
                    if (hasExcluded && hasIncluded) break;
                }
                if (hasExcluded && hasIncluded)
                    State = BillableModelState.PartiallyIncluded;
                else if (hasExcluded)
                    State = BillableModelState.Excluded;
                else if (hasIncluded)
                    State = BillableModelState.FullyIncluded;
                else
                    State = BillableModelState.Claimed;
                AutomaticEditingInProgress = false;
            }
        }

        public void NetAmountChanged(int index)
        {
        }
    }
}
