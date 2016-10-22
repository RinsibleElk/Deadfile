using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Views;
using Deadfile.Content.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly DelegateCommand<object> _navigateCommand;

        private const string SecondPageViewKey = nameof(SecondPage);
        private static Uri SecondPageViewUri = new Uri(SecondPageViewKey, UriKind.Relative);

        public HomePageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _regionManager = regionManager;
            _navigateCommand = new DelegateCommand<object>(this.NavigateToSecondPage);
            Title = "Home Page";
        }

        private void NavigateToSecondPage(object ignored)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, SecondPageViewUri);
        }

        public ICommand NavigateCommand { get { return _navigateCommand; } }
    }
}

