using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.ActionsPad
{
    class ActionsPadViewModel : ViewModelBase, IActionsPadViewModel
    {
        private readonly DelegateCommand _editItemCommand;
        private readonly DelegateCommand _saveItemCommand;
        private readonly DelegateCommand _discardItemCommand;
        private readonly DelegateCommand _deleteItemCommand;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken = null;
        private SubscriptionToken _canSaveEventSubscriptionToken = null;
        private SubscriptionToken _canEditEventSubscriptionToken = null;

        public ActionsPadViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _editItemCommand = new DelegateCommand(EditItemAction, CanEditItem);
            _saveItemCommand = new DelegateCommand(SaveItemAction, CanSaveItem);
            _discardItemCommand = new DelegateCommand(DiscardItemAction, CanDiscardItem);
            _deleteItemCommand = new DelegateCommand(DeleteItemAction, CanDeleteItem);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // Subscribe for event from content.
            _lockedForEditingEventSubscriptionToken =
                EventAggregator.GetEvent<LockedForEditingEvent>().Subscribe(LockedForEditingAction);
            _canSaveEventSubscriptionToken =
                EventAggregator.GetEvent<CanSaveEvent>().Subscribe(CanSaveAction);
            _canEditEventSubscriptionToken =
                EventAggregator.GetEvent<CanEditEvent>().Subscribe(CanEditAction);
        }

        private bool _canSave = true;
        private void CanSaveAction(bool canSave)
        {
            _canSave = canSave;
            _saveItemCommand.RaiseCanExecuteChanged();
        }

        private bool _canEdit = false;
        private void CanEditAction(bool canEdit)
        {
            _canEdit = canEdit;
            _editItemCommand.RaiseCanExecuteChanged();
        }

        protected virtual void LockedForEditingAction(bool lockedForEditing)
        {
            SaveItemVisibility = lockedForEditing ? Visibility.Visible : Visibility.Collapsed;
            DiscardItemVisibility = lockedForEditing ? Visibility.Visible : Visibility.Collapsed;
            EditItemVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
            DeleteItemVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            // Unsubscribe for event from content.
            EventAggregator.GetEvent<LockedForEditingEvent>().Unsubscribe(_lockedForEditingEventSubscriptionToken);
            _lockedForEditingEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanSaveEvent>().Unsubscribe(_canSaveEventSubscriptionToken);
            _canSaveEventSubscriptionToken = null;
            EventAggregator.GetEvent<CanEditEvent>().Unsubscribe(_canEditEventSubscriptionToken);
            _canEditEventSubscriptionToken = null;
        }

        private void DiscardItemAction()
        {
            // Notify of the Discard. This leads to en mass Undo-ing.
            EventAggregator.GetEvent<DiscardChangesEvent>().Publish();

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditItemEvent>().Publish(EditAction.EndEditing);
        }

        private bool CanDiscardItem()
        {
            return _discardItemVisibility == Visibility.Visible;
        }

        private bool CanDeleteItem()
        {
            //TODO
            return _deleteItemVisibility == Visibility.Visible;
        }

        private bool CanSaveItem()
        {
            return _saveItemVisibility == Visibility.Visible && _canSave;
        }

        private bool CanEditItem()
        {
            return _editItemVisibility == Visibility.Visible && _canEdit;
        }

        public ICommand EditItemCommand
        {
            get { return _editItemCommand; }
        }

        public ICommand SaveItemCommand
        {
            get { return _saveItemCommand; }
        }

        public ICommand DeleteItemCommand
        {
            get { return _deleteItemCommand; }
        }

        public ICommand DiscardItemCommand
        {
            get { return _discardItemCommand; }
        }

        private Visibility _editItemVisibility = Visibility.Visible;

        public Visibility EditItemVisibility
        {
            get { return _editItemVisibility; }
            set
            {
                if (SetProperty(ref _editItemVisibility, value))
                {
                    _editItemCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _saveItemVisibility = Visibility.Collapsed;

        public Visibility SaveItemVisibility
        {
            get { return _saveItemVisibility; }
            set
            {
                if (SetProperty(ref _saveItemVisibility, value))
                {
                    _saveItemCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _discardItemVisibility = Visibility.Collapsed;

        public Visibility DiscardItemVisibility
        {
            get { return _discardItemVisibility; }
            set
            {
                if (SetProperty(ref _discardItemVisibility, value))
                {
                    _discardItemCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _deleteItemVisibility = Visibility.Visible;

        public Visibility DeleteItemVisibility
        {
            get { return _deleteItemVisibility; }
            set
            {
                if (SetProperty(ref _deleteItemVisibility, value))
                {
                    _deleteItemCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void EditItemAction()
        {
            EventAggregator.GetEvent<EditItemEvent>().Publish(EditAction.StartEditing);
        }

        private void SaveItemAction()
        {
            // Perform the save, and lock the item again.
            //TODO If this fails at the moment I'm pretty boned.
            EventAggregator.GetEvent<SaveEvent>().Publish();

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditItemEvent>().Publish(EditAction.EndEditing);
        }

        private void DeleteItemAction()
        {

        }
    }
}
