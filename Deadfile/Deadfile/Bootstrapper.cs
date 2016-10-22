using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using Deadfile.Content;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism.Modularity;

namespace Deadfile
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(ContentModule));
        }

        protected override IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
            container.RegisterType<IDeadfileNavigationService, DeadfileNavigationService>();
            return container;
        }
    }
}
