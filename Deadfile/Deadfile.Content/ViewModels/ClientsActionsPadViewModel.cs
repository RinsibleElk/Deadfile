using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public sealed class ClientsActionsPadViewModel : ViewModelBase, IClientsActionsPadViewModel
    {
        public ClientsActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
