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
using Deadfile.Tab.JobChildren;
using Deadfile.Tab.JobChildren.Applications;
using Deadfile.Tab.JobChildren.BillableHours;
using Deadfile.Tab.JobChildren.Expenses;
using Deadfile.Tab.Jobs;
using Deadfile.Tab.Management.LocalAuthorities;
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
    /// No need to make this too general at present. How the hell do I clean this up though?
    /// </remarks>
    public sealed class TabModule
    {
        private readonly SimpleContainer _container;
        public TabModule(SimpleContainer globalContainer)
        {
            // I hate registering my IOC container with itself so I'm not going to do that.
            _container = globalContainer.CreateChildContainer();
            var navigationContainer = new NavigationContainer(_container);

            // Resolve to my NavigationService - a singleton per Tab.
            _container.RegisterInstance(typeof(INavigationService), nameof(NavigationService), new NavigationService(navigationContainer));

            // OK and now we have a local EventAggregator. We use the Prism version because we can keep it local to each tab.
            _container.RegisterSingleton(typeof(Prism.Events.IEventAggregator), nameof(Prism.Events.EventAggregator), typeof(Prism.Events.EventAggregator));

            // We have to tell him about everything in our module.
            _container.RegisterSingleton(typeof(TabViewModel), nameof(TabViewModel), typeof(TabViewModel));
            _container.RegisterSingleton(typeof(NavigationBarViewModel), RegionNames.NavigationBar, typeof(NavigationBarViewModel));

            // Pages (content) - referenced by key
            _container.RegisterSingleton(typeof(HomePageViewModel), Experience.Home + RegionNames.Page, typeof(HomePageViewModel));
            _container.RegisterSingleton(typeof(ClientsPageViewModel), Experience.Clients + RegionNames.Page, typeof(ClientsPageViewModel));
            _container.RegisterSingleton(typeof(JobsPageViewModel), Experience.Jobs + RegionNames.Page, typeof(JobsPageViewModel));
            _container.RegisterSingleton(typeof(LocalAuthoritiesPageViewModel), Experience.LocalAuthorities + RegionNames.Page, typeof(LocalAuthoritiesPageViewModel));

            // Job Children - referenced by key
            _container.RegisterSingleton(typeof(IApplicationsJobChildViewModel), JobChildExperience.Applications + JobChildKeys.JobChildKey, typeof(ApplicationsJobChildViewModel));
            _container.RegisterSingleton(typeof(IExpensesJobChildViewModel), JobChildExperience.Expenses + JobChildKeys.JobChildKey, typeof(ExpensesJobChildViewModel));
            _container.RegisterSingleton(typeof(IBillableHoursJobChildViewModel), JobChildExperience.BillableHours + JobChildKeys.JobChildKey, typeof(BillableHoursJobChildViewModel));

            // Actions pads
            _container.RegisterSingleton(typeof(HomeActionsPadViewModel), Experience.Home + RegionNames.ActionsPad, typeof(HomeActionsPadViewModel));
            _container.RegisterSingleton(typeof(ClientsActionsPadViewModel), Experience.Clients + RegionNames.ActionsPad, typeof(ClientsActionsPadViewModel));
            _container.RegisterSingleton(typeof(JobsActionsPadViewModel), Experience.Jobs + RegionNames.ActionsPad, typeof(JobsActionsPadViewModel));

            // Browser
            _container.RegisterSingleton(typeof(BrowserPaneViewModel), RegionNames.BrowserPane, typeof(BrowserPaneViewModel));

            // Quotes
            _container.RegisterSingleton(typeof(QuotesBarViewModel), RegionNames.QuotesBar, typeof(QuotesBarViewModel));
        }

        public IScreen GetFirstViewModel()
        {
            return _container.GetInstance<TabViewModel>();
        }
    }
}
