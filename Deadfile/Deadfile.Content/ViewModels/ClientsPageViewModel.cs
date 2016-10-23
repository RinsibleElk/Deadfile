using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Events;

namespace Deadfile.Content.ViewModels
{
    public class ClientsPageViewModel : ViewModelBase, IClientsPageViewModel
    {
        public ClientsPageViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        private int selectedClientId;

        public int SelectedClientId
        {
            get { return selectedClientId; }
            set { SetProperty(ref selectedClientId, value); }
        }
        public ICollectionView Clients { get; }
    }
}
