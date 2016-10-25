using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly DelegateCommand _deleteClientCommand;
        public ClientsActionsPadViewModel(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _addClientCommand = new DelegateCommand(AddClientAction);
            _editClientCommand = new DelegateCommand(EditClientAction);
            _saveClientCommand = new DelegateCommand(SaveClientAction);
            _deleteClientCommand = new DelegateCommand(DeleteClientAction);
        }

        public ICommand AddClientCommand { get { return _addClientCommand; } }
        public ICommand EditClientCommand { get { return _editClientCommand; } }
        public ICommand SaveClientCommand { get { return _saveClientCommand; } }
        public ICommand DeleteClientCommand { get { return _deleteClientCommand; } }

        private void EditClientAction()
        {
            EventAggregator.GetEvent<EditClientEvent>().Publish();
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
