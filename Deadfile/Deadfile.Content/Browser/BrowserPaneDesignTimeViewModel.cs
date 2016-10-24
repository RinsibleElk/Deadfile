using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.Browser
{
    class BrowserPaneDesignTimeViewModel : IBrowserPaneViewModel
    {
        public BrowserPaneDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            Clients=new ObservableCollection<BrowserClient>(repository.GetBrowserClients(null));
        }
        public ObservableCollection<BrowserClient> Clients { get; set; }
        public string FilterText { get; set; }
        public BrowserModel SelectedItem { get; set; }
    }
}
