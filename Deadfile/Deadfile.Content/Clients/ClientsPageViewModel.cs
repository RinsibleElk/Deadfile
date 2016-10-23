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
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Clients
{
    public class ClientsPageViewModel : ContentViewModelBase, IClientsPageViewModel
    {
        public ClientsPageViewModel(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator, navigationService)
        {
        }

        public override Experience Experience
        {
            get { return Experience.Clients; }
        }

        private ClientModel _selectedClient;
        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private ICollectionView _clients;
        public ICollectionView Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // subscribe to messages from the browser pane
            EventAggregator.GetEvent<SelectedClientEvent>().Subscribe(SelectedClientChanged);
            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // unsubscribe to messages from the browser pane
            EventAggregator.GetEvent<SelectedClientEvent>().Unsubscribe(SelectedClientChanged);

            base.OnNavigatedFrom(navigationContext);
        }

        private void SelectedClientChanged(ClientModel selectedClient)
        {
            SelectedClient = selectedClient;
        }
    }
}
