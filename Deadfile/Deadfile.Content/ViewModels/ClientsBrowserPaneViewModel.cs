using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public sealed class ClientsBrowserPaneViewModel : ViewModelBase, IClientsBrowserPaneViewModel
    {
        private readonly IDeadfileRepository _repository;
        public ClientsBrowserPaneViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        private ClientModel _selectedClient;

        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                EventAggregator.GetEvent<SelectedClientEvent>().Publish(value);
                SetProperty(ref _selectedClient, value);
            }
        }

        private ICollectionView _clients;
        public ICollectionView Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var clients = _repository.GetClients();
            Clients = CollectionViewSource.GetDefaultView(clients);
        }
    }
}
