using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Prism.Commands;
using Prism.Events;
using Action = System.Action;
using LogManager = NLog.LogManager;
using INavigationService = Deadfile.Infrastructure.Interfaces.INavigationService;

namespace Deadfile.Tab.Tab
{
    public sealed class TabViewModel : Screen, ITabViewModel
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly INavigationService _navigationService;
        private object _navigationBar;
        private IPageViewModel _contentArea;
        private object _browserPane;
        private readonly Prism.Events.IEventAggregator _eventAggregator;

        public TabViewModel(TabIdentity tabIdentity,
            Prism.Events.IEventAggregator eventAggregator,
            INavigationService navigationService)
        {
            _tabIdentity = tabIdentity;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            DisplayName = "Home";
        }

        private void AddClientAction(AddClientMessage addClientAction)
        {
            _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Clients + "Page", new ClientNavigationKey(ModelBase.NewModelId));
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
                if (Equals(value, _contentArea))
                {
                    ContentArea?.CompleteNavigation();
                    return;
                }
                _contentArea = value;
                NotifyOfPropertyChange(() => ContentArea);
                if (_contentArea != null && _contentArea.ShowActionsPad)
                {
                    _navigationService.RequestNavigate(this, nameof(ActionsPad), value.Experience + "ActionsPad", null);
                    ActionsPadIsVisible = true;
                }
                else
                {
                    _navigationService.RequestDeactivate(this, nameof(ActionsPad));
                    ActionsPadIsVisible = false;
                }
                if (_contentArea != null && _contentArea.ShowBrowserPane)
                {
                    _navigationService.RequestNavigate(this, nameof(BrowserPane), "BrowserPane", null);
                    BrowserPaneIsVisible = true;
                }
                else
                {
                    _navigationService.RequestDeactivate(this, nameof(BrowserPane));
                    BrowserPaneIsVisible = false;
                }
                ContentArea?.CompleteNavigation();
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

        public bool ActionsPadIsVisible
        {
            get { return _actionsPadIsVisible; }
            set
            {
                if (_actionsPadIsVisible == value) return;
                _actionsPadIsVisible = value;
                NotifyOfPropertyChange(() => ActionsPadIsVisible);
            }
        }

        public bool BrowserPaneIsVisible
        {
            get { return _browserPaneIsVisible; }
            set
            {
                if (_browserPaneIsVisible == value) return;
                _browserPaneIsVisible = value;
                NotifyOfPropertyChange(() => BrowserPaneIsVisible);
            }
        }

        public void NavigateAction(NavigateMessage message)
        {
            _navigationService.RequestNavigate(this, nameof(ContentArea), message.Experience + "Page", null);
        }

        protected override void OnActivate()
        {
            Logger.Info("Activated tab {0}", _tabIdentity.TabIndex);

            base.OnActivate();

            if (_closed)
            {
                _navigationService.RequestNavigate(this, nameof(NavigationBar), "NavigationBar", null);
                _navigationService.RequestNavigate(this, nameof(ContentArea), "HomePage", null);
                _navigationService.RequestNavigate(this, nameof(QuotesBar), "QuotesBar", null);
                _closed = false;
            }

            _navigateEventSubscriptionToken = _eventAggregator.GetEvent<NavigateEvent>().Subscribe(NavigateAction);
            _displayNameEventSubscriptionToken = _eventAggregator.GetEvent<DisplayNameEvent>().Subscribe(DisplayNameChanged);
            _addClientEventSubscriptionToken = _eventAggregator.GetEvent<AddClientEvent>().Subscribe(AddClientAction);

            // Subscribe to selection changes.
            _navigateToSelectedClientSubscriptionToken = _eventAggregator.GetEvent<SelectedItemEvent>().Subscribe(NavigateToExperience);

            // Subscribe to the clients page requesting navigation to create a new job for a selected client.
            _addNewJobSubscriptionToken = _eventAggregator.GetEvent<AddNewJobEvent>().Subscribe(AddNewJobAction);

            // Subscribe to the clients page requesting navigation to create a new invoice for a selected client.
            _invoiceClientSubscriptionToken = _eventAggregator.GetEvent<InvoiceClientEvent>().Subscribe(InvoiceClientAction);
        }

