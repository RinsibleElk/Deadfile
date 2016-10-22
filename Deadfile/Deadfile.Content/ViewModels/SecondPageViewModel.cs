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
    public class SecondPageViewModel : ViewModelBase, ISecondPageViewModel
    {
        private readonly IDeadfileNavigationService _navigationService;
        private readonly DelegateCommand<object> _navigateCommand;

        public SecondPageViewModel(IDeadfileNavigationService navigationService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _navigateCommand = new DelegateCommand<object>(this.NavigateToThirdPage);
            Title = "Second Page";
        }

        private void NavigateToThirdPage(object ignored)
        {
            _navigationService.NavigateTo(Experience.ThirdPage);
        }

        public ICommand NavigateCommand { get { return _navigateCommand; } }
    }
}

