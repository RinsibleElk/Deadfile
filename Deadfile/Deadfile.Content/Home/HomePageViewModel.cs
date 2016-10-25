using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Home
{
    public class HomePageViewModel : ContentViewModelBase, IHomePageViewModel
    {
        private readonly DelegateCommand _clientsCommand;

        public HomePageViewModel(IDeadfileNavigationService navigationService, IEventAggregator eventAggregator) : base(eventAggregator, navigationService)
        {
            _clientsCommand = new DelegateCommand(() => this.NavigateToClientsPage(ClientModel.NewClientId));
            Title = "Home Page";
        }

        public override Experience Experience
        {
            get { return Experience.Home; }
        }

        private SubscriptionToken _navigateToSelectedClientSubscriptionToken;
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // Subscribe to selection changes.
            _navigateToSelectedClientSubscriptionToken = EventAggregator.GetEvent<SelectedClientEvent>().Subscribe(NavigateToClientsPage);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            // Unsubscribe from selection changes.
            EventAggregator.GetEvent<SelectedClientEvent>().Unsubscribe(_navigateToSelectedClientSubscriptionToken);
            _navigateToSelectedClientSubscriptionToken = null;
        }

        private void NavigateToClientsPage(int selectedClientId)
        {
            NavigationService.NavigateTo(Experience.Clients, selectedClientId);
        }

        public ICommand ClientsCommand { get { return _clientsCommand; } }
    }
}

