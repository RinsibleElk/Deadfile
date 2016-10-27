using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public NavigationParameters ConvertToNavigationParameters<T>(T value) where T : new()
        {
            // convention based
            var navigationParameters = new NavigationParameters();
            var t = typeof(T);
            if (t.IsPrimitive || t == typeof(string))
            {
                navigationParameters.Add("Value", value);
            }
            else
            {
                foreach (var propertyInfo in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        navigationParameters.Add(propertyInfo.Name, propertyInfo.GetMethod.Invoke(value, new object[0]));
                    }
                }
            }
            return navigationParameters;
        }

        public T ConvertToUserType<T>(NavigationParameters navigationParameters) where T : new()
        {
            // convention based
            var t = typeof(T);
            if (!t.IsPrimitive && t != typeof(string))
            {
                var value = new T();
                foreach (var propertyInfo in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetMethod.Invoke(value, new object[] { navigationParameters[propertyInfo.Name] });
                    }
                }
                return value;
            }
            else
                return (T)navigationParameters["Value"];
        }
    }
}
