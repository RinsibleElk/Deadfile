using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
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
            Logger.Info("Event,AddClientEvent,Send,{0}", _tabIdentity.TabIndex);
            _eventAggregator.GetEvent<AddClientEvent>().Publish(new AddClientMessage());
        }

        public void LocalAuthorities()
        {
            Logger.Info("Event,NavigateEvent,Send,{0},{1}", _tabIdentity.TabIndex, Experience.LocalAuthorities);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.LocalAuthorities));
        }

        public void DefineQuotations()
        {
            Logger.Info("Event,NavigateEvent,Send,{0},{1}", _tabIdentity.TabIndex, Experience.DefineQuotations);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.DefineQuotations));
        }

        public void UnbilledClients()
        {
            Logger.Info("Event,NavigateEvent,Send,{0},{1}", _tabIdentity.TabIndex, Experience.UnbilledClients);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.UnbilledClients));
        }

        public void Import()
        {
            Logger.Info("Event,NavigateEvent,Send,{0},{1}", _tabIdentity.TabIndex, Experience.Import);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.Import));
        }

        public void TodoReport()
        {
            Logger.Info("Event,NavigateEvent,Send,{0},{1}", _tabIdentity.TabIndex, Experience.TodoReport);
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.TodoReport));
        }

        public Experience Experience
        {
            get { return Experience.Home; }
        }

        public bool ShowActionsPad { get; } = true;
        public void CompleteNavigation()
        {
        }

        public void OnNavigatedTo(object parameters)
        {
            Logger.Info("Navigated to Home {0}", _tabIdentity.TabIndex);
            Logger.Info("Event,DisplayNameEvent,Send,{0},{1}", _tabIdentity.TabIndex, "Home");
            _eventAggregator.GetEvent<DisplayNameEvent>().Publish("Home");
        }

        public void OnNavigatedFrom()
        {
            Logger.Info("Navigated from Home {0}", _tabIdentity.TabIndex);
        }
    }
}
