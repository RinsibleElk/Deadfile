using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public class BillableHourModel : ModelBase
    {
        // No need to report changes or validate.
        public override int Id
        {
            get { return BillableHourId; }
            set { BillableHourId = value; }
        }

        private int _billableHourId = ModelBase.NewModelId;
        public int BillableHourId
        {
            get { return _billableHourId; }
            set { _billableHourId = value; }
        }
    }
}
