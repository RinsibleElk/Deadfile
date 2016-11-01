using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.DesignTime
{
    class PageDesignTimeViewModel : DesignTimeViewModel, IPageViewModel, INavigationAware
    {
        public void OnNavigatedTo(object parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public virtual Experience Experience { get; }
        public bool ShowActionsPad { get; } = false;
    }
}