        internal void TestOnlyOnActivate()
        {
            OnActivate();
        }

        private void AddNewJobAction(int clientId)
        {
            _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Jobs + RegionNames.Page, new ClientAndJobNavigationKey(clientId, ModelBase.NewModelId));
        }

        private void InvoiceClientAction(int clientId)
        {
            _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Invoices + RegionNames.Page, new ClientAndInvoiceNavigationKey(clientId, ModelBase.NewModelId));
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            _eventAggregator.GetEvent<NavigateEvent>().Unsubscribe(_navigateEventSubscriptionToken);
            _navigateEventSubscriptionToken = null;
            _eventAggregator.GetEvent<DisplayNameEvent>().Unsubscribe(_displayNameEventSubscriptionToken);
            _displayNameEventSubscriptionToken = null;
            _eventAggregator.GetEvent<AddClientEvent>().Unsubscribe(_addClientEventSubscriptionToken);
            _addClientEventSubscriptionToken = null;

            // Unsubscribe from selection changes.
            _eventAggregator.GetEvent<SelectedItemEvent>().Unsubscribe(_navigateToSelectedClientSubscriptionToken);
            _navigateToSelectedClientSubscriptionToken = null;

            // Unsubscribe from the clients page notifications.
            _eventAggregator.GetEvent<AddNewJobEvent>().Unsubscribe(_addNewJobSubscriptionToken);
            _addNewJobSubscriptionToken = null;
            _eventAggregator.GetEvent<InvoiceClientEvent>().Unsubscribe(_invoiceClientSubscriptionToken);
            _invoiceClientSubscriptionToken = null;

            // Tear everything down and free up resources.
            //TODO for some reason these don't all actually get freed - what the hell is holding on to these damn things????!!
            if (close)
            {
                _closed = true;
                _navigationService.RequestDeactivate(this, nameof(ContentArea));
                _navigationService.RequestDeactivate(this, nameof(BrowserPane));
                _navigationService.RequestDeactivate(this, nameof(ActionsPad));
                _navigationService.RequestDeactivate(this, nameof(NavigationBar));
                _navigationService.Teardown();
            }
        }

        internal void TestOnlyOnDeactivate(bool close)
        {
            OnDeactivate(close);
        }

        private SubscriptionToken _navigateToSelectedClientSubscriptionToken = null;
        private object _quotesBar;
        private object _actionsPad;
        private SubscriptionToken _addNewJobSubscriptionToken = null;
        private SubscriptionToken _invoiceClientSubscriptionToken = null;
        private SubscriptionToken _navigateEventSubscriptionToken = null;
        private SubscriptionToken _displayNameEventSubscriptionToken;
        private SubscriptionToken _addClientEventSubscriptionToken;
        private bool _closed = true;
        private bool _actionsPadIsVisible;
        private int _browserPaneWidth;
        private bool _browserPaneIsVisible;
        private bool _browserPaneIsCollapsed;

        private void NavigateToExperience(SelectedItemPacket packet)
        {
            switch (packet.Type)
            {
                case BrowserModelType.Client:
                    _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Clients + RegionNames.Page, new ClientNavigationKey(packet.Id));
                    break;
                case BrowserModelType.Job:
                    _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Jobs + RegionNames.Page, new ClientAndJobNavigationKey(packet.ParentId, packet.Id));
                    break;
                case BrowserModelType.Invoice:
                    _navigationService.RequestNavigate(this, nameof(ContentArea), Experience.Invoices + RegionNames.Page, new ClientAndInvoiceNavigationKey(packet.ParentId, packet.Id));
                    break;
            }
        }
    }
}
