using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public abstract class ContentViewModelBase : ViewModelBase, IDeadfileViewModel
    {
        protected readonly IDeadfileNavigationService NavigationService;
        public ContentViewModelBase(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator)
        {
            NavigationService = navigationService;
        }
        public abstract Experience Experience { get; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            NavigationService.NavigateBrowserTo(this.Experience);
            NavigationService.NavigateActionsTo(this.Experience);
        }
    }
}
