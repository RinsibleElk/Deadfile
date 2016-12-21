using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Model.Browser;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Events;
using Deadfile.Tab.Home;
using Deadfile.Tab.Import;
using Deadfile.Tab.Invoices;
using Deadfile.Tab.JobChildren;
using Deadfile.Tab.JobChildren.Applications;
using Deadfile.Tab.JobChildren.BillableHours;
using Deadfile.Tab.JobChildren.Expenses;
using Deadfile.Tab.JobChildren.JobTasks;
using Deadfile.Tab.Jobs;
using Deadfile.Tab.Json;
using Deadfile.Tab.Management.DefineQuotations;
using Deadfile.Tab.Management.LocalAuthorities;
using Deadfile.Tab.Management.TodoReport;
using Deadfile.Tab.Management.UnbilledClients;
using Deadfile.Tab.Navigation;
using Deadfile.Tab.Quotes;
using Deadfile.Tab.Tab;
using MahApps.Metro.Controls.Dialogs;

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
            _container.RegisterSingleton(typeof(UnbilledClientsPageViewModel), Experience.UnbilledClients + RegionNames.Page, typeof(UnbilledClientsPageViewModel));
            _container.RegisterSingleton(typeof(TodoReportPageViewModel), Experience.TodoReport + RegionNames.Page, typeof(TodoReportPageViewModel));
            _container.RegisterSingleton(typeof(ImportPageViewModel), Experience.Import + RegionNames.Page, typeof(ImportPageViewModel));
            _container.RegisterSingleton(typeof(JsonPageViewModel), Experience.Json + RegionNames.Page, typeof(JsonPageViewModel));
            _container.RegisterSingleton(typeof(DefineQuotationsPageViewModel), Experience.DefineQuotations + RegionNames.Page, typeof(DefineQuotationsPageViewModel));
            _container.RegisterSingleton(typeof(InvoicesPageViewModel), Experience.Invoices + RegionNames.Page, typeof(InvoicesPageViewModel));

            // Job Children - referenced by key
            _container.RegisterSingleton(typeof(JobTasksJobChildViewModel), JobChildExperience.JobTasks + JobChildKeys.JobChildKey, typeof(JobTasksJobChildViewModel));
            _container.RegisterSingleton(typeof(ApplicationsJobChildViewModel), JobChildExperience.Applications + JobChildKeys.JobChildKey, typeof(ApplicationsJobChildViewModel));
            _container.RegisterSingleton(typeof(ExpensesJobChildViewModel), JobChildExperience.Expenses + JobChildKeys.JobChildKey, typeof(ExpensesJobChildViewModel));
            _container.RegisterSingleton(typeof(BillableHoursJobChildViewModel), JobChildExperience.BillableHours + JobChildKeys.JobChildKey, typeof(BillableHoursJobChildViewModel));

            // Actions pads
            _container.RegisterSingleton(typeof(HomeActionsPadViewModel), Experience.Home + RegionNames.ActionsPad, typeof(HomeActionsPadViewModel));
            _container.RegisterSingleton(typeof(ClientsActionsPadViewModel), Experience.Clients + RegionNames.ActionsPad, typeof(ClientsActionsPadViewModel));
            _container.RegisterSingleton(typeof(JobsActionsPadViewModel), Experience.Jobs + RegionNames.ActionsPad, typeof(JobsActionsPadViewModel));
            _container.RegisterSingleton(typeof(InvoicesActionsPadViewModel), Experience.Invoices + RegionNames.ActionsPad, typeof(InvoicesActionsPadViewModel));

            // Browser
            _container.RegisterSingleton(typeof(BrowserPaneViewModel), RegionNames.BrowserPane, typeof(BrowserPaneViewModel));

            // Quotes
            _container.RegisterSingleton(typeof(QuotesBarViewModel), RegionNames.QuotesBar, typeof(QuotesBarViewModel));

            // Tab identity
            _container.RegisterSingleton(typeof(TabIdentity), nameof(TabIdentity), typeof(TabIdentity));
        }

        public IScreen GetFirstViewModel()
        {
            return _container.GetInstance<TabViewModel>();
        }

        public void NavigateToBrowserModel(BrowserModel browserModel)
        {
            _container.GetInstance<Prism.Events.IEventAggregator>()
                .GetEvent<SelectedItemEvent>()
                .Publish(new SelectedItemPacket(browserModel.ModelType, browserModel.ParentId, browserModel.Id));
        }
    }
}
