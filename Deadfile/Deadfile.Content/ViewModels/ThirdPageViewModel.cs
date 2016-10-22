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
    public class ThirdPageViewModel : ViewModelBase, IThirdPageViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly DelegateCommand<object> _navigateCommand;

        private const string FourthPageViewKey = nameof(FourthPage);
        private static Uri FourthPageViewUri = new Uri(FourthPageViewKey, UriKind.Relative);

        public ThirdPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _regionManager = regionManager;
            _navigateCommand = new DelegateCommand<object>(NavigateToFourthPage);
            Title = "Third Page";
        }

        private void NavigateToFourthPage(object ignored)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, FourthPageViewUri);
        }

        public ICommand NavigateCommand { get { return _navigateCommand; } }
    }
}

