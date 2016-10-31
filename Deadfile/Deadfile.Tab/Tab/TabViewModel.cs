using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Tab
{
    public class TabViewModel : Screen, ITabViewModel
    {
        private readonly INavigationService _navigationService;
        private object _navigationBar;
        private IPageViewModel _contentArea;
        private object _browserPane;
        private readonly Prism.Events.IEventAggregator _eventAggregator;

        public TabViewModel(Prism.Events.IEventAggregator eventAggregator, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<NavigateEvent>().Subscribe(Handle);
            eventAggregator.GetEvent<DisplayNameEvent>().Subscribe(DisplayNameChanged);
            navigationService.RequestNavigate(this, nameof(NavigationBar), "NavigationBar", null);
            navigationService.RequestNavigate(this, nameof(ContentArea), "HomePage", null);
            navigationService.RequestNavigate(this, nameof(BrowserPane), "BrowserPane", null);
            navigationService.RequestNavigate(this, nameof(QuotesBar), "QuotesBar", null);
        }

        private void DisplayNameChanged(string displayName)
        {
            DisplayName = displayName;
        }

        public object NavigationBar
        {
            get { return _navigationBar; }
            set
            {
                if (Equals(value, _navigationBar)) return;
                _navigationBar = value;
                NotifyOfPropertyChange(() => NavigationBar);
            }
        }

        public IPageViewModel ContentArea
        {
            get { return _contentArea; }
            set
            {
                if (Equals(value, _contentArea)) return;
                _contentArea = value;
                NotifyOfPropertyChange(() => ContentArea);
                _navigationService.RequestNavigate(this, nameof(ActionsPad), value.Experience + "ActionsPad", null);
            }
        }

        public object BrowserPane
        {
            get { return _browserPane; }
            set
            {
                if (Equals(value, _browserPane)) return;
                _browserPane = value;
                NotifyOfPropertyChange(() => BrowserPane);
            }
        }

        public object QuotesBar
        {
            get { return _quotesBar; }
            set
            {
                if (Equals(value, _quotesBar)) return;
                _quotesBar = value;
                NotifyOfPropertyChange(() => QuotesBar);
            }
        }

        public object ActionsPad
        {
            get { return _actionsPad; }
            set
            {
                if (Equals(value, _actionsPad)) return;
                _actionsPad = value;
                NotifyOfPropertyChange(() => ActionsPad);
            }
        }

        public void Handle(NavigateMessage message)
        {
            _navigationService.RequestNavigate(this, nameof(ContentArea), message.Experience + "Page", ModelBase.NewModelId);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe to selection changes.
            _navigateToSelectedClientSubscriptionToken = _eventAggregator.GetEvent<SelectedItemEvent>().Subscribe(NavigateToExperience);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe from selection changes.
            _eventAggregator.GetEvent<SelectedItemEvent>().Unsubscribe(_navigateToSelectedClientSubscriptionToken);
            _navigateToSelectedClientSubscriptionToken = null;
        }

        private SubscriptionToken _navigateToSelectedClientSubscriptionToken = null;
        private object _quotesBar;
        private object _actionsPad;

        private void NavigateToExperience(SelectedItemPacket packet)
        {
            switch (packet.Type)
            {
                case BrowserModelType.Client:
                    _navigationService.RequestNavigate(this, nameof(ContentArea), "ClientsPage", packet.Id);
                    break;
                case BrowserModelType.Job:
                    _navigationService.RequestNavigate(this, nameof(ContentArea), "JobsPage", packet.Id);
                    break;
            }
        }
    }
}
