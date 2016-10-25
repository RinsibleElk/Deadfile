using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Deadfile.Content.Events;
using Deadfile.Content.ViewModels;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Browser
{
    public sealed class BrowserPaneViewModel : ViewModelBase, IBrowserPaneViewModel
    {
        private readonly IDeadfileRepository _repository;
        public BrowserPaneViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository) : base(eventAggregator)
        {
            _repository = repository;
            Clients = new ObservableCollection<BrowserClient>(_repository.GetBrowserClients(null));
        }

        private SubscriptionToken _navigationEnabledSubscriptionToken = null;
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // Subscribe to event from the content page.
            _navigationEnabledSubscriptionToken =
                EventAggregator.GetEvent<NavigationEnabledEvent>().Subscribe(NavigationEnabledAction);
        }

        private void NavigationEnabledAction(bool enabled)
        {
            BrowsingEnabled = enabled;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            // Unsubscribe to event from the content page.
            EventAggregator.GetEvent<NavigationEnabledEvent>().Unsubscribe(_navigationEnabledSubscriptionToken);
            _navigationEnabledSubscriptionToken = null;
        }

        private BrowserModel _selectedItem;
        public BrowserModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    switch (_selectedItem.ModelType)
                    {
                        //TODO Deal with failures of all these...? By just not allowing the selection change?
                        case BrowserModelType.Client:
                            EventAggregator.GetEvent<SelectedClientEvent>().Publish(_selectedItem.Id);
                            break;
                        case BrowserModelType.Job:
                            EventAggregator.GetEvent<SelectedJobEvent>().Publish(_selectedItem.Id);
                            break;
                        case BrowserModelType.Invoice:
                            EventAggregator.GetEvent<SelectedInvoiceEvent>().Publish(_selectedItem.Id);
                            break;
                    }
                }
            }
        }

        private ObservableCollection<BrowserClient> _clients;
        public ObservableCollection<BrowserClient> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private string _filterText = "";

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    Clients = new ObservableCollection<BrowserClient>(_repository.GetBrowserClients(_filterText));
                }
            }
        }

        private FilterSettings _filterSettings = new FilterSettings();
        public FilterSettings FilterSettings
        {
            get { return _filterSettings; }
            set { SetProperty(ref _filterSettings, value); }
        }

        private bool _browsingEnabled = true;
        public bool BrowsingEnabled
        {
            get { return _browsingEnabled; }
            set { SetProperty(ref _browsingEnabled, value); }
        }
    }
}
