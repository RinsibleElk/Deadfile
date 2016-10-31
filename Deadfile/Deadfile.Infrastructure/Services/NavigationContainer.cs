using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    /// <summary>
    /// Implementation of <see cref="INavigationContainer"/> that uses a <see cref="SimpleContainer"/>.
    /// </summary>
    public class NavigationContainer : INavigationContainer
    {
        private readonly SimpleContainer _container;

        /// <summary>
        /// Create a new instance of the navigation container.
        /// </summary>
        /// <param name="container"></param>
        public NavigationContainer(SimpleContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Perform lookup based on key. No caching is done - this is left to the container.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetInstance(string key)
        {
            return _container.GetInstance(null, key);
        }
    }
}
