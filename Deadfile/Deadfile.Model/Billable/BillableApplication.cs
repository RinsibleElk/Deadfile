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
        private int _applicationId;
        public int ApplicationId
        {
            get { return _applicationId; }
            set { SetProperty(ref _applicationId, value); }
        }

        public override BillableModelType ModelType
        {
            get { return BillableModelType.Application; }
        }

        public override string Text { get { return _localAuthorityReference + " (" + NetAmount + ")"; } }

        private string _localAuthorityReference;
        public string LocalAuthorityReference
        {
            get { return _localAuthorityReference; }
            set
            {
                if (SetProperty(ref _localAuthorityReference, value))
                    OnPropertyChanged(nameof(Text));
            }
        }
    }
}
