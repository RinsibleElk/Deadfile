using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Common;
using Deadfile.Tab.DesignTime;
using Deadfile.Tab.Events;
using Deadfile.Tab.Home;
using Deadfile.Tab.Quotes;
using Deadfile.Tab.Navigation;

namespace Deadfile.Tab.Tab
{
    class TabDesignTimeViewModel : DesignTimeViewModel, ITabViewModel
    {
        public object NavigationBar { get; set; } = new NavigationBarDesignTimeViewModel();
        public IPageViewModel ContentArea { get; set; } = new HomePageDesignTimeViewModel();
        public object BrowserPane { get; set; } = new BrowserPaneDesignTimeViewModel();
        public object QuotesBar { get; set; } = new QuotesBarDesignTimeViewModel();
        public object ActionsPad { get; set; } = new HomeActionsPadDesignTimeViewModel();

        public void Handle(NavigateMessage navigateMessage)
        {

        }
    }
}
