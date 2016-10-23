using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public class ClientsPageViewModel : ViewModelBase, IClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        public ClientsPageViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository) : base(eventAggregator)
        {
            _repository = repository;
        }

        public int SelectedClientIndex { get; set; }

        private ClientModel selectedClient;
        public ClientModel SelectedClient
        {
            get { return selectedClient; }
            set { SetProperty(ref selectedClient, value); }
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
