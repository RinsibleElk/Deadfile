using Deadfile.Content.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.DesignTime;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;

namespace Deadfile.Content.Clients
{
    sealed class ClientsActionsPadDesignTimeViewModel : IClientsActionsPadViewModel
    {
        public ICommand AddClientCommand { get; } = null;
        public ICommand EditClientCommand { get; } = null;
        public ICommand SaveClientCommand { get; } = null;
        public ICommand DeleteClientCommand { get; } = null;
    }
}
