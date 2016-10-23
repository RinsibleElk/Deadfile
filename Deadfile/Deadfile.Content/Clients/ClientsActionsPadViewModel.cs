using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;
using Deadfile.Content.ViewModels;
using Prism.Commands;

namespace Deadfile.Content.Clients
{
    public sealed class ClientsActionsPadViewModel : ViewModelBase, IClientsActionsPadViewModel
    {
        private readonly IDeadfileNavigationService _navigationService;
        private readonly DelegateCommand _editClientCommand;
        private readonly DelegateCommand _newClientCommand;
        public ClientsActionsPadViewModel(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _editClientCommand = new DelegateCommand(EditClientAction);
            _newClientCommand = new DelegateCommand(NewClientAction);
        }

        public ICommand NewClientCommand { get { return _newClientCommand; } }
        public ICommand EditClientCommand { get { return _editClientCommand; } }

        private SubscriptionToken _selectedClientChangedSubscriptionToken = null;
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _selectedClientChangedSubscriptionToken = EventAggregator.GetEvent<SelectedClientEvent>().Subscribe(SelectedClientChanged);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            EventAggregator.GetEvent<SelectedClientEvent>().Unsubscribe(_selectedClientChangedSubscriptionToken);
            _selectedClientChangedSubscriptionToken = null;
            base.OnNavigatedFrom(navigationContext);
        }

        private int _selectedClientId = ClientModel.NewClientId;
        private void SelectedClientChanged(ClientModel clientModel)
        {
            _selectedClientId = clientModel.ClientId;
        }

        private void EditClientAction()
        {
            _navigationService.NavigateTo(Experience.EditClient, _selectedClientId);
        }

        private void NewClientAction()
        {
            _navigationService.NavigateTo(Experience.EditClient, ClientModel.NewClientId);
        }
    }
}
