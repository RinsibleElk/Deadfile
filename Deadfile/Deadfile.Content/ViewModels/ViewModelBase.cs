using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    /// <summary>
    /// Navigation aware base class for ViewModels in the ContentRegion region.
    /// </summary>
    public class ViewModelBase : BindableBase, INavigationAware
    {
        protected readonly IEventAggregator EventAggregator;
        public ViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        private string _title = "Hello World";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventAggregator.GetEvent<NavigationEvent>().Publish(navigationContext.NavigationService.Journal);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
