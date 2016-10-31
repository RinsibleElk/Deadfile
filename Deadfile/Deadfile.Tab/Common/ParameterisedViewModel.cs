using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Generic base class for content view models, that casts navigation parameters up for the user.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterisedViewModel<T> : Screen, INavigationAware
    {
        public virtual void OnNavigatedTo(T parameters)
        {
            
        }

        public void OnNavigatedTo(object parameters)
        {
            OnNavigatedTo(parameters == null ? default(T) : (T)parameters);
        }

        public virtual void OnNavigatedFrom()
        {
        }
    }
}
