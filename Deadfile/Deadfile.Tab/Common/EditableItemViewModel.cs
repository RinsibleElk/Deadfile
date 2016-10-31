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
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    public abstract class EditableItemViewModel<K, T> : ParameterisedViewModel<K>, IEditableItemViewModel<T> where T : ModelBase, new()
    {
        protected readonly IEventAggregator EventAggregator;
        private readonly UndoTracker<T> _undoTracker = new UndoTracker<T>();
        private SubscriptionToken _saveSubscriptionToken = null;
        public EditableItemViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public abstract T GetModel(K key);

        private void HandleUndo(UndoMessage message)
        {
            if (message == UndoMessage.Undo)
                _undoTracker.Undo();
            else
                _undoTracker.Redo();
        }

        private T _selectedItem = new T();
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value.Id == _selectedItem.Id) return;
                _selectedItem = value;
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
                CanSave = _errors.Count == 0;
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
                        _saveSubscriptionToken = EventAggregator.GetEvent<SaveEvent>().Subscribe(PerformSave);
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
                case nameof(_undoTracker.CanUndo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_undoTracker.CanUndo ? CanUndoMessage.CanUndo : CanUndoMessage.CannotUndo);
                    break;
                case nameof(_undoTracker.CanRedo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_undoTracker.CanRedo ? CanUndoMessage.CanRedo : CanUndoMessage.CannotRedo);
                    break;
            }
        }
        public abstract void PerformSave();
        private void PerformSave(SaveMessage message)
        {
            PerformSave();
        }

        private void HandleEditAction(EditActionMessage editAction)
        {
            if (editAction == EditActionMessage.Add)
                SelectedItem = new T();

            // This fires an event to lock down navigation.
            Editable = editAction != EditActionMessage.EndEditing;
        }

        public override void OnNavigatedTo(K parameters)
        {
            base.OnNavigatedTo(parameters);

            SelectedItem = GetModel(parameters);
            _handleEditActionSubscriptionToken = EventAggregator.GetEvent<EditActionEvent>().Subscribe(HandleEditAction);
            _handleUndoSubcriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(HandleUndo);
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            EventAggregator.GetEvent<EditActionEvent>().Unsubscribe(_handleEditActionSubscriptionToken);
            EventAggregator.GetEvent<UndoEvent>().Unsubscribe(_handleUndoSubcriptionToken);
        }
    }
}
