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
            eventAggregator.GetEvent<CanUndoEvent>().Subscribe(UpdateCanUndo);
            eventAggregator.GetEvent<CanRedoEvent>().Subscribe(UpdateCanRedo);
            eventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(UpdateLockedForEditing);
        }

        private void UpdateLockedForEditing(bool lockedForEditing)
        {
            _lockedForEditing = lockedForEditing;
            _homeCommand.RaiseCanExecuteChanged();
            _backCommand.RaiseCanExecuteChanged();
            _forwardCommand.RaiseCanExecuteChanged();
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
    }
}
