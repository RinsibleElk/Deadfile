using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    interface IReportPageViewModel<T> : IPageViewModel,
        IManagementViewModel<T> where T : ModelBase
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        bool IncludeInactive { get; set; }
        void Print();
    }
}
