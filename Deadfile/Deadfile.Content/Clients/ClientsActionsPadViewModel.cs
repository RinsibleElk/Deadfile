﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;
using Deadfile.Content.ViewModels;
using Prism.Commands;

namespace Deadfile.Content.Clients
{
    public sealed class ClientsActionsPadViewModel : ViewModelBase, IClientsActionsPadViewModel
    {
        private readonly DelegateCommand _addClientCommand;
        private readonly DelegateCommand _editClientCommand;
        private readonly DelegateCommand _saveClientCommand;
        private readonly DelegateCommand _discardClientCommand;
        private readonly DelegateCommand _deleteClientCommand;
        private SubscriptionToken _lockedForEditingEventSubscriptionToken = null;
        private SubscriptionToken _canSaveEventSubscriptionToken = null;
        private SubscriptionToken _canEditEventSubscriptionToken = null;

        public ClientsActionsPadViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _addClientCommand = new DelegateCommand(AddClientAction, CanAddClient);
            _editClientCommand = new DelegateCommand(EditClientAction, CanEditClient);
            _saveClientCommand = new DelegateCommand(SaveClientAction, CanSaveClient);
            _discardClientCommand = new DelegateCommand(DiscardClientAction, CanDiscardClient);
            _deleteClientCommand = new DelegateCommand(DeleteClientAction, CanDeleteClient);
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
            _saveClientCommand.RaiseCanExecuteChanged();
        }

        private bool _canEdit = false;
        private void CanEditAction(bool canEdit)
        {
            _canEdit = canEdit;
            _editClientCommand.RaiseCanExecuteChanged();
        }

        private void LockedForEditingAction(bool lockedForEditing)
        {
            AddClientVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
            SaveClientVisibility = lockedForEditing ? Visibility.Visible : Visibility.Collapsed;
            DiscardClientVisibility = lockedForEditing ? Visibility.Visible : Visibility.Collapsed;
            EditClientVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
            DeleteClientVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
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

        private void DiscardClientAction()
        {
            // Notify of the Discard. This leads to en mass Undo-ing.
            EventAggregator.GetEvent<DiscardChangesEvent>().Publish();

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditClientEvent>().Publish(ClientEdit.EndEditing);
        }

        private bool CanDiscardClient()
        {
            return _discardClientVisibility == Visibility.Visible;
        }

        private bool CanDeleteClient()
        {
            //TODO
            return _deleteClientVisibility == Visibility.Visible;
        }

        private bool CanSaveClient()
        {
            return _saveClientVisibility == Visibility.Visible && _canSave;
        }

        private bool CanEditClient()
        {
            return _editClientVisibility == Visibility.Visible && _canEdit;
        }

        private bool CanAddClient()
        {
            return _addClientVisibility == Visibility.Visible;
        }

        public ICommand AddClientCommand
        {
            get { return _addClientCommand; }
        }

        public ICommand EditClientCommand
        {
            get { return _editClientCommand; }
        }

        public ICommand SaveClientCommand
        {
            get { return _saveClientCommand; }
        }

        public ICommand DeleteClientCommand
        {
            get { return _deleteClientCommand; }
        }

        public ICommand DiscardClientCommand
        {
            get { return _discardClientCommand; }
        }

        private Visibility _addClientVisibility = Visibility.Visible;

        public Visibility AddClientVisibility
        {
            get { return _addClientVisibility; }
            set
            {
                if (SetProperty(ref _addClientVisibility, value))
                {
                    _addClientCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _editClientVisibility = Visibility.Visible;

        public Visibility EditClientVisibility
        {
            get { return _editClientVisibility; }
            set
            {
                if (SetProperty(ref _editClientVisibility, value))
                {
                    _editClientCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _saveClientVisibility = Visibility.Collapsed;

        public Visibility SaveClientVisibility
        {
            get { return _saveClientVisibility; }
            set
            {
                if (SetProperty(ref _saveClientVisibility, value))
                {
                    _saveClientCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _discardClientVisibility = Visibility.Collapsed;

        public Visibility DiscardClientVisibility
        {
            get { return _discardClientVisibility; }
            set
            {
                if (SetProperty(ref _discardClientVisibility, value))
                {
                    _discardClientCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Visibility _deleteClientVisibility = Visibility.Visible;

        public Visibility DeleteClientVisibility
        {
            get { return _deleteClientVisibility; }
            set
            {
                if (SetProperty(ref _deleteClientVisibility, value))
                {
                    _deleteClientCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void EditClientAction()
        {
            EventAggregator.GetEvent<EditClientEvent>().Publish(ClientEdit.StartEditing);
        }

        private void AddClientAction()
        {
            EventAggregator.GetEvent<EditClientEvent>().Publish(ClientEdit.NewClient);
        }

        private void SaveClientAction()
        {
            // Perform the save, and lock the client again.
            //TODO If this fails at the moment I'm pretty boned.
            EventAggregator.GetEvent<SaveEvent>().Publish();

            // Notify the other pages for the end of editing.
            EventAggregator.GetEvent<EditClientEvent>().Publish(ClientEdit.EndEditing);
        }

        private void DeleteClientAction()
        {
            
        }
    }
}
