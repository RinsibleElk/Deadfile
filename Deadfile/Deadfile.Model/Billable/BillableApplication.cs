using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// Leaf billable item representing a planning application for a particular job.
    /// </summary>
    public class BillableApplication : BillableModel
    {
        private string _localAuthorityReference;

        public override BillableModelType ModelType
        {
            get { return BillableModelType.Application; }
        }

        public string LocalAuthorityReference
        {
            get { return _localAuthorityReference; }
            set { SetProperty(ref _localAuthorityReference, value); }
        }
    }
}
