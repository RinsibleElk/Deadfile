using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.ViewModels;
using Deadfile.Model;

namespace Deadfile.Content.Clients
{
    public interface IClientsPageViewModel : IDeadfileViewModel, IEditableItemViewModel<ClientModel>
    {
    }
}
