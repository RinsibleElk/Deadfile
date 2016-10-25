using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Model;

namespace Deadfile.Content.Clients
{
    public interface IClientsPageViewModel : IDeadfileViewModel
    {
        ClientModel SelectedClient { get; set; }
        bool Editable { get; }
    }
}
