using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Actions
{
    class ActionsPadViewModel : PropertyChangedBase, IActionsPadViewModel, INavigationAware
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected readonly TabIdentity TabIdentity;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken;
        private SubscriptionToken _canEditEventSubscriptionToken;
        private SubscriptionToken _canSaveEventSubscriptionToken;
        private SubscriptionToken _canDeleteEventSubscriptionToken;
        protected IEventAggregator EventAggregator { get; private set; }

        public ActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator)
        {
            TabIdentity = tabIdentity;
            EventAggregator = eventAggregator;
        }

        public void EditItem()
        {
            Logger.Info("Event,EditActionEvent,Send,{0},{1}", TabIdentity.TabIndex, EditActionMessage.StartEditing);
            EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.StartEditing);
        }

        private bool _canEditItem = true;
        private bool _editItemIsVisible = true;
        private bool _saveItemIsVisible = false;
        private bool _canSaveItem = false;
        private bool _canDeleteItem = true;
        private bool _deleteItemIsVisible = true;
        private bool _discardItemIsVisible = false;
        private bool _canDiscardItem = true;
        private SubscriptionToken _canDiscardEventSubscriptionToken = null;

        public bool CanEditItem
        {
            get { return _canEditItem; }
            set
            {
                if (value == _canEditItem) return;
                _canEditItem = value;
                NotifyOfPropertyChange(() => CanEditItem);
                CanEditItemChanged(_canEditItem);
            }
        }

        public bool EditItemIsVisible
        {
            get { return _editItemIsVisible; }
            set
            {
                if (value == _editItemIsVisible) return;
                _editItemIsVisible = value;
                NotifyOfPropertyChange(() => EditItemIsVisible);
            }
        }

        public void SaveItem()
        {
            try
            {
                // Perform the save, and lock the item again.
                Logger.Info("Event,SaveEvent,Send,{0},{1}", TabIdentity.TabIndex, SaveMessage.Save);
                EventAggregator.GetEvent<SaveEvent>().Publish(SaveMessage.Save);

                // Notify the other pages for the end of editing.
                Logger.Info("Event,EditActionEvent,Send,{0},{1}", TabIdentity.TabIndex, EditActionMessage.EndEditing);
                EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception thrown during Save");
                throw;
            }
        }

        public bool CanSaveItem
        {
            get { return _canSaveItem; }
            set
            {
                if (value == _canSaveItem) return;
                _canSaveItem = value;
                NotifyOfPropertyChange(() => CanSaveItem);
                CanSaveItemChanged(_canSaveItem);
            }
        }

        protected virtual void CanSaveItemChanged(bool canSave)
        {
        }

        protected virtual void CanEditItemChanged(bool canEdit)
        {
        }

        public bool SaveItemIsVisible
        {
            get { return _saveItemIsVisible; }
            set
            {
                if (value == _saveItemIsVisible) return;
                _saveItemIsVisible = value;
                NotifyOfPropertyChange(() => SaveItemIsVisible);
            }
        }

        public void DeleteItem()
        {
            // Perform the deletion.
            Logger.Info("Event,DeleteEvent,Send,{0},{1}", TabIdentity.TabIndex, DeleteMessage.Delete);
            EventAggregator.GetEvent<DeleteEvent>().Publish(DeleteMessage.Delete);
        }

        public bool CanDeleteItem
        {
            get { return _canDeleteItem; }
            set
            {
                if (value == _canDeleteItem) return;
                _canDeleteItem = value;
                NotifyOfPropertyChange(() => CanDeleteItem);
            }
        }

        public bool DeleteItemIsVisible
        {
            get { return _deleteItemIsVisible; }
            set
            {
                if (value == _deleteItemIsVisible) return;
                _deleteItemIsVisible = value;
                NotifyOfPropertyChange(() => DeleteItemIsVisible);
            }
        }

        public void DiscardItem()
        {
            try
            {
                // Notify of the Discard. This leads to en masse Undo-ing.
                Logger.Info("Event,DiscardChangesEvent,Send,{0},{1}", TabIdentity.TabIndex,
                    DiscardChangesMessage.Discard);
                EventAggregator.GetEvent<DiscardChangesEvent>().Publish(DiscardChangesMessage.Discard);

                // Notify the other pages for the end of editing.
                Logger.Info("Event,EditActionEvent,Send,{0},{1}", TabIdentity.TabIndex, EditActionMessage.EndEditing);
                EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception thrown during Discard");
                throw;
            }
        }

        public bool CanDiscardItem
        {
            get { return _canDiscardItem; }
            set
            {
                if (value == _canDiscardItem) return;
                _canDiscardItem = value;
                NotifyOfPropertyChange(() => CanDiscardItem);
            }
        }

        public bool DiscardItemIsVisible
        {
            get { return _discardItemIsVisible; }
            set
            {
                if (value == _discardItemIsVisible) return;
                _discardItemIsVisible = value;
                NotifyOfPropertyChange(() => DiscardItemIsVisible);
            }
        }

        public void OnNavigatedTo(object parameters)
        {
            // Subscribe for event from content.
            _lockedForEditingEventSubscriptionToken =
                EventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(LockedForEditingAction);
            _canSaveEventSubscriptionToken =
                EventAggregator.GetEvent<CanSaveEvent>().Subscribe(CanSaveAction);
            _canDeleteEventSubscriptionToken =
                EventAggregator.GetEvent<CanDeleteEvent>().Subscribe(CanDeleteAction);
            _canDiscardEventSubscriptionToken =
                EventAggregator.GetEvent<CanDiscardEvent>().Subscribe(CanDiscardAction);
            _canEditEventSubscriptionToken =
                EventAggregator.GetEvent<CanEditEvent>().Subscribe(CanEditAction);
        }

        private void CanDiscardAction(CanDiscardMessage message)
        {
            CanDiscardItem = message == CanDiscardMessage.CanDiscard;
        }

        private void CanEditAction(CanEditMessage canEditMessage)
        {
            Logger.Info("Event,CanEditEvent,Receive,{0},{1}", TabIdentity.TabIndex, canEditMessage);
            CanEditItem = canEditMessage == CanEditMessage.CanEdit;
        }

        private void CanSaveAction(CanSaveMessage canSaveMessage)
        {
            Logger.Info("Event,CanSaveEvent,Receive,{0},{1}", TabIdentity.TabIndex, canSaveMessage);
            CanSaveItem = canSaveMessage == CanSaveMessage.CanSave;
        }

        private void CanDeleteAction(CanDeleteMessage canDeleteMessage)
        {
            Logger.Info("Event,CanDeleteEvent,Receive,{0},{1}", TabIdentity.TabIndex, canDeleteMessage);
            CanDeleteItem = canDeleteMessage == CanDeleteMessage.CanDelete;
        }

        protected virtual void LockedForEditingAction(LockedForEditingMessage lockedForEditingMessage)
        {
            Logger.Info("Event,LockedForEditingEvent,Receive,{0},{1}", TabIdentity.TabIndex, lockedForEditingMessage);
            SaveItemIsVisible = lockedForEditingMessage.IsLocked;
            DiscardItemIsVisible = lockedForEditingMessage.IsLocked;
            EditItemIsVisible = !lockedForEditingMessage.IsLocked;
            DeleteItemIsVisible = !lockedForEditingMessage.IsLocked;
        }

        public void OnNavigatedFrom()
        {
            // Unsubscribe for event from content.
            EventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingEventSubscriptionToken);
            _lockedForEditingEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanSaveEvent>().Unsubscribe(_canSaveEventSubscriptionToken);
            _canSaveEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanEditEvent>().Unsubscribe(_canEditEventSubscriptionToken);
            _canEditEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanDeleteEvent>().Unsubscribe(_canDeleteEventSubscriptionToken);
            _canDeleteEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanDiscardEvent>().Unsubscribe(_canDiscardEventSubscriptionToken);
            _canDiscardEventSubscriptionToken = null;
        }
    }
}
