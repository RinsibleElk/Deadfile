using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Events;
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

        private string _title = "Hello World";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            EventAggregator.GetEvent<NavigationEvent>().Publish(navigationContext.NavigationService.Journal);
            NavigationService.NavigateBrowserTo(this.Experience);
            NavigationService.NavigateActionsTo(this.Experience);
        }
    }
}
