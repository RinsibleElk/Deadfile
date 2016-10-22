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
    public class SecondPageViewModel : ViewModelBase, ISecondPageViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly DelegateCommand<object> _navigateCommand;

        private const string ThirdPageViewKey = nameof(ThirdPage);
        private static Uri ThirdPageViewUri = new Uri(ThirdPageViewKey, UriKind.Relative);

        public SecondPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _regionManager = regionManager;
            _navigateCommand = new DelegateCommand<object>(this.NavigateToThirdPage);
            Title = "Second Page";
        }

        private void NavigateToThirdPage(object ignored)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ThirdPageViewUri);
        }

        public ICommand NavigateCommand { get { return _navigateCommand; } }
    }
}

