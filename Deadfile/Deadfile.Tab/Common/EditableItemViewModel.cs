using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Services;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Events;
using Deadfile.Tab.Navigation;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    public abstract class EditableItemViewModel<K, T> : ParameterisedViewModel<K>, IEditableItemViewModel<T> where T : ModelBase, new()
    {
        protected readonly IEventAggregator EventAggregator;

        // This is the main undo tracker for the object under management. However, it may not be the active one responding to undo events from the nav-bar.
        private readonly UndoTracker<T> _undoTracker = new UndoTracker<T>();

        // This is the "active" undo tracker. For the Jobs page, this could represent a billable item.
        private IUndoTracker _activeUndoTracker = null;

        private SubscriptionToken _saveSubscriptionToken = null;
        public EditableItemViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        /// <summary>
        /// Ask the specific implementation to look up a value in the database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract T GetModel(K key);

        /// <summary>
        /// Ask the specific implementation whether to edit on navigate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract bool ShouldEditOnNavigate(K key);

        /// <summary>
        /// Respond to an event from the <see cref="NavigationBarViewModel"/>. This should only happen if an Undo or Redo is possible (marshalled to the
        /// <see cref="NavigationBarViewModel"/> via the <see cref="CanUndoEvent"/>).
        /// </summary>
        /// <param name="message"></param>
        private void HandleUndo(UndoMessage message)
        {
            if (message == UndoMessage.Undo)
                _activeUndoTracker.Undo();
            else
                _activeUndoTracker.Redo();
        }

        private T _selectedItem = new T() {Id = ModelBase.NewModelId};
        /// <summary>
        /// The item selected in the <see cref="BrowserPaneViewModel"/>.
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                var valueToSet = value ?? new T() {Id = ModelBase.NewModelId};
                if (valueToSet.Id == _selectedItem.Id) return;
                _selectedItem = valueToSet;
                NotifyOfPropertyChange(() => SelectedItem);
                Editable = false;
                Errors = new List<string>();
                CanEdit = _selectedItem.Id != ModelBase.NewModelId;
            }
        }

        private bool _canEdit = false;
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                if (_canEdit == value) return;
                _canEdit = value;
                NotifyOfPropertyChange(() => CanEdit);
                EventAggregator.GetEvent<CanEditEvent>().Publish(_canEdit ? CanEditMessage.CanEdit : CanEditMessage.CannotEdit);
            }
        }

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                if (_errors == value) return;
                _errors = value;
                NotifyOfPropertyChange(() => Errors);
                CanSave = Editable && _errors.Count == 0;
            }
        }

        private bool _canSave = false;
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (_canSave == value) return;
                _canSave = value;
                NotifyOfPropertyChange(() => CanSave);
                EventAggregator.GetEvent<CanSaveEvent>().Publish(_canSave ? CanSaveMessage.CanSave : CanSaveMessage.CannotSave);
            }
        }

        private bool _editable = false;
        private SubscriptionToken _handleEditActionSubscriptionToken;
        private SubscriptionToken _handleUndoSubcriptionToken;
        private SubscriptionToken _discardChangesSubscriptionToken;

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
                        ActivateUndoTracker(_undoTracker, _selectedItem);
                        _selectedItem.ErrorsChanged += SelectedItemErrorsChanged;
                        _saveSubscriptionToken = EventAggregator.GetEvent<SaveEvent>().Subscribe(PerformSave);
                        _discardChangesSubscriptionToken = EventAggregator.GetEvent<DiscardChangesEvent>().Subscribe(DiscardChangesAction);
                        _selectedItem.RefreshAllErrors();
                    }
                    else
                    {
                        DeactivateUndoTracker();
                        // Deliberately do this after deactivation so that the deactivation takes care of notifying the
                        // browser of CanUndo/CanRedo changes.
                        EventAggregator.GetEvent<SaveEvent>().Unsubscribe(_saveSubscriptionToken);
                        EventAggregator.GetEvent<DiscardChangesEvent>().Unsubscribe(_discardChangesSubscriptionToken);
                        _saveSubscriptionToken = null;
                        _selectedItem.ErrorsChanged -= SelectedItemErrorsChanged;
                        _selectedItem.ClearAllErrors();
                    }

                    EditingStatusChanged(_editable);

                    // Only fire when it changes.
                    EventAggregator.GetEvent<LockedForEditingEvent>().Publish(_editable ? LockedForEditingMessage.Locked : LockedForEditingMessage.Unlocked);
                }
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

        private void UndoTrackerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                // We use the name of the main undo tracker's properties here, however it may not be the one that is firing the event..
                case nameof(_undoTracker.CanUndo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_activeUndoTracker.CanUndo ? CanUndoMessage.CanUndo : CanUndoMessage.CannotUndo);
                    break;
                case nameof(_undoTracker.CanRedo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_activeUndoTracker.CanRedo ? CanUndoMessage.CanRedo : CanUndoMessage.CannotRedo);
                    break;
            }
        }

        public abstract void PerformSave();
        private void PerformSave(SaveMessage message)
        {
            PerformSave();
        }

        private void DiscardChangesAction(DiscardChangesMessage discardChangesMessage)
        {
            if (discardChangesMessage == DiscardChangesMessage.Discard)
                while (_activeUndoTracker.CanUndo) _activeUndoTracker.Undo();
        }

        private void HandleEditAction(EditActionMessage editAction)
        {
            if (editAction == EditActionMessage.Add)
                SelectedItem = new T();

            // This fires an event to lock down navigation.
            Editable = editAction != EditActionMessage.EndEditing;
        }

        private bool _pendingEdit = false;
        public override void OnNavigatedTo(K parameters)
        {
            base.OnNavigatedTo(parameters);

            // Get the model.
            SelectedItem = GetModel(parameters);
            _handleEditActionSubscriptionToken = EventAggregator.GetEvent<EditActionEvent>().Subscribe(HandleEditAction);
            _handleUndoSubcriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(HandleUndo);

            // Important. We cannot just immediately start editing - this is because the actions pad will not be loaded yet.
            _pendingEdit = ShouldEditOnNavigate(parameters);
        }

        /// <summary>
        /// Finalise navigation by enabling editing if requested.
        /// </summary>
        public void CompleteNavigation()
        {
            if (_pendingEdit)
            {
                _pendingEdit = false;
                Editable = true;
            }
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            EventAggregator.GetEvent<EditActionEvent>().Unsubscribe(_handleEditActionSubscriptionToken);
            EventAggregator.GetEvent<UndoEvent>().Unsubscribe(_handleUndoSubcriptionToken);
        }

        /// <summary>
        /// Can be called by a derived class who manages sub-objects that need to manage the current active undo tracker, responding to Undo
        /// events from the navigation bar.
        /// </summary>
        /// <remarks>
        /// The motivation is for the Jobs page, where there are related editable items that might be managed on the same page.
        /// </remarks>
        /// <typeparam name="TObjectUnderEdit"></typeparam>
        /// <param name="newActiveUndoTracker"></param>
        /// <param name="objectUnderEdit"></param>
        public void ActivateUndoTracker<TObjectUnderEdit>(UndoTracker<TObjectUnderEdit> newActiveUndoTracker, TObjectUnderEdit objectUnderEdit) where TObjectUnderEdit : ModelBase
        {
            if (_activeUndoTracker != null)
                throw new ApplicationException("There is already an active object handling undo and redo events");
            newActiveUndoTracker.Activate(objectUnderEdit);
            _activeUndoTracker = newActiveUndoTracker;
            _activeUndoTracker.PropertyChanged += UndoTrackerPropertyChanged;
        }

        /// <summary>
        /// Can be called by a derived class who manages sub-objects that need to manage the current active undo tracker, responding to Undo
        /// events from the navigation bar.
        /// </summary>
        /// <remarks>
        /// The motivation is for the Jobs page, where there are related editable items that might be managed on the same page.
        /// </remarks>
        public void DeactivateUndoTracker()
        {
            if (_activeUndoTracker == null)
                throw new ApplicationException("There is no active object handling undo and redo events to deactivate");
            _activeUndoTracker.PropertyChanged -= UndoTrackerPropertyChanged;
            _activeUndoTracker.Deactivate();
            _activeUndoTracker = null;
        }
    }
}
