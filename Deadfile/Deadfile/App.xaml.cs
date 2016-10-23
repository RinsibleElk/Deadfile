using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace Deadfile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                return ViewTypeToViewModelTypeResolver(viewType);
            });

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private static Type ViewTypeToViewModelTypeResolver(Type viewType)
        {
            var viewName = viewType.FullName;
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            // We're going to have a LOT of groups of types in the following vein:
            // - XView
            // - IXViewModel
            // - XDesignTimeViewModel
            // - XViewModel
            // Keep them together so it's easier to browse.
            var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
            return Type.GetType(viewModelName);
        }
    }
}
