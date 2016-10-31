using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Home;
using Deadfile.Tab.Jobs;
using Deadfile.Tab.Navigation;
using Deadfile.Tab.Quotes;
using Deadfile.Tab.Tab;

namespace Deadfile.Tab
{
    /// <summary>
    /// On creation of a new tab, a new TabModule is created with the global container. Its responsibilities are to create
    /// a local child container - used in Navigation - and a local event aggregator and to prepare the child container to be able to
    /// handle navigation.
    /// </summary>
    /// <remarks>
    /// No need to make this too general at present.
    /// </remarks>
    public sealed class TabModule
    {
        private readonly SimpleContainer _container;
        public TabModule(SimpleContainer globalContainer)
        {
            _container = globalContainer.CreateChildContainer();

            // Resolve to my NavigationService - a singleton per Tab.
            _container.RegisterInstance(typeof(INavigationService), nameof(NavigationService), new NavigationService(_container));

            // OK and now we have a local EventAggregator override.
            _container.RegisterSingleton(typeof(Prism.Events.IEventAggregator), nameof(Prism.Events.EventAggregator), typeof(Prism.Events.EventAggregator));

            // And we have a local TabServices.
            _container.RegisterSingleton(typeof(ITabServices), nameof(TabServices), typeof(TabServices));

            // We have to tell him about everything in our module.
            _container.RegisterSingleton(typeof(TabViewModel), nameof(TabViewModel), typeof(TabViewModel));
            _container.RegisterSingleton(typeof(NavigationBarViewModel), "NavigationBar", typeof(NavigationBarViewModel));
            _container.RegisterSingleton(typeof(HomePageViewModel), "HomePage", typeof(HomePageViewModel));
            _container.RegisterSingleton(typeof(ClientsPageViewModel), "ClientsPage", typeof(ClientsPageViewModel));
            _container.RegisterSingleton(typeof(JobsPageViewModel), "JobsPage", typeof(JobsPageViewModel));
            _container.RegisterSingleton(typeof(BrowserPaneViewModel), "BrowserPane", typeof(BrowserPaneViewModel));
            _container.RegisterSingleton(typeof(QuotesBarViewModel), "QuotesBar", typeof(QuotesBarViewModel));
            _container.RegisterSingleton(typeof(HomeActionsPadViewModel), "HomeActionsPad", typeof(HomeActionsPadViewModel));
            _container.RegisterSingleton(typeof(ClientsActionsPadViewModel), "ClientsActionsPad", typeof(ClientsActionsPadViewModel));
            _container.RegisterSingleton(typeof(JobsActionsPadViewModel), "JobsActionsPad", typeof(JobsActionsPadViewModel));
        }

        public IScreen GetFirstViewModel()
        {
            return _container.GetInstance<TabViewModel>();
        }
    }
}
