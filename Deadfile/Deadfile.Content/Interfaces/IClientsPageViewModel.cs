using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Content.Interfaces
{
    public interface IClientsPageViewModel
    {
        int SelectedClientId { get; set; }

        ICollectionView Clients { get; }
    }
}
