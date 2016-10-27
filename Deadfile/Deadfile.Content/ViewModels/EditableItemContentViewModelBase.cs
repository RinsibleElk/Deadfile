using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Undo;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public abstract class EditableItemContentViewModelBase<T>
        : ParameterisedContentViewModelBase<int>, IEditableItemViewModel<T>
        where T : ModelBase, new()
    {
        private readonly UndoTracker<T> _undoTracker = new UndoTracker<T>();
        private SubscriptionToken _editItemSubscriptionToken = null;
        private SubscriptionToken _navigateToSelectedItemSubscriptionToken = null;
        private SubscriptionToken _undoSubscriptionToken = null;
        private SubscriptionToken _redoSubscriptionToken = null;
        private SubscriptionToken _saveSubscriptionToken = null;

        public EditableItemContentViewModelBase(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            INavigationParameterMapper navigationParameterMapper)
            : base(eventAggregator, navigationService, navigationParameterMapper)
        {
        }

        public abstract void PerformSave();

        private bool _editable = false;
        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (SetProperty(ref _editable, value))
                {
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
                        EventAggregator.GetEvent<SaveEvent>().Unsubscribe(_saveSubscriptionToken);
                        _saveSubscriptionToken = null;
                        _undoTracker.Deactivate();
                        // Deliberately do this after deactivation so that the deactivation takes care of notifying the
                        // browser of CanUndo/CanRedo changes.
                        _undoTracker.PropertyChanged -= UndoTrackerPropertyChanged;
                        _selectedItem.ErrorsChanged -= SelectedItemErrorsChanged;
                        _selectedItem.ClearAllErrors();
                    }

                    // Only fire when it changes.
                    EventAggregator.GetEvent<LockedForEditingEvent>().Publish(_editable);
                }
            }
        }

        public abstract T GetModelById(int id);

        private void EditItemAction(EditAction editAction)
        {
            if (editAction == EditAction.Add)
                SelectedItem = new T();

            // This fires an event to lock down navigation.
            Editable = editAction != EditAction.EndEditing;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext, int selectedId)
        {
            try
            {
                if (selectedId == ModelBase.NewModelId)
                    SelectedItem = new T();
                else
                    SelectedItem = GetModelById(selectedId);
            }
            catch (Exception e)
            {
                SelectedItem = new T();
            }

            // subscribe to messages from the browser pane
            _navigateToSelectedItemSubscriptionToken =
                EventAggregator.GetEvent<SelectedItemEvent>().Subscribe(NavigateToExperience);
            _undoSubscriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(PerformUndo);
            _redoSubscriptionToken = EventAggregator.GetEvent<RedoEvent>().Subscribe(PerformRedo);

            // subscribe to messages from the actions pane
            _editItemSubscriptionToken = EventAggregator.GetEvent<EditItemEvent>().Subscribe(EditItemAction);
        }

        private void PerformUndo()
        {
            _undoTracker.Undo();
        }

        private void PerformRedo()
        {
            _undoTracker.Redo();
        }

        // selected item is not nullable
        private T _selectedItem = new T();

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                // Careful - we don't want to use ReferenceEquals here!!!
                if (_selectedItem.Id != value.Id)
                {
                    if (SetProperty(ref _selectedItem, value))
                    {
                        // On Navigation we are always read-only. This is really error handling though, as there should be other
                        // mechanisms in place.
                        Editable = false;
                        Errors = new List<string>();
                        CanEdit = _selectedItem.Id != ModelBase.NewModelId;
                    }
                }
            }
        }

        private bool _canEdit = false;
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                if (SetProperty(ref _canEdit, value))
                    EventAggregator.GetEvent<CanEditEvent>().Publish(_canEdit);
            }
        }


        private bool _canSave = true;
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (SetProperty(ref _canSave, value))
                    EventAggregator.GetEvent<CanSaveEvent>().Publish(_canSave);
            }
        }

        private List<string> _errors;
        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                if (SetProperty(ref _errors, value))
                {
                    CanSave = _errors.Count == 0;
                }
            }
        }

        private void NavigateToExperience(SelectedItemPacket packet)
        {
            switch (packet.Type)
            {
                case BrowserModelType.Client:
                    NavigationService.NavigateTo(Experience.Clients, packet.Id);
                    break;
                case BrowserModelType.Job:
                    NavigationService.NavigateTo(Experience.Jobs, packet.Id);
                    break;
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

        private void UndoTrackerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_undoTracker.CanUndo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_undoTracker.CanUndo);
                    break;
                case nameof(_undoTracker.CanRedo):
                    EventAggregator.GetEvent<CanRedoEvent>().Publish(_undoTracker.CanRedo);
                    break;
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Unsubscribe to messages from the browser pane.
            EventAggregator.GetEvent<SelectedItemEvent>().Unsubscribe(_navigateToSelectedItemSubscriptionToken);
            _navigateToSelectedItemSubscriptionToken = null;
            EventAggregator.GetEvent<UndoEvent>().Unsubscribe(_undoSubscriptionToken);
            _undoSubscriptionToken = null;
            EventAggregator.GetEvent<RedoEvent>().Unsubscribe(_redoSubscriptionToken);
            _redoSubscriptionToken = null;

            // Unsubscribe to messages from the actions pad.
            EventAggregator.GetEvent<EditItemEvent>().Unsubscribe(_editItemSubscriptionToken);
            _editItemSubscriptionToken = null;

            base.OnNavigatedFrom(navigationContext);
        }
    }
}
