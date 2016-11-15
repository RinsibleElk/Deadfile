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
            set { SetProperty(ref _fullAddress, value); }
        }
    }
}
