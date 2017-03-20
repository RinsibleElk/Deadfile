using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using EventAggregator = Caliburn.Micro.EventAggregator;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// These are parameterised by the job id of the parent job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class SimpleEditableItemViewModel<T> : ParameterisedViewModel<ClientAndJobNavigationKey>, ISimpleEditableItemViewModel<T> where T : JobChildModelBase, new()
    {
        private readonly IDeadfileDialogCoordinator _dialogCoordinator;
        private readonly IEventAggregator _eventAggregator;
        private readonly DelegateCommand _editCommand;
        private readonly DelegateCommand _discardCommand;
        private readonly DelegateCommand _deleteCommand;
        private readonly DelegateCommand _saveCommand;
        private readonly IDeadfileDispatcherTimer _maintainSelectionTimer;
        private List<string> _errors;

        public SimpleEditableItemViewModel(IDeadfileDispatcherTimerService timerService,
            IDeadfileDialogCoordinator dialogCoordinator,
            IEventAggregator eventAggregator)
        {
            _maintainSelectionTimer = timerService.CreateTimer(TimeSpan.FromMilliseconds(10), SetSelectedIndex);
            _dialogCoordinator = dialogCoordinator;
            _eventAggregator = eventAggregator;
            _editCommand = new DelegateCommand(StartEditing);
            _discardCommand = new DelegateCommand(DiscardEdits);
            _deleteCommand = new DelegateCommand(DeleteItem, () => CanDeleteItem);
            _saveCommand = new DelegateCommand(PerformSaveAction);
        }

        private void DiscardEdits()
        {
            while (UndoTracker.CanUndo)
                UndoTracker.Undo();
            Editable = false;
        }

        private void StartEditing()
        {
            Editable = true;
        }

        private void PerformSaveAction()
        {
            PerformSave();
            Editable = false;
        }

        private async void DeleteItem()
        {
            var result = await _dialogCoordinator.ConfirmDeleteAsync(this, "Confirm Deletion", "Are you sure? This action is permanent.");
            // Open a dialog to ask the user if they are sure.
            if (result == MessageDialogResult.Affirmative)
            {
                PerformDelete();

                // Refresh the observable collection.
                Populate();
            }
        }

        protected abstract void PerformDelete();

        private bool _canDeleteItem = false;
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

        protected abstract void PerformSave();

        private bool _editable;
        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (value == _editable) return;
                _editable = value;
                NotifyOfPropertyChange(() => Editable);

                if (_editable)
                {
                    UndoTrackerActivatable.ActivateUndoTracker(UndoTracker, SelectedItem);
                    _selectedItem.ErrorsChanged += SelectedItemErrorsChanged;
                    _selectedItem.RefreshAllErrors();
                }
                else
                {
                    UndoTrackerActivatable.DeactivateUndoTracker();
                    // Deliberately do this after deactivation so that the deactivation takes care of notifying the
                    // browser of CanUndo/CanRedo changes.
                    _selectedItem.ErrorsChanged -= SelectedItemErrorsChanged;
                    _selectedItem.ClearAllErrors();

                    // Reload the data.
                    Populate();
                }

                // Maintain the selection.
                if (value) _editingSelectedIndex = _selectedIndex;
                else _editingSelectedIndex = null;

                // Allow deletion.
                CanDeleteItem = (SelectedItem != null) && !Editable;

                // Only fire when it changes.
                _eventAggregator.GetEvent<LockedForEditingEvent>()
                    .Publish(new LockedForEditingMessage() {IsLocked = _editable});
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

        private void Populate()
        {
            // Populate the table.
            // We always add one more, to represent the user wanting to add a new one.
            var value = new T() { JobId = _jobId, ClientId = _clientId };
            Items = new ObservableCollection<T>(GetModelsForJobId(_jobId, _filter)) {value};
            SelectedItem = value;
        }

        private string _filter = null;
        /// <summary>
        /// The user's filter for the table of items.
        /// </summary>
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) return;
                _filter = value;
                NotifyOfPropertyChange(() => Filter);

                Populate();
            }
        }

        public ICommand EditCommand => _editCommand;
        public ICommand DiscardCommand => _discardCommand;
        public ICommand DeleteCommand => _deleteCommand;
        public ICommand SaveCommand => _saveCommand;

        /// <summary>
        /// The type-specific undo tracker.
        /// </summary>
        public UndoTracker<T> UndoTracker { get; } = new UndoTracker<T>();

        /// <summary>
        /// The table of items.
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

        private T _selectedItem;
        private int _jobId = ModelBase.NewModelId;
        private int _clientId = ModelBase.NewModelId;
        private ObservableCollection<T> _items;

        /// <summary>
        /// The item selected by the user.
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange();

                CanDeleteItem = (SelectedItem != null) && !Editable;
            }
        }

        /// <summary>
        /// Provides access to the parent job to perform undos.
        /// </summary>
        IUndoTracker ISimpleEditableItemViewModel.UndoTracker => UndoTracker;

        /// <summary>
        /// The user is navigating away.
        /// </summary>
        /// <param name="key"></param>
        public override void OnNavigatedTo(ClientAndJobNavigationKey key)
        {
            base.OnNavigatedTo(key);

            // Hold on to the parent job.
            _jobId = key.JobId;
            _clientId = key.ClientId;

            // Populate.
            Populate();

            // Cheese the filter. We don't want to load the models twice, that's wasteful.
            _filter = null;
            NotifyOfPropertyChange(nameof(Filter));
        }

        /// <summary>
        /// Unsubscribe from events.
        /// </summary>
        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            _jobId = ModelBase.NewModelId;
            _clientId = ModelBase.NewModelId;

            // Bin the table.
            SelectedItem = new T() {JobId = _jobId, ClientId = _clientId};
            Items = new ObservableCollection<T> {SelectedItem};

            // Same again.
            _filter = null;
            NotifyOfPropertyChange(nameof(Filter));
        }

        /// <summary>
        /// Ask the sub class to get all the models for this job, given a user-entered filter (which may be null or empty).
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetModelsForJobId(int jobId, string filter);

        protected IUndoTrackerActivatable UndoTrackerActivatable = null;

        /// <summary>
        /// Register an undo tracker activatable.
        /// </summary>
        /// <param name="undoTrackerActivatable"></param>
        public void RegisterUndoTrackerActivatable(IUndoTrackerActivatable undoTrackerActivatable)
        {
            UndoTrackerActivatable = undoTrackerActivatable;
        }

        private int? _editingSelectedIndex = null;
        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);

                if ((_editingSelectedIndex != null) && (_selectedIndex != _editingSelectedIndex.Value))
                {
                    _maintainSelectionTimer.Start();
                }
            }
        }

        private bool _parentEditable;
        public bool ParentEditable
        {
            get { return _parentEditable; }
            set
            {
                if (value == _parentEditable) return;
                _parentEditable = value;
                NotifyOfPropertyChange(() => ParentEditable);

                // Maintain the selection.
                if (value) _editingSelectedIndex = _selectedIndex;
                else _editingSelectedIndex = null;
            }
        }

        private void SetSelectedIndex()
        {
            if (_editingSelectedIndex != null)
            {
                SelectedIndex = _editingSelectedIndex.Value;
            }
            _maintainSelectionTimer.Stop();
        }
    }
}
