﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Core;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Events;
using Prism.Events;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Browser
{
    public sealed class BrowserPaneViewModel : Screen, IBrowserPaneViewModel, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeadfileRepository _repository;
        private SubscriptionToken _lockedForEditingActionSubscriptionToken = null;
        private SubscriptionToken _refreshBrowserSubscriptionToken = null;
        public BrowserPaneViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            Items = new ObservableCollection<BrowserModel>(_repository.GetBrowserItems(BrowserSettings));
            BrowserSettings.Refresh += BrowserSettingsRefresh;
        }

        private void BrowserSettingsRefresh(object sender, EventArgs e)
        {
            RefreshBrowser();
        }

        private bool _ignoreSelectionChange;
        private void RefreshBrowser()
        {
            _ignoreSelectionChange = true;
            Items = new ObservableCollection<BrowserModel>(_repository.GetBrowserItems(BrowserSettings));
            _ignoreSelectionChange = false;
        }

        private void LockedForEditingAction(LockedForEditingMessage message)
        {
            BrowsingEnabled = (!message.IsLocked);
        }

        private void RefreshBrowserAction(RefreshBrowserMessage message)
        {
            RefreshBrowser();
        }

        private BrowserModel _selectedItem;
        public BrowserModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (!_ignoreSelectionChange)
                {
                    if (_selectedItem == value) return;
                    _selectedItem = value;
                    NotifyOfPropertyChange(() => SelectedItem);
                    if (_selectedItem != null)
                        _eventAggregator.GetEvent<SelectedItemEvent>().Publish(new SelectedItemPacket(_selectedItem.ModelType, _selectedItem.ParentId, _selectedItem.Id));
                }
            }
        }

        private ObservableCollection<BrowserModel> _items;

        public ObservableCollection<BrowserModel> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        private BrowserSettings _browserSettings = new BrowserSettings();
        public BrowserSettings BrowserSettings
        {
            get { return _browserSettings; }
            set
            {
                if (_browserSettings == value) return;
                _browserSettings = value;
                NotifyOfPropertyChange(() => BrowserSettings);
            }
        }

        private bool _browsingEnabled = true;
        public bool BrowsingEnabled
        {
            get { return _browsingEnabled; }
            set
            {
                if (_browsingEnabled == value) return;
                _browsingEnabled = value;
                NotifyOfPropertyChange(() => BrowsingEnabled);
            }
        }

        public void OnNavigatedTo(object parameters)
        {
            _lockedForEditingActionSubscriptionToken = _eventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(LockedForEditingAction);
            _refreshBrowserSubscriptionToken = _eventAggregator.GetEvent<RefreshBrowserEvent>().Subscribe(RefreshBrowserAction);
        }

        public void OnNavigatedFrom()
        {
            _eventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingActionSubscriptionToken);
            _eventAggregator.GetEvent<RefreshBrowserEvent>().Unsubscribe(_refreshBrowserSubscriptionToken);
        }
    }
}
