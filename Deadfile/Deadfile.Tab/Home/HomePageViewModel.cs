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
        private readonly IEventAggregator _eventAggregator;
        public HomePageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void AddClient()
        {
            _eventAggregator.GetEvent<AddClientEvent>().Publish(new AddClientMessage());
        }

        public void LocalAuthorities()
        {
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.LocalAuthorities));
        }

        public void DefineQuotations()
        {
            _eventAggregator.GetEvent<NavigateEvent>().Publish(new NavigateMessage(Experience.DefineQuotations));
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
            _eventAggregator.GetEvent<DisplayNameEvent>().Publish("Home");
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
