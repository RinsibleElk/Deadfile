using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;

namespace Deadfile.Tab.Tab
{
    interface ITabViewModel : IScreen
    {
        object NavigationBar { get; set; }
        IPageViewModel ContentArea { get; set; }
        object BrowserPane { get; set; }
        object QuotesBar { get; set; }
        object ActionsPad { get; set; }
        bool BrowserAndActionsAreVisible { get; set; }
    }
}
