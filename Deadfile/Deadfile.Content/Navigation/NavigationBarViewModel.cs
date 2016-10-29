using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Deadfile.Content.Navigation
{
    public class NavigationBarViewModel : BindableBase, INavigationBarViewModel
    {
        private IRegionNavigationJournal _navigationJournal = null;
        private readonly IEventAggregator _eventAggregator;
        private readonly DelegateCommand _backCommand;
        private readonly DelegateCommand _homeCommand;
        private readonly DelegateCommand _forwardCommand;
        private readonly DelegateCommand _undoCommand;
        private readonly DelegateCommand _redoCommand;
        private bool _lockedForEditing = false;
        private bool _canUndo = false;
        private bool _canRedo = false;
        public NavigationBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(Navigated);
            _backCommand = new DelegateCommand(NavigateBack, CanNavigateBack);
            _homeCommand = new DelegateCommand(NavigateHome, CanNavigateBack);
            _forwardCommand=new DelegateCommand(NavigateForward, CanNavigateForward);
            _undoCommand = new DelegateCommand(PerformUndo, CanUndo);
            _redoCommand = new DelegateCommand(PerformRedo, CanRedo);
        }

        private void DiscardChangesAction()
        {
            while (CanUndo()) PerformUndo();
        }

        private void UpdateLockedForEditing(bool lockedForEditing)
        {
            _lockedForEditing = lockedForEditing;
            _homeCommand.RaiseCanExecuteChanged();
            _backCommand.RaiseCanExecuteChanged();
            _forwardCommand.RaiseCanExecuteChanged();
            _undoCommand.RaiseCanExecuteChanged();
            _redoCommand.RaiseCanExecuteChanged();
        }

        private void UpdateCanUndo(bool canUndo)
        {
            _canUndo = canUndo;
            _undoCommand.RaiseCanExecuteChanged();
        }

        private void UpdateCanRedo(bool canRedo)
        {
            _canRedo = canRedo;
            _redoCommand.RaiseCanExecuteChanged();
        }

        private void Navigated(IRegionNavigationJournal navigationJournal)
        {
            this._navigationJournal = navigationJournal;
            _backCommand.RaiseCanExecuteChanged();
            _homeCommand.RaiseCanExecuteChanged();
            _forwardCommand.RaiseCanExecuteChanged();
        }
        public ICommand BackCommand { get { return _backCommand; } }

        public ICommand HomeCommand { get { return _homeCommand; } }

        public ICommand ForwardCommand { get { return _forwardCommand; } }

        public ICommand UndoCommand { get { return _undoCommand; } }

        public ICommand RedoCommand { get { return _redoCommand; } }

        private void NavigateBack()
        {
            if (_navigationJournal != null)
            {
                _navigationJournal.GoBack();
                _forwardCommand.RaiseCanExecuteChanged();
            }
        }

        private void NavigateForward()
        {
            if (_navigationJournal != null)
            {
                _navigationJournal.GoForward();
                _backCommand.RaiseCanExecuteChanged();
                _forwardCommand.RaiseCanExecuteChanged();
                _homeCommand.RaiseCanExecuteChanged();
            }
        }

        private void NavigateHome()
        {
            if (_navigationJournal != null)
            {
                while (_navigationJournal.CanGoBack)
                    _navigationJournal.GoBack();
                _forwardCommand.RaiseCanExecuteChanged();
            }
        }
        private bool CanNavigateBack()
        {
            return (!_lockedForEditing) && (_navigationJournal != null) && (_navigationJournal.CanGoBack);
        }

        private bool CanNavigateForward()
        {
            return (!_lockedForEditing) && (_navigationJournal != null) && (_navigationJournal.CanGoForward);
        }

        private void PerformUndo()
        {
            _eventAggregator.GetEvent<UndoEvent>().Publish();
        }

        private bool CanUndo()
        {
            return _canUndo;
        }

        private void PerformRedo()
        {
            _eventAggregator.GetEvent<RedoEvent>().Publish();
        }

        private bool CanRedo()
        {
            return _canRedo;
        }

        private SubscriptionToken _canUndoEventSubscriptionToken = null;
        private SubscriptionToken _canRedoEventSubscriptionToken = null;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken = null;
        private SubscriptionToken _discardChangesEventSubscriptionToken = null;
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
                        _canUndoEventSubscriptionToken = _eventAggregator.GetEvent<CanUndoEvent>().Subscribe(UpdateCanUndo);
                        _canRedoEventSubscriptionToken = _eventAggregator.GetEvent<CanRedoEvent>().Subscribe(UpdateCanRedo);
                        _lockedForEditingEventSubscriptionToken = _eventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(UpdateLockedForEditing);
                        _discardChangesEventSubscriptionToken = _eventAggregator.GetEvent<DiscardChangesEvent>().Subscribe(DiscardChangesAction);
                    }
                    else
                    {
                        _eventAggregator.GetEvent<CanUndoEvent>().Unsubscribe(_canUndoEventSubscriptionToken);
                        _eventAggregator.GetEvent<CanRedoEvent>().Unsubscribe(_canRedoEventSubscriptionToken);
                        _eventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingEventSubscriptionToken);
                        _eventAggregator.GetEvent<DiscardChangesEvent>().Unsubscribe(_discardChangesEventSubscriptionToken);
                        _canUndoEventSubscriptionToken = null;
                        _canRedoEventSubscriptionToken = null;
                        _lockedForEditingEventSubscriptionToken = null;
                        _discardChangesEventSubscriptionToken = null;
                    }
                }
            }
        }
        public event EventHandler IsActiveChanged;
    }
}
