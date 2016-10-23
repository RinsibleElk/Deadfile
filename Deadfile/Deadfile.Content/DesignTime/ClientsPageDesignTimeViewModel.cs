﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Deadfile.Model.DesignTime;

namespace Deadfile.Content.DesignTime
{
    class ClientsPageDesignTimeViewModel : IClientsPageViewModel
    {
        public ClientsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            var clientsList = new List<ClientModel>(repository.GetClients());
            SelectedClient = clientsList[5];
        }

        public ClientModel SelectedClient { get; set; }
    }
}
