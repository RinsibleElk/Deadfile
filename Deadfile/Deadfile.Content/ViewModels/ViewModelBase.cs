using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    /// <summary>
    /// Navigation aware base class for ViewModels in the ContentRegion region.
    /// </summary>
    public class ViewModelBase : BindableBase, INavigationAware, IActiveAware
    {
        protected readonly IEventAggregator EventAggregator;
        public ViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnActiveChanged(bool isActive)
        {
            
        }

        private bool _isActive = false;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (SetProperty(ref _isActive, value))
                {
                    OnActiveChanged(_isActive);
                }
            }
        }
        public event EventHandler IsActiveChanged;
    }
}
