using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Common
{
    public abstract class NavigationAwareViewModel : Screen, INavigationAware
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public abstract void OnNavigatedTo(object parameters);
        public virtual void OnNavigatedFrom()
        {
        }
    }

    /// <summary>
    /// Generic base class for content view models, that casts navigation parameters up for the user.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterisedViewModel<T> : NavigationAwareViewModel
    {
        public virtual void OnNavigatedTo(T parameters)
        {
            
        }
        public sealed override void OnNavigatedTo(object parameters)
        {
            OnNavigatedTo(parameters == null ? default(T) : (T)parameters);
        }
    }
}
