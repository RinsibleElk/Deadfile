﻿using System;
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
        private SubscriptionToken _lockedForEditingEventSubscriptionToken;
        private SubscriptionToken _canEditEventSubscriptionToken;
        private SubscriptionToken _canSaveEventSubscriptionToken;
        private SubscriptionToken _canDeleteEventSubscriptionToken;
        protected IEventAggregator EventAggregator { get; private set; }

        public ActionsPadViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void EditItem()
        {
            EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.StartEditing);
        }

        private bool _canEditItem = true;
        private bool _editItemIsVisible = true;
        private bool _saveItemIsVisible = false;
        private bool _canSaveItem = false;
        private bool _canDeleteItem = true;
        private bool _deleteItemIsVisible = true;
        private bool _discardItemIsVisible = false;

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
            // Perform the save, and lock the item again.
            //TODO If this fails at the moment I'm pretty boned.
            EventAggregator.GetEvent<SaveEvent>().Publish(SaveMessage.Save);

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
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
            // Notify of the Discard. This leads to en masse Undo-ing.
            EventAggregator.GetEvent<DiscardChangesEvent>().Publish(DiscardChangesMessage.Discard);

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
        }

        public bool CanDiscardItem
        {
            get { return _discardItemIsVisible; }
            set
            {
                if (value == _discardItemIsVisible) return;
                _discardItemIsVisible = value;
                NotifyOfPropertyChange(() => CanDiscardItem);
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
            _canEditEventSubscriptionToken =
                EventAggregator.GetEvent<CanEditEvent>().Subscribe(CanEditAction);
        }

        private void CanEditAction(CanEditMessage canEditMessage)
        {
            CanEditItem = canEditMessage == CanEditMessage.CanEdit;
        }

        private void CanSaveAction(CanSaveMessage canSaveMessage)
        {
            CanSaveItem = canSaveMessage == CanSaveMessage.CanSave;
        }

        private void CanDeleteAction(CanDeleteMessage canDeleteMessage)
        {
            CanDeleteItem = canDeleteMessage == CanDeleteMessage.CanDelete;
        }

        protected virtual void LockedForEditingAction(LockedForEditingMessage lockedForEditingMessage)
        {
            SaveItemIsVisible = lockedForEditingMessage.IsLocked;
            CanDiscardItem = lockedForEditingMessage.IsLocked;
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
        }
    }
}
