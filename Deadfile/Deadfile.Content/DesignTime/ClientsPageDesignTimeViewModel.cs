using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.DesignTime
{
    class ClientsPageDesignTimeViewModel : IClientsPageViewModel
    {
        public ClientsPageDesignTimeViewModel()
        {
            Clients = CollectionViewSource.GetDefaultView(DesignTimeClients.Clients);
        }
        public int SelectedClientId { get; set; } = 5;
        public ICollectionView Clients { get; private set; }
    }
}
