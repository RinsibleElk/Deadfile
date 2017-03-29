using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Action = System.Action;

namespace Deadfile.Tab.Tab
{
    interface ITabViewModel : IScreen
    {
        object NavigationBar { get; set; }
        IPageViewModel ContentArea { get; set; }
        object BrowserPane { get; set; }
        object QuotesBar { get; set; }
        object ActionsPad { get; set; }
        bool ActionsPadIsVisible { get; set; }
        bool BrowserPaneIsVisible { get; set; }
        bool BrowserPaneIsCollapsed { get; set; }
        ICommand CollapseBrowserPaneCommand { get; }
    }
}
