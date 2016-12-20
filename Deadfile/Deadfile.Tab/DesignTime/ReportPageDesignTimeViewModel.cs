using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.DesignTime
{
    class ReportPageDesignTimeViewModel<T> : ManagementPageDesignTimeViewModel<T>, IReportPageViewModel<T> where T : ModelBase
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);
        public bool IncludeInactive { get; set; } = false;
        public void Print()
        {
            throw new NotImplementedException();
        }
    }
}
