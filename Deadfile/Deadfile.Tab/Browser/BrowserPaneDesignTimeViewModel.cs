using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;

namespace Deadfile.Tab.Browser
{
    class BrowserPaneDesignTimeViewModel : IBrowserPaneViewModel
    {
        public BrowserPaneDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            Items = new ObservableCollection<BrowserModel>(repository.GetBrowserItems(BrowserSettings));
        }
        public ObservableCollection<BrowserModel> Items { get; set; }
        public BrowserModel SelectedItem { get; set; }
        public BrowserSettings BrowserSettings { get; set; } = new BrowserSettings();
        public bool BrowsingEnabled { get; set; } = true;
    }
}
