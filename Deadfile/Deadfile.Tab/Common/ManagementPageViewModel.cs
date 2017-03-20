using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Tab.Events;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Tab.Navigation;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Base class for management pages. Sets the display name for you.
    /// </summary>
    public abstract class ManagementPageViewModel<T> : Screen, IManagementViewModel<T> where T : ModelBase, new()
    {
        private readonly IDeadfileDialogCoordinator _dialogCoordinator;
        protected readonly IEventAggregator EventAggregator;
        private readonly bool _allowAdds;
        private ObservableCollection<T> _items;
        private bool _editable;
        private T _selectedItem = new T();
        private List<string> _errors;
        private readonly UndoTracker<T> _undoTracker = new UndoTracker<T>();
        private SubscriptionToken _undoEventSubscriptionToken = null;
        private string _filter;
        private readonly DelegateCommand _editCommand;
        private readonly DelegateCommand _discardCommand;
        private readonly DelegateCommand _deleteCommand;
        private readonly DelegateCommand _saveCommand;

        /// <summary>
        /// Requires an event aggregator to communicate the display name to the tab.
        /// </summary>
        /// <param name="dialogCoordinator"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="allowAdds"></param>
        protected ManagementPageViewModel(IDeadfileDialogCoordinator dialogCoordinator, IEventAggregator eventAggregator, bool allowAdds)
        {
            _dialogCoordinator = dialogCoordinator;
            EventAggregator = eventAggregator;
            _allowAdds = allowAdds;
            _editCommand = new DelegateCommand(StartEditing);
            _discardCommand = new DelegateCommand(DiscardEdits);
            _deleteCommand = new DelegateCommand(DeleteItem, () => CanDeleteItem);
            _saveCommand = new DelegateCommand(PerformSaveAction, () => CanSaveItem);
        }

        private bool _canSaveItem = false;
        private bool CanSaveItem
        {
            get { return _canSaveItem; }
            set
            {
                if (_canSaveItem != value)
                {
                    _canSaveItem = value;
                    _saveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private async void DeleteItem()
        {
            var result = await _dialogCoordinator.ConfirmDeleteAsync(this, "Confirm Deletion", "Are you sure? This action is permanent.");
            // Open a dialog to ask the user if they are sure.
            if (result == MessageDialogResult.Affirmative)
            {
                PerformDelete();

                // Refresh the observable collection.
                RefreshModels();
            }
        }

        protected abstract void PerformDelete();

        private bool CanDeleteItem
        {
            get { return _canDeleteItem; }
            set
            {
                if (_canDeleteItem != value)
                {
                    _canDeleteItem = value;
                    _deleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void PerformSaveAction()
        {
            PerformSave();
            Editable = false;
        }

        protected abstract void PerformSave();

        private void DiscardEdits()
        {
            while (_undoTracker.CanUndo)
                _undoTracker.Undo();
            Editable = false;
        }

        private void StartEditing()
        {
            Editable = true;
        }

        /// <summary>
        /// Handle an <see cref="UndoMessage"/> from the <see cref="NavigationBarViewModel"/>.
        /// </summary>
        /// <param name="message"></param>
        private void HandleUndo(UndoMessage message)
        {
            if (message == UndoMessage.Undo)
                _undoTracker.Undo();
            else
                _undoTracker.Redo();
        }

        protected void RefreshModels()
        {
            // add a placeholder for an added item
            if (_allowAdds)
            {
                SelectedItem = new T();
                Items = new ObservableCollection<T>(GetModels(Filter));
                Items.Add(SelectedItem);
            }
            else
            {
                Items = new ObservableCollection<T>(GetModels(Filter));
                if (Items.Count == 0)
                    SelectedItem = null;
                else
                    SelectedItem = Items[0];
            }
        }

        /// <summary>
        /// For management pages, the base class takes care of the display name for you.
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(object parameters)
        {
            // convert camel case to spaces
            DisplayNameBroadcaster.BroadcastDisplayName(EventAggregator, Experience);

            // Observe UndoMessages received from the NavigationBarViewModel.
            _undoEventSubscriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(HandleUndo);

            RefreshModels();
        }

        /// <summary>
        /// Can be overridden to perform event handling.
        /// </summary>
        public virtual void OnNavigatedFrom()
        {
            SelectedItem = new T();
            Items = new ObservableCollection<T>();

            // Stop observing UndoMessages received from the NavigationBarViewModel.
            EventAggregator.GetEvent<UndoEvent>().Unsubscribe(_undoEventSubscriptionToken);
        }

        public abstract Experience Experience { get; }
        public bool ShowActionsPad { get; } = false;
        public void CompleteNavigation()
        {
        }

        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (_editable != value)
                {
                    _editable = value;
                    NotifyOfPropertyChange(() => Editable);
                    if (_editable)
                    {
                        _undoTracker.Activate(_selectedItem);
                        _undoTracker.PropertyChanged += UndoTrackerPropertyChanged;
                        _selectedItem.ErrorsChanged += SelectedItemErrorsChanged;
                        _selectedItem.RefreshAllErrors();
                    }
                    else
                    {
                        _undoTracker.Deactivate();
                        // Deliberately do this after deactivation so that the deactivation takes care of notifying the
                        // browser of CanUndo/CanRedo changes.
                        _undoTracker.PropertyChanged -= UndoTrackerPropertyChanged;
                        _selectedItem.ErrorsChanged -= SelectedItemErrorsChanged;
                        _selectedItem.ClearAllErrors();

                        // Refresh the observable collection.
                        RefreshModels();
                    }

                    EditingStatusChanged(_editable);

                    // Allow deletion.
                    CanDeleteItem = _allowAdds && (SelectedItem != null) && !Editable;

                    // Allow save.
                    CanSaveItem = (SelectedItem != null) && Editable && !SelectedItem.HasErrors;

                    // Only fire when it changes.
                    EventAggregator.GetEvent<LockedForEditingEvent>()
                        .Publish(new LockedForEditingMessage() {IsLocked = _editable});
                }
            }
        }

        private void UndoTrackerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_undoTracker.CanUndo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_undoTracker.CanUndo ? CanUndoMessage.CanUndo : CanUndoMessage.CannotUndo);
                    break;
                case nameof(_undoTracker.CanRedo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_undoTracker.CanRedo ? CanUndoMessage.CanRedo : CanUndoMessage.CannotRedo);
                    break;
            }
        }

        public abstract void EditingStatusChanged(bool editable);
        private void SelectedItemErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            Errors = FlattenErrors();
            CanSaveItem = (SelectedItem != null) && Editable && !SelectedItem.HasErrors;
        }

        private List<string> FlattenErrors()
        {
            List<string> errors = new List<string>();
            Dictionary<string, List<string>> allErrors = SelectedItem.GetAllErrors();
            foreach (string propertyName in allErrors.Keys)
            {
                foreach (var errorString in allErrors[propertyName])
                {
                    errors.Add(propertyName + ": " + errorString);
                }
            }
            return errors;
        }

        /// <summary>
        /// The collection of items under edit.
        /// </summary>
        public ObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;

                // Is this too lame of a user experience????
                if (Editable)
                    DiscardEdits();

                if (value == null) _selectedItem = new T();
                else _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);

                // Allow deletion.
                CanDeleteItem = _allowAdds && (SelectedItem != null) && !Editable;
            }
        }

        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                if (Equals(value, _errors)) return;
                _errors = value;
                NotifyOfPropertyChange(() => Errors);
            }
        }

        protected abstract IEnumerable<T> GetModels(string filter);

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) return;
                _filter = value;
                NotifyOfPropertyChange(() => Filter);

                RefreshModels();
            }
        }

        public ICommand EditCommand => _editCommand;
        public ICommand DiscardCommand => _discardCommand;
        public ICommand DeleteCommand => _deleteCommand;
        public ICommand SaveCommand => _saveCommand;

        public UndoTracker<T> UndoTracker
        {
            get { return _undoTracker; }
        }

        protected IUndoTrackerActivatable UndoTrackerActivatable = null;
        private bool _canDeleteItem;

        public void RegisterUndoTrackerActivatable(IUndoTrackerActivatable undoTrackerActivatable)
        {
            UndoTrackerActivatable = undoTrackerActivatable;
        }

        public int SelectedIndex { get; set; }
        public bool ParentEditable { get; set; }

        IUndoTracker ISimpleEditableItemViewModel.UndoTracker
        {
            get { return UndoTracker; }
        }
    }
}
