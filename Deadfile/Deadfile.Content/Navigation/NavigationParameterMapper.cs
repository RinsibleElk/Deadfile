using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Regions;

namespace Deadfile.Content.Navigation
{
    /// <summary>
    /// Some reflection madness to convert complex types into <see cref="NavigationParameters"/> and back again.
    /// </summary>
    public class NavigationParameterMapper : INavigationParameterMapper
    {
        public NavigationParameters ConvertToNavigationParameters<T>(T value)
        {
            // for now just assume it's simple types
            var t = typeof(T);
            if (!t.IsPrimitive && t != typeof(string))
            {
                throw new NotImplementedException("I haven't implemented this for non-primitives yet");
            }
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Value", value);
            return navigationParameters;
        }

        public T ConvertToUserType<T>(NavigationParameters navigationParameters)
        {
            // for now just assume it's simple types
            var t = typeof(T);
            if (!t.IsPrimitive && t != typeof(string))
            {
                throw new NotImplementedException("I haven't implemented this for non-primitives yet");
            }
            var value = (T)navigationParameters["Value"];
            return value;
        }
    }
}
