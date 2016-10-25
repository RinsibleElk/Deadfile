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
    public class ClientsPageViewModel : ParameterisedContentViewModelBase<int>, IClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        public ClientsPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            IDeadfileRepository repository,
            INavigationParameterMapper mapper)
            : base(eventAggregator, navigationService, mapper)
        {
            _repository = repository;
        }

        public override Experience Experience
        {
            get { return Experience.Clients; }
        }

        private ClientModel _selectedClient;
        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                // Careful - we don't want to use ReferenceEquals here!!!
                if ((_selectedClient == null) || (value == null) || (_selectedClient.ClientId != value.ClientId))
                    SetProperty(ref _selectedClient, value);
            }
        }

        private SubscriptionToken _navigateToSelectedClientSubscriptionToken = null;
        public override void OnNavigatedTo(NavigationContext navigationContext, int selectedClientId)
        {
            //TODO This could fail!!! We'd have to navigate back...
            if (selectedClientId == ClientModel.NewClientId)
                SelectedClient = null;
            else
                SelectedClient = _repository.GetClientById(selectedClientId);

            // subscribe to messages from the browser pane
            if (_navigateToSelectedClientSubscriptionToken == null)
                _navigateToSelectedClientSubscriptionToken = EventAggregator.GetEvent<SelectedClientEvent>().Subscribe(NavigateToClientsPage);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // unsubscribe to messages from the browser pane
            EventAggregator.GetEvent<SelectedClientEvent>().Unsubscribe(_navigateToSelectedClientSubscriptionToken);
            _navigateToSelectedClientSubscriptionToken = null;

            base.OnNavigatedFrom(navigationContext);
        }

        private void NavigateToClientsPage(int selectedClientId)
        {
            NavigationService.NavigateTo(Experience.Clients, selectedClientId);
        }

        private bool _editable = false;
        public bool Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }
    }
}
