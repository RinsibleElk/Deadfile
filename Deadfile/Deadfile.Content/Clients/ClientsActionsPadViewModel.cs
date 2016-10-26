using System;
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
        private readonly IDeadfileNavigationService _navigationService;
        private readonly DelegateCommand _addClientCommand;
        private readonly DelegateCommand _editClientCommand;
        private readonly DelegateCommand _saveClientCommand;
        private readonly DelegateCommand _discardClientCommand;
        private readonly DelegateCommand _deleteClientCommand;

        public ClientsActionsPadViewModel(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _addClientCommand = new DelegateCommand(AddClientAction, CanAddClient);
            _editClientCommand = new DelegateCommand(EditClientAction, CanEditClient);
            _saveClientCommand = new DelegateCommand(SaveClientAction, CanSaveClient);
            _discardClientCommand=new DelegateCommand(DiscardClientAction, CanDiscardClient);
            _deleteClientCommand = new DelegateCommand(DeleteClientAction, CanDeleteClient);
        }

        private void DiscardClientAction()
        {
        }

        private bool CanDiscardClient()
        {
            return true;
        }

        private bool CanDeleteClient()
        {
            //TODO
            return true;
        }

        private bool CanSaveClient()
        {
            //TODO
            return false;
        }

        private bool CanEditClient()
        {
            //TODO
            return true;
        }

        private bool CanAddClient()
        {
            //TODO
            return true;
        }

        public ICommand AddClientCommand { get { return _addClientCommand; } }
        public ICommand EditClientCommand { get { return _editClientCommand; } }
        public ICommand SaveClientCommand { get { return _saveClientCommand; } }
        public ICommand DeleteClientCommand { get { return _deleteClientCommand; } }
        public ICommand DiscardClientCommand { get { return _discardClientCommand; } }

        private Visibility _addClientVisibility = Visibility.Visible;
        public Visibility AddClientVisibility
        {
            get { return _addClientVisibility; }
            set { SetProperty(ref _addClientVisibility, value); }
        }

        private Visibility _editClientVisibility = Visibility.Visible;
        public Visibility EditClientVisibility
        {
            get { return _editClientVisibility; }
            set { SetProperty(ref _editClientVisibility, value); }
        }

        private Visibility _saveClientVisibility = Visibility.Collapsed;
        public Visibility SaveClientVisibility
        {
            get { return _saveClientVisibility; }
            set { SetProperty(ref _saveClientVisibility, value); }
        }

        private Visibility _discardClientVisibility = Visibility.Collapsed;
        public Visibility DiscardClientVisibility
        {
            get { return _discardClientVisibility; }
            set { SetProperty(ref _discardClientVisibility, value); }
        }

        private Visibility _deleteClientVisibility = Visibility.Visible;
        public Visibility DeleteClientVisibility
        {
            get { return _deleteClientVisibility; }
            set { SetProperty(ref _deleteClientVisibility, value); }
        }

        private void EditClientAction()
        {
            var lfee= EventAggregator.GetEvent<EditClientEvent>();
            lfee.Publish();
        }

        private void AddClientAction()
        {
        }

        private void SaveClientAction()
        {
            
        }

        private void DeleteClientAction()
        {
            
        }
    }
}
