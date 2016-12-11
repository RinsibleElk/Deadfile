using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Navigation
{
    public class NavigationBarViewModel : PropertyChangedBase, INavigationBarViewModel, INavigationAware
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly INavigationService _navigationService;
        private readonly Prism.Events.IEventAggregator _eventAggregator;
        private bool _canHome;
        private bool _canRedo;
        private bool _canUndo;
        private bool _canForward;
        private bool _canBack;
        private bool _lockedForEditing = false;
        private SubscriptionToken _canUndoEventSubscriptionToken;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken;
        private SubscriptionToken _navigateFallBackEventSubscriptionToken;

        public NavigationBarViewModel(TabIdentity tabIdentity,
            INavigationService navigationService,
            Prism.Events.IEventAggregator eventAggregator)
        {
            _tabIdentity = tabIdentity;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _navigationService.PropertyChanged += NavigationPropertyChanged;
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
            Logger.Info("Event,UndoEvent,Send,{0},{1}", _tabIdentity.TabIndex, UndoMessage.Undo);
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
