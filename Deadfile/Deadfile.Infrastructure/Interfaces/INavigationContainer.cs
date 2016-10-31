using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Services;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Allows unit testability of the <see cref="NavigationService"/>.
    /// </summary>
    public interface INavigationContainer
    {
        /// <summary>
        /// Get an instance of a view model by string key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetInstance(string key);
    }
}
