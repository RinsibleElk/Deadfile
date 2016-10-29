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
using Prism.Mvvm;
using Prism.Regions;

namespace Deadfile.Content.Browser
{
    public sealed class BrowserPaneViewModel : BindableBase, IBrowserPaneViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeadfileRepository _repository;
        private SubscriptionToken _lockedForEditingActionSubscriptionToken = null;
        public BrowserPaneViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            Items = new ObservableCollection<BrowserModel>(_repository.GetBrowserItems(BrowserSettings));
            BrowserSettings.Refresh += BrowserSettingsRefresh;
        }

        private void BrowserSettingsRefresh(object sender, EventArgs e)
        {
            Items = new ObservableCollection<BrowserModel>(_repository.GetBrowserItems(BrowserSettings));
        }

        private void LockedForEditingAction(bool isLocked)
        {
            BrowsingEnabled = !isLocked;
        }

        private BrowserModel _selectedItem;
        public BrowserModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                    _eventAggregator.GetEvent<SelectedItemEvent>().Publish(new SelectedItemPacket(_selectedItem.ModelType, _selectedItem.Id));
            }
        }

        private ObservableCollection<BrowserModel> _items;
        public ObservableCollection<BrowserModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private BrowserSettings _browserSettings = new BrowserSettings();
        public BrowserSettings BrowserSettings
        {
            get { return _browserSettings; }
            set { SetProperty(ref _browserSettings, value); }
        }

        private bool _browsingEnabled = true;
        public bool BrowsingEnabled
        {
            get { return _browsingEnabled; }
            set
            {
                SetProperty(ref _browsingEnabled, value);
            }
        }

        private bool _isActive = false;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (SetProperty(ref _isActive, value))
                {
                    if (_isActive)
                    {
                        _lockedForEditingActionSubscriptionToken = _eventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(LockedForEditingAction);
                    }
                    else
                    {
                        _eventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingActionSubscriptionToken);
                    }
                }
            }
        }
        public event EventHandler IsActiveChanged;
    }
}
