using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    /// <summary>
    /// Generic base class for content view models, that converts NavigationParameters back into something usable.
    /// </summary>
    /// <remarks>
    /// I'm assuming here that it is best to keep the dictionary of values in the navigation parameters as simple types.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterisedContentViewModelBase<T> : ContentViewModelBase where T : new()
    {
        private readonly INavigationParameterMapper _navigationParameterMapper;
        public ParameterisedContentViewModelBase(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            INavigationParameterMapper navigationParameterMapper)
            : base(eventAggregator, navigationService)
        {
            _navigationParameterMapper = navigationParameterMapper;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext, T navigationParameter)
        {
        }

        public sealed override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            OnNavigatedTo(navigationContext, _navigationParameterMapper.ConvertToUserType<T>(navigationContext.Parameters));
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext, T navigationParameter)
        {
            return true;
        }

        public sealed override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return base.IsNavigationTarget(navigationContext) &&
                   IsNavigationTarget(navigationContext,
                       _navigationParameterMapper.ConvertToUserType<T>(navigationContext.Parameters));
        }
    }
}
