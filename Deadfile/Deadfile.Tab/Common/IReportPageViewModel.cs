using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    interface IReportPageViewModel<T> : IPageViewModel,
        IVisualPresenter,
        IManagementViewModel<T> where T : ModelBase
    {
        CompanyForFilter CompanyFilter { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        bool IncludeInactive { get; set; }
        void Print();
    }
}
