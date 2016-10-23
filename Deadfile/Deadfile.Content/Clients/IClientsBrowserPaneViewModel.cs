using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Content.Clients
{
    public interface IClientsBrowserPaneViewModel
    {
        ClientModel SelectedClient { get; set; }
        ICollectionView Clients { get; }
        string ClientsFilter { get; set; }
    }
}
