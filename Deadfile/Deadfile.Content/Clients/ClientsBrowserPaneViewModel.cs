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
using Deadfile.Content.ViewModels;

namespace Deadfile.Content.Clients
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

        private string _clientsFilter;

        public string ClientsFilter
        {
            get { return _clientsFilter; }
            set
            {
                SetProperty(ref _clientsFilter, value);

                // Do it lame way for now and have SQL Server do the filter for me. It's on a delay anyway...
                var filteredClients = _repository.GetFilteredClients(_clientsFilter);
                Clients = CollectionViewSource.GetDefaultView(filteredClients);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var clients = _repository.GetClients();
            Clients = CollectionViewSource.GetDefaultView(clients);
        }
    }
}
