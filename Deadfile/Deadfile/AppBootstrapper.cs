using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab;
using Dragablz;
using MahApps.Metro.Controls.Dialogs;

namespace Deadfile
{
    /// <summary>
    /// Bootstrapper using Caliburn.Micro's SimpleContainer at two scopes - the Global one defined below declares the global services
    /// while modules then use child containers to ensure that messaging and navigation within their scope is local.
    /// </summary>
    public class AppBootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initialize <see cref="AppBootstrapper"/>.
        /// </summary>
        public AppBootstrapper()
        {
            Initialize();
        }

        /// <summary>
        /// Start up the first <see cref="ShellViewModel"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        /// <summary>
        /// The global container.
        /// </summary>
        private static readonly SimpleContainer ContainerInstance = new SimpleContainer();

        /// <summary>
        /// Configure the global container.
        /// </summary>
        protected override void Configure()
        {
            // These things are global.
            ContainerInstance.RegisterInstance(typeof(IWindowManager), nameof(WindowManager), new WindowManager());
            ContainerInstance.RegisterPerRequest(typeof(ShellViewModel), nameof(ShellViewModel), typeof(ShellViewModel));
            ContainerInstance.RegisterInstance(typeof(SimpleContainer), nameof(SimpleContainer), ContainerInstance);
            ContainerInstance.RegisterSingleton(typeof(IInterTabClient), nameof(InterTabClient), typeof(InterTabClient));

            // Data.
            ContainerInstance.RegisterSingleton(typeof(IRandomNumberGenerator), nameof(RandomNumberGenerator), typeof(RandomNumberGenerator));
            ContainerInstance.RegisterSingleton(typeof(IDeadfileRepository), nameof(DeadfileRepository), typeof(DeadfileRepository));
            ContainerInstance.RegisterSingleton(typeof(IModelEntityMapper), nameof(ModelEntityMapper), typeof(ModelEntityMapper));

            // Timers.
            ContainerInstance.RegisterSingleton(typeof(IQuotationsTimerService), nameof(QuotationsTimerService), typeof(QuotationsTimerService));
            ContainerInstance.RegisterSingleton(typeof(IDeadfileDispatcherTimerService), nameof(DeadfileDeadfileDispatcherTimerService), typeof(DeadfileDeadfileDispatcherTimerService));

            // Navigation to external browser.
            ContainerInstance.RegisterSingleton(typeof(IUrlNavigationService), nameof(UrlNavigationService), typeof(UrlNavigationService));

            // For testing. If the database is empty, populate it with fake data.
//            var repo = ContainerInstance.GetInstance<IDeadfileRepository>();
//            repo.SetUpFakeData();

            // We have a module for each tab. This creates the local environment for each tab. For instance, navigation and messaging are local within each tab.
            ContainerInstance.RegisterPerRequest(typeof(TabModule), nameof(TabModule), typeof(TabModule));

            // Dialog coordinator
            ContainerInstance.RegisterInstance(typeof(IDialogCoordinator), nameof(DialogCoordinator), DialogCoordinator.Instance);
            ContainerInstance.RegisterSingleton(typeof(IDeadfileDialogCoordinator), nameof(DeadfileDialogCoordinator), typeof(DeadfileDialogCoordinator));

            // Print service
            ContainerInstance.RegisterSingleton(typeof(IPrintService), nameof(PrintService), typeof(PrintService));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return ContainerInstance.GetAllInstances(service);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return ContainerInstance.GetInstance(serviceType, key);
        }

        protected override void BuildUp(object instance)
        {
            ContainerInstance.BuildUp(instance);
        }

        /// <summary>
        /// Magic string below for the <see cref="TabModule"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("Deadfile.Tab")
            };
        }

    }
}
