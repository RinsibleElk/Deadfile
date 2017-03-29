using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
using Prism.Commands;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Home
{
    class HomePageViewModel : Screen, IHomePageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IEventAggregator _eventAggregator;
        public HomePageViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator)
        {
            _tabIdentity = tabIdentity;
            _eventAggregator = eventAggregator;
        }

        public void AddClient()
        {
            Logger.Info("Event|AddClientEvent|Send|{0}", _tabIdentity.TabIndex);
            _eventAggregator.GetEvent<AddClientEvent>().Publish(new AddClientMessage());
        }

        public void LocalAuthorities()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.LocalAuthorities);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.LocalAuthorities));
        }

        public void DefineQuotations()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.DefineQuotations);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.DefineQuotations));
        }

        public void UnbilledJobs()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.UnbilledJobs);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.UnbilledJobs));
        }

        public void TodoReport()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.TodoReport);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.TodoReport));
        }

        public void ImportExportToJson()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.Json);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.Json));
        }

        public void UnpaidInvoices()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.UnpaidInvoices);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.UnpaidInvoices));
        }

        public void CurrentApplications()
        {
            Logger.Info("Event|NavigateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, Experience.CurrentApplications);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.CurrentApplications));
        }

        public Experience Experience => Experience.Home;

        public bool ShowActionsPad { get; } = false;
        public bool ShowBrowserPane { get; } = true;
        public void CompleteNavigation()
        {
        }

        public void OnNavigatedTo(object parameters)
        {
            Logger.Info("Navigated to Home {0}", _tabIdentity.TabIndex);
            Logger.Info("Event|DisplayNameEvent|Send|{0}|{1}", _tabIdentity.TabIndex, "Home");
            _eventAggregator.GetEvent<DisplayNameEvent>().Publish("Home");
        }

        public void OnNavigatedFrom()
        {
            Logger.Info("Navigated from Home {0}", _tabIdentity.TabIndex);
        }
    }
}
