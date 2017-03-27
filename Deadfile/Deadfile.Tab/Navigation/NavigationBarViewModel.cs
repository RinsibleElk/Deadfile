using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Events;
using Prism.Commands;
using Prism.Events;

namespace Deadfile.Tab.Navigation
{
    public class NavigationBarViewModel : PropertyChangedBase, INavigationBarViewModel, INavigationAware
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly INavigationService _navigationService;
        private readonly Prism.Events.IEventAggregator _eventAggregator;
        private readonly IDeadfileRepository _repository;
        private bool _canHome;
        private bool _canRedo;
        private bool _canUndo;
        private bool _canForward;
        private bool _canBack;
        private bool _lockedForEditing = false;
        private SubscriptionToken _canUndoEventSubscriptionToken;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken;
        private SubscriptionToken _navigateFallBackEventSubscriptionToken;
        private string _searchText;
        private bool _isSearchShown;
        private ObservableCollection<BrowserModel> _searchResults;
        private BrowserModel _selectedSearchItem;

        public NavigationBarViewModel(TabIdentity tabIdentity,
            INavigationService navigationService,
            Prism.Events.IEventAggregator eventAggregator,
            IDeadfileRepository repository)
        {
            _tabIdentity = tabIdentity;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _repository = repository;
            _navigationService.PropertyChanged += NavigationPropertyChanged;
            LostFocusCommand = new DelegateCommand(SearchTextLostFocus);
            GotFocusCommand = new DelegateCommand(SearchTextGotFocus);
        }

        private void SearchTextGotFocus()
        {
            IsSearchShown = !string.IsNullOrEmpty(SearchText);
        }

        private void SearchTextLostFocus()
        {
            IsSearchShown = false;
        }

