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
            get { return Experience.HomePage; }
        }

        private void NavigateToClientsPage(object ignored)
        {
            NavigationService.NavigateTo(Experience.ClientsPage);
        }

        public ICommand ClientsCommand { get { return _clientsCommand; } }
    }
}

