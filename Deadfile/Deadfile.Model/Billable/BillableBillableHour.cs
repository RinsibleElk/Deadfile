using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// Leaf billable item representing a billable hour for a particular job.
    /// </summary>
    public class BillableBillableHour : BillableModel
    {

        private int _billableHourId;
        public int BillableHourId
        {
            get { return _billableHourId; }
            set { SetProperty(ref _billableHourId, value); }
        }

        public override BillableModelType ModelType => BillableModelType.BillableHour;

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (SetProperty(ref _description, value))
                    RaisePropertyChanged(nameof(Text));
            }
        }

        public override string Text => $"{Description} ({Hours} hours)";

        public override int Id
        {
            get { return BillableHourId; }
            set { BillableHourId = value; }
        }
    }
}
