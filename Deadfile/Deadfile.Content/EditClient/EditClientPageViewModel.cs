using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Undo;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.EditClient
{
    public sealed class EditClientPageViewModel : ParameterisedContentViewModelBase<int>, IEditClientPageViewModel
    {
        private readonly UndoTracker<ClientModel> _clientUndoTracker = new UndoTracker<ClientModel>();
        private readonly IDeadfileRepository _repository;

        public EditClientPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            INavigationParameterMapper navigationParameterMapper,
            IDeadfileRepository repository)
            : base(eventAggregator, navigationService, navigationParameterMapper)
        {
            _repository = repository;
        }

        private ClientModel _clientModelUnderEdit = new ClientModel();
        public ClientModel ClientModelUnderEdit
        {
            get { return _clientModelUnderEdit; }
            set { SetProperty(ref _clientModelUnderEdit, value); }
        }

        public override Experience Experience { get { return Experience.EditClient; } }

        public override void OnNavigatedTo(NavigationContext navigationContext, int clientId)
        {
            base.OnNavigatedTo(navigationContext, clientId);

            if (clientId == ClientModel.NewClientId)
            {
                Title = "New Client";
                ClientModelUnderEdit = new ClientModel();
            }
            else
            {
                Title = "Edit Client";
                ClientModelUnderEdit = _repository.GetClientById(clientId);
            }

            _clientUndoTracker.Activate(ClientModelUnderEdit);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            _clientUndoTracker.Deactivate();
        }
    }
}
