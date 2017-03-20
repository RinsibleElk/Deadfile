using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Events;
using Deadfile.Tab.Navigation;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    public abstract class EditableItemViewModel<TKey, TModel, TState> : ParameterisedViewModel<TKey>, IEditableItemViewModel<TModel>, IPageViewModel
        where TModel : StateManagedModelBase
        where TState : struct
    {
        private readonly TabIdentity _tabIdentity;
        protected readonly IEventAggregator EventAggregator;
        protected readonly IDeadfileDialogCoordinator DialogCoordinator;

        // This is the main undo tracker for the object under management. However, it may not be the active one responding to undo events from the nav-bar.
        protected readonly UndoTracker<TModel> UndoTracker;

        // This is the "active" undo tracker. For the Jobs page, this could represent a billable item.
        private IUndoTracker _activeUndoTracker = null;

        // Subscription tokens.
        private SubscriptionToken _saveSubscriptionToken = null;
        private SubscriptionToken _deleteSubscriptionToken = null;
        private SubscriptionToken _handleEditActionSubscriptionToken;
        private SubscriptionToken _handleUndoSubcriptionToken;
        private SubscriptionToken _discardChangesSubscriptionToken;

        protected EditableItemViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator,
            IDeadfileDialogCoordinator dialogCoordinator,
            UndoTracker<TModel> undoTracker)
        {
            _tabIdentity = tabIdentity;
            EventAggregator = eventAggregator;
            DialogCoordinator = dialogCoordinator;
            UndoTracker = undoTracker;
        }

        /// <summary>
        /// Ask the specific implementation to look up a value in the database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TModel GetModel(TKey key);

        /// <summary>
        /// Ask the specific implementation whether to edit on navigate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract bool ShouldEditOnNavigate(TKey key);

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

        private TModel _selectedItem = null;
        /// <summary>
        /// The item selected in the <see cref="BrowserPaneViewModel"/>.
        /// </summary>
        public TModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != null && value != null && value.Id == _selectedItem.Id)
                {
                    _selectedItem = value;
                    NotifyOfPropertyChange(() => SelectedItem);
                    return;
                }
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
                Editable = false;
                Errors = new List<string>();
                CanEdit = _selectedItem != null && _selectedItem.Id != ModelBase.NewModelId;
                CanDelete = _selectedItem != null && _selectedItem.Id != ModelBase.NewModelId && _selectedItem.StateIsActive;
            }
        }

        internal abstract bool CanEdit { get; set; }
        internal abstract bool CanDelete { get; set; }
        internal abstract bool CanSave { get; set; }
        internal abstract bool UnderEdit { get; set; }

        private TState _state = new TState();
        public TState State
        {
            get { return _state; }
            set
            {
                if (_state.Equals(value)) return;
                _state = value;
                NotifyOfPropertyChange(() => State);
                EventAggregator.GetEvent<PageStateEvent<TState>>().Publish(_state);
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

        private bool _editable = false;
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
                        ActivateUndoTracker(UndoTracker, _selectedItem);
                        _selectedItem.ErrorsChanged += SelectedItemErrorsChanged;
                        _saveSubscriptionToken = EventAggregator.GetEvent<SaveEvent>().Subscribe(PerformSave);
                        _discardChangesSubscriptionToken = EventAggregator.GetEvent<DiscardChangesEvent>().Subscribe(DiscardChangesAction);
                        _selectedItem.RefreshAllErrors();
                        CanDelete = false;
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
                        CanDelete = SelectedItem.Id != ModelBase.NewModelId && SelectedItem.StateIsActive;
                    }

                    EditingStatusChanged(_editable);

                    UnderEdit = _editable;

                    // Only fire when it changes.
                    if (_editable)
                    {
                        EventAggregator.GetEvent<LockedForEditingEvent>()
                            .Publish(new LockedForEditingMessage() {IsLocked = _editable});
                        CanSave = _errors.Count == 0;
                    }
                    else
                    {
                        EventAggregator.GetEvent<LockedForEditingEvent>()
                            .Publish(new LockedForEditingMessage() { IsLocked = _editable, NewParameters = GetLookupParameters() });
                        CanSave = false;
                        // This implies that changes for a new model have been discarded. Best behaviour is to move back off the page and drop the resulting "Forward" action.
                        if (SelectedItem.Id == ModelBase.NewModelId)
                        {
                            EventAggregator.GetEvent<NavigateFallBackEvent>().Publish(NavigateFallBackMessage.FallBack);
                        }
                    }
                }
            }
        }

        protected abstract TKey GetLookupParameters();

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
                case nameof(UndoTracker.CanUndo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_activeUndoTracker.CanUndo ? CanUndoMessage.CanUndo : CanUndoMessage.CannotUndo);
                    break;
                case nameof(UndoTracker.CanRedo):
                    EventAggregator.GetEvent<CanUndoEvent>().Publish(_activeUndoTracker.CanRedo ? CanUndoMessage.CanRedo : CanUndoMessage.CannotRedo);
                    break;
            }
        }

        private async void PerformSave(SaveMessage message)
        {
            var isAnAdd = SelectedItem.Id == ModelBase.NewModelId;
            await PerformSave(message == SaveMessage.SaveAndPrint);

            if (isAnAdd)
            {
                // Notify the browser that something has changed.
                Logger.Info("Event|RefreshBrowserEvent|Send|{0}|{1}", _tabIdentity, RefreshBrowserMessage.Refresh);
                EventAggregator.GetEvent<RefreshBrowserEvent>().Publish(RefreshBrowserMessage.Refresh);
            }
        }

        protected abstract Task PerformSave(bool andPrint);

        protected abstract Task<bool> PerformDelete();

        private async void PerformDelete(DeleteMessage message)
        {
            var deleted = await PerformDelete();
            if (deleted)
            {
                // Notify the browser that something has changed.
                Logger.Info("Event|RefreshBrowserEvent|Send|{0}|{1}", _tabIdentity, RefreshBrowserMessage.Refresh);
                EventAggregator.GetEvent<RefreshBrowserEvent>().Publish(RefreshBrowserMessage.Refresh);
            }
        }

        private void DiscardChangesAction(DiscardChangesMessage discardChangesMessage)
        {
            if (discardChangesMessage == DiscardChangesMessage.Discard)
                while (_activeUndoTracker.CanUndo) _activeUndoTracker.Undo();
        }

        private void HandleEditAction(EditActionMessage editAction)
        {
            // This fires an event to lock down navigation.
            Editable = editAction != EditActionMessage.EndEditing;
        }

        private bool _pendingEdit = false;
        public override void OnNavigatedTo(TKey parameters)
        {
            base.OnNavigatedTo(parameters);

            // Get the model.
            SelectedItem = GetModel(parameters);
            _handleEditActionSubscriptionToken = EventAggregator.GetEvent<EditActionEvent>().Subscribe(HandleEditAction);
            _handleUndoSubcriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(HandleUndo);
            _deleteSubscriptionToken = EventAggregator.GetEvent<DeleteEvent>().Subscribe(PerformDelete);

            // Important. We cannot just immediately start editing - this is because the actions pad will not be loaded yet.
            _pendingEdit = ShouldEditOnNavigate(parameters);
        }

        public abstract Experience Experience { get; }
        public bool ShowActionsPad => true;

        /// <summary>
        /// Finalise navigation by enabling editing if requested.
        /// </summary>
        public void CompleteNavigation()
        {
            Logger.Info("Event|PageStateEvent|Send|{0}|{1}", _tabIdentity.TabIndex, State);
            EventAggregator.GetEvent<PageStateEvent<TState>>().Publish(State);
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
            EventAggregator.GetEvent<DeleteEvent>().Unsubscribe(_deleteSubscriptionToken);
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
        public virtual void ActivateUndoTracker<TObjectUnderEdit>(UndoTracker<TObjectUnderEdit> newActiveUndoTracker, TObjectUnderEdit objectUnderEdit) where TObjectUnderEdit : ModelBase
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
        public virtual void DeactivateUndoTracker()
        {
            if (_activeUndoTracker == null)
                throw new ApplicationException("There is no active object handling undo and redo events to deactivate");
            _activeUndoTracker.PropertyChanged -= UndoTrackerPropertyChanged;
            _activeUndoTracker.Deactivate();
            _activeUndoTracker = null;
        }
    }
}
