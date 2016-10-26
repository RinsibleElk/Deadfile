﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Undo;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Clients
{
    public class ClientsPageViewModel : ParameterisedContentViewModelBase<int>, IClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        private readonly UndoTracker<ClientModel> _undoTracker = new UndoTracker<ClientModel>();

        public ClientsPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            IDeadfileRepository repository,
            INavigationParameterMapper mapper)
            : base(eventAggregator, navigationService, mapper)
        {
            _repository = repository;
        }

        public override Experience Experience
        {
            get { return Experience.Clients; }
        }

        private ClientModel _selectedClient;

        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                // Careful - we don't want to use ReferenceEquals here!!!
                if ((_selectedClient == null) || (value == null) || (_selectedClient.ClientId != value.ClientId))
                {
                    if (SetProperty(ref _selectedClient, value))
                    {
                        // On Navigation we are always read-only. This is really error handling though, as there should be other
                        // mechanisms in place.
                        Editable = false;
                        Errors = new List<string>();
                    }
                }
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

        private SubscriptionToken _editClientSubscriptionToken = null;
        private SubscriptionToken _navigateToSelectedClientSubscriptionToken = null;
        private SubscriptionToken _undoSubscriptionToken = null;
        private SubscriptionToken _redoSubscriptionToken = null;
        private SubscriptionToken _saveSubscriptionToken = null;

        public override void OnNavigatedTo(NavigationContext navigationContext, int selectedClientId)
        {
            try
            {
                if (selectedClientId == ClientModel.NewClientId)
                    SelectedClient = null;
                else
                    SelectedClient = _repository.GetClientById(selectedClientId);
            }
            catch (Exception)
            {
                SelectedClient = null;
            }

            // subscribe to messages from the browser pane
            _navigateToSelectedClientSubscriptionToken =
                EventAggregator.GetEvent<SelectedClientEvent>().Subscribe(NavigateToClientsPage);
            _undoSubscriptionToken = EventAggregator.GetEvent<UndoEvent>().Subscribe(PerformUndo);
            _redoSubscriptionToken = EventAggregator.GetEvent<RedoEvent>().Subscribe(PerformRedo);

            // subscribe to messages from the actions pane
            _editClientSubscriptionToken = EventAggregator.GetEvent<EditClientEvent>().Subscribe(EditClientAction);
        }

        private void PerformSave()
        {
            try
            {
                _repository.SaveClient(SelectedClient);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        private void EditClientAction(bool editable)
        {
            // This fires an event to lock down navigation.
            Editable = editable;

            // Probably unnecessary, but let's do it.
            Errors = new List<string>();
        }

        private void PerformUndo()
        {
            _undoTracker.Undo();
        }

        private void PerformRedo()
        {
            _undoTracker.Redo();
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Unsubscribe to messages from the browser pane.
            EventAggregator.GetEvent<SelectedClientEvent>().Unsubscribe(_navigateToSelectedClientSubscriptionToken);
            _navigateToSelectedClientSubscriptionToken = null;
            EventAggregator.GetEvent<UndoEvent>().Unsubscribe(_undoSubscriptionToken);
            _undoSubscriptionToken = null;
            EventAggregator.GetEvent<RedoEvent>().Unsubscribe(_redoSubscriptionToken);
            _redoSubscriptionToken = null;

            // Unsubscribe to messages from the actions pad.
            EventAggregator.GetEvent<EditClientEvent>().Unsubscribe(_editClientSubscriptionToken);
            _editClientSubscriptionToken = null;

            base.OnNavigatedFrom(navigationContext);
        }

        private void NavigateToClientsPage(int selectedClientId)
        {
            NavigationService.NavigateTo(Experience.Clients, selectedClientId);
        }

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
                        _undoTracker.Activate(_selectedClient);
                        _undoTracker.PropertyChanged += UndoTrackerPropertyChanged;
                        _selectedClient.ErrorsChanged += SelectedClientErrorsChanged;
                        _saveSubscriptionToken = EventAggregator.GetEvent<SaveEvent>().Subscribe(PerformSave);
                    }
                    else
                    {
                        EventAggregator.GetEvent<SaveEvent>().Unsubscribe(_saveSubscriptionToken);
                        _saveSubscriptionToken = null;
                        _undoTracker.Deactivate();
                        // Deliberately do this after deactivation so that the deactivation takes care of notifying the
                        // browser of CanUndo/CanRedo changes.
                        _undoTracker.PropertyChanged -= UndoTrackerPropertyChanged;
                        _selectedClient.ErrorsChanged -= SelectedClientErrorsChanged;
                    }

                    // Only fire when it changes.
                    EventAggregator.GetEvent<LockedForEditingEvent>().Publish(_editable);
                }
            }
        }

        private void SelectedClientErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            Errors = FlattenErrors();
        }

        private List<string> FlattenErrors()
        {
            List<string> errors = new List<string>();
            Dictionary<string, List<string>> allErrors = SelectedClient.GetAllErrors();
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
    }
}
