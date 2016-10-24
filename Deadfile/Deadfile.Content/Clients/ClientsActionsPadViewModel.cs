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
        private readonly DelegateCommand _editClientCommand;
        private readonly DelegateCommand _newClientCommand;
        public ClientsActionsPadViewModel(IEventAggregator eventAggregator, IDeadfileNavigationService navigationService) : base(eventAggregator)
        {
            _navigationService = navigationService;
            _editClientCommand = new DelegateCommand(EditClientAction);
            _newClientCommand = new DelegateCommand(NewClientAction);
        }

        public ICommand NewClientCommand { get { return _newClientCommand; } }
        public ICommand EditClientCommand { get { return _editClientCommand; } }

        private void EditClientAction()
        {
        }

        private void NewClientAction()
        {
        }
    }
}