        private void NavigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanHome = _navigationService.CanGoBack;
            CanBack = _navigationService.CanGoBack;
            CanForward = _navigationService.CanGoForward;
        }

        public void Home()
        {
            while (_navigationService.CanGoBack) _navigationService.GoBack();
        }

        public bool CanHome
        {
            get { return !_lockedForEditing && _canHome; }
            set
            {
                if (value == _canHome) return;
                _canHome = value;
                NotifyOfPropertyChange(() => CanHome);
            }
        }

        public void Back()
        {
            _navigationService.GoBack();
        }

        public bool CanBack
        {
            get { return !_lockedForEditing && _canBack; }
            set
            {
                if (value == _canBack) return;
                _canBack = value;
                NotifyOfPropertyChange(() => CanBack);
            }
        }

        public void Forward()
        {
            _navigationService.GoForward();
        }

        public bool CanForward
        {
            get { return !_lockedForEditing && _canForward; }
            set
            {
                if (value == _canForward) return;
                _canForward = value;
                NotifyOfPropertyChange(() => CanForward);
            }
        }

        public void Undo()
        {
            Logger.Info("Event|UndoEvent|Send|{0}|{1}", _tabIdentity.TabIndex, UndoMessage.Undo);
            _eventAggregator.GetEvent<UndoEvent>().Publish(UndoMessage.Undo);
        }

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                if (value == _canUndo) return;
                _canUndo = value;
                NotifyOfPropertyChange(() => CanUndo);
            }
        }

        public void Redo()
        {
            _eventAggregator.GetEvent<UndoEvent>().Publish(UndoMessage.Redo);
        }

        public bool CanRedo
        {
            get { return _canRedo; }
            set
            {
                if (value == _canRedo) return;
                _canRedo = value;
                NotifyOfPropertyChange(() => CanRedo);
            }
        }


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                if (String.IsNullOrWhiteSpace(_searchText))
                {
                    IsSearchShown = false;
                    SearchResults = new ObservableCollection<BrowserModel>();
                    SelectedSearchItem = null;
                }
                else
                {
                    IsSearchShown = true;
                    var clients =
                        _repository.GetBrowserItems(new BrowserSettings
                        {
                            Mode = BrowserMode.Client,
                            FilterText = _searchText,
                            IncludeInactiveEnabled = true
                        });
                    var jobs =
                        _repository.GetBrowserItems(new BrowserSettings
                        {
                            Mode = BrowserMode.Job,
                            FilterText = _searchText,
                            IncludeInactiveEnabled = true
                        });
                    var invoices =
                        _repository.GetBrowserItems(new BrowserSettings
                        {
                            Mode = BrowserMode.Invoice,
                            FilterText = _searchText,
                            IncludeInactiveEnabled = true
                        });
                    SearchResults = new ObservableCollection<BrowserModel>(clients.Concat(jobs).Concat(invoices));
                    SelectedSearchItem = null;
                }
            }
        }

        public bool IsSearchShown
        {
            get { return _isSearchShown; }
            set
            {
                if (value == _isSearchShown) return;
                _isSearchShown = value;
                NotifyOfPropertyChange(() => IsSearchShown);
            }
        }

        public ObservableCollection<BrowserModel> SearchResults
        {
            get { return _searchResults; }
            set
            {
                if (value == _searchResults) return;
                _searchResults = value;
                NotifyOfPropertyChange(() => SearchResults);
            }
        }

        public BrowserModel SelectedSearchItem
        {
            get { return _selectedSearchItem; }
            set
            {
                if (value == _selectedSearchItem) return;
                _selectedSearchItem = value;
                NotifyOfPropertyChange(() => SelectedSearchItem);
                if (_selectedSearchItem != null)
                    _eventAggregator.GetEvent<SelectedItemEvent>().Publish(new SelectedItemPacket(_selectedSearchItem.ModelType, _selectedSearchItem.ParentId, _selectedSearchItem.Id));
            }
        }

        public ICommand LostFocusCommand { get; }
        public ICommand GotFocusCommand { get; }

        public void OnNavigatedTo(object parameters)
        {
            _canUndoEventSubscriptionToken = _eventAggregator.GetEvent<CanUndoEvent>().Subscribe(UpdateCanUndo);
            _lockedForEditingEventSubscriptionToken = _eventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(UpdateLockedForEditing);
            _navigateFallBackEventSubscriptionToken = _eventAggregator.GetEvent<NavigateFallBackEvent>().Subscribe(NavigateFallBack);
        }

        private void NavigateFallBack(NavigateFallBackMessage message)
        {
            _navigationService.FallBack();
        }

        private void UpdateLockedForEditing(LockedForEditingMessage lockedForEditingMessage)
        {
            _lockedForEditing = lockedForEditingMessage.IsLocked;
            if (!_lockedForEditing)
            {
                _navigationService.SetCurrentNavigationParameters(lockedForEditingMessage.NewParameters);
                CanUndo = false;
                CanRedo = false;
            }
            NotifyOfPropertyChange(nameof(CanHome));
            NotifyOfPropertyChange(nameof(CanBack));
            NotifyOfPropertyChange(nameof(CanForward));
        }

        private void UpdateCanUndo(CanUndoMessage canUndoMessage)
        {
            switch (canUndoMessage)
            {
                case CanUndoMessage.CanUndo:
                    CanUndo = true;
                    break;
                case CanUndoMessage.CanRedo:
                    CanRedo = true;
                    break;
                case CanUndoMessage.CannotUndo:
                    CanUndo = false;
                    break;
                case CanUndoMessage.CannotRedo:
                    CanRedo = false;
                    break;
            }
        }

        public void OnNavigatedFrom()
        {
            _eventAggregator.GetEvent<CanUndoEvent>().Unsubscribe(_canUndoEventSubscriptionToken);
            _eventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingEventSubscriptionToken);
            _eventAggregator.GetEvent<NavigateFallBackEvent>().Unsubscribe(_navigateFallBackEventSubscriptionToken);
            _canUndoEventSubscriptionToken = null;
            _lockedForEditingEventSubscriptionToken = null;
        }
    }
}
