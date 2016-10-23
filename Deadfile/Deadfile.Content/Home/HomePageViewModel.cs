using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.ViewModels;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Home
{
    public class HomePageViewModel : ContentViewModelBase, IHomePageViewModel
    {
        private readonly DelegateCommand<object> _clientsCommand;

        public HomePageViewModel(IDeadfileNavigationService navigationService, IEventAggregator eventAggregator) : base(eventAggregator, navigationService)
        {
            _clientsCommand = new DelegateCommand<object>(this.NavigateToClientsPage);
            Title = "Home Page";
        }

        public override Experience Experience
        {
            get { return Experience.Home; }
        }

        private void NavigateToClientsPage(object ignored)
        {
            NavigationService.NavigateTo(Experience.Clients);
        }

        public ICommand ClientsCommand { get { return _clientsCommand; } }
    }
}

