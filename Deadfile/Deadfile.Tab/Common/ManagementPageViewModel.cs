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
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Tab.Navigation;
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
        private readonly DelegateCommand _saveCommand;

        /// <summary>
        /// Requires an event aggregator to communicate the display name to the tab.
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="allowAdds"></param>
        protected ManagementPageViewModel(IEventAggregator eventAggregator, bool allowAdds)
        {
            EventAggregator = eventAggregator;
            _allowAdds = allowAdds;
            _editCommand = new DelegateCommand(StartEditing);
            _discardCommand = new DelegateCommand(DiscardEdits);
            _saveCommand = new DelegateCommand(PerformSaveAction);
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
                    }

                    EditingStatusChanged(_editable);

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
                SelectedItem = new T();
                Items = new ObservableCollection<T>(GetModels(Filter));
                Items.Add(SelectedItem);
            }
        }

        public ICommand EditCommand { get { return _editCommand; } }
        public ICommand DiscardCommand { get { return _discardCommand; } }
        public ICommand SaveCommand { get { return _saveCommand; } }

        public UndoTracker<T> UndoTracker
        {
            get { return _undoTracker; }
        }

        protected IUndoTrackerActivatable UndoTrackerActivatable = null;
        public void RegisterUndoTrackerActivatable(IUndoTrackerActivatable undoTrackerActivatable)
        {
            UndoTrackerActivatable = undoTrackerActivatable;
        }

        IUndoTracker ISimpleEditableItemViewModel.UndoTracker
        {
            get { return UndoTracker; }
        }
    }
}
