using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;

namespace Deadfile.Content.Interfaces
{
    public interface INavigationParameterMapper
    {
        /// <summary>
        /// Convert the user's type to NavigationParameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        NavigationParameters ConvertToNavigationParameters<T>(T value);

        /// <summary>
        /// Convert NavigationParameters to the user's type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="navigationParameters"></param>
        /// <returns></returns>
        T ConvertToUserType<T>(NavigationParameters navigationParameters);
    }
}
