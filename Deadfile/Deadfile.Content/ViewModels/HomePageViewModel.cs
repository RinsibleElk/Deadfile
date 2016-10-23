using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Views;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        private readonly IDeadfileNavigationService _navigationService;
        private readonly DelegateCommand<object> _navigateCommand;
        private readonly DelegateCommand<object> _clientsCommand;

        public HomePageViewModel(IDeadfileNavigationService navigationService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _navigateCommand = new DelegateCommand<object>(this.NavigateToSecondPage);
            _clientsCommand = new DelegateCommand<object>(this.NavigateToClientsPage);
            Title = "Home Page";
        }

        private void NavigateToSecondPage(object ignored)
        {
            _navigationService.NavigateTo(Experience.SecondPage);
        }

        private void NavigateToClientsPage(object ignored)
        {
            _navigationService.NavigateTo(Experience.ClientsPage);
        }

        public ICommand NavigateCommand { get { return _navigateCommand; } }

        public ICommand ClientsCommand { get { return _clientsCommand; } }
    }
}

