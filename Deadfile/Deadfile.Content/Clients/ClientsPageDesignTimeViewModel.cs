using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.Clients
{
    class ClientsPageDesignTimeViewModel : IClientsPageViewModel
    {
        public ClientsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            var clientsList = new List<ClientModel>(repository.GetClients());
            SelectedClient = clientsList[5];
            Title = "Clients";
        }

        public ClientModel SelectedClient { get; set; }

        public Experience Experience { get { return Experience.Clients; } }

        public string Title { get; set; }

        public bool Editable { get; } = false;
        public List<string> Errors { get; } = new List<string>();
    }
}
