using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    public class BillableJob : BillableModel
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
    }
}
