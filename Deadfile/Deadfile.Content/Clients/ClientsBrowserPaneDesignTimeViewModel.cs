using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.Clients
{
    class ClientsBrowserPaneDesignTimeViewModel : IClientsBrowserPaneViewModel
    {
        public ClientsBrowserPaneDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            var clientsList = new List<ClientModel>(repository.GetClients());
            Clients = CollectionViewSource.GetDefaultView(clientsList);
            SelectedClient = clientsList[5];
        }
        public ClientModel SelectedClient { get; set; }
        public ICollectionView Clients { get; }
    }
}
