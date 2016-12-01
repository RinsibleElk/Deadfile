using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace Deadfile.Tab.Clients
{
    class ClientsPageViewModel : EditableItemViewModel<ClientNavigationKey, ClientModel>, IClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;

        public ClientsPageViewModel(IEventAggregator eventAggregator,
            IDeadfileRepository repository,
            IDialogCoordinator dialogCoordinator) : base(eventAggregator, dialogCoordinator, new UndoTracker<ClientModel>())
        {
            _repository = repository;
        }

        /// <summary>
        /// User hits the button for Add New Job.
        /// </summary>
        public void AddNewJob()
        {
            // Navigate to the Jobs page with the specified Client and no job given.
            EventAggregator.GetEvent<AddNewJobEvent>().Publish(SelectedItem.ClientId);
        }

        /// <summary>
        /// User hits the button for Invoice Client.
        /// </summary>
        public void InvoiceClient()
        {
            // Navigate to the Invoices page with the specified Client and no invoice given.
            EventAggregator.GetEvent<InvoiceClientEvent>().Publish(SelectedItem.ClientId);
        }

        private bool _canAddNewJob = false;
        public bool CanAddNewJob
        {
            get { return _canAddNewJob; }
            set
            {
                if (value == _canAddNewJob) return;
                _canAddNewJob = value;
                NotifyOfPropertyChange(() => CanAddNewJob);
            }
        }

        private bool _canInvoiceClient = false;
        public bool CanInvoiceClient
        {
            get { return _canInvoiceClient; }
            set
            {
                if (value == _canInvoiceClient) return;
                _canInvoiceClient = value;
                NotifyOfPropertyChange(() => CanInvoiceClient);
            }
        }

        public override void OnNavigatedTo(ClientNavigationKey selectedClient)
        {
            var idToSelect = selectedClient.ClientId == 0 ? ModelBase.NewModelId : selectedClient.ClientId;
            base.OnNavigatedTo(new ClientNavigationKey(idToSelect));

            CanAddNewJob = (!Editable) && (idToSelect != ModelBase.NewModelId);
            CanInvoiceClient = (!Editable) && (idToSelect != ModelBase.NewModelId);
        }

        protected override ClientModel GetModel(ClientNavigationKey key)
        {
            ClientModel clientModel;
            if (key.ClientId == 0 || key.ClientId == ModelBase.NewModelId)
            {
                clientModel = new ClientModel();
                DisplayName = "Clients";
            }
            else
            {
                clientModel = _repository.GetClientById(key.ClientId);
                if (clientModel.ClientId == ModelBase.NewModelId)
                    DisplayName = "Clients";
                else
                    DisplayName = clientModel.FullName;
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return clientModel;
        }

        protected override bool ShouldEditOnNavigate(ClientNavigationKey key)
        {
            return key.ClientId == ModelBase.NewModelId;
        }

        protected override ClientNavigationKey GetLookupParameters()
        {
            return new ClientNavigationKey(SelectedItem.ClientId);
        }

        public override void EditingStatusChanged(bool editable)
        {
            CanAddNewJob = (!Editable) && (SelectedItem.ClientId != ModelBase.NewModelId);
            CanInvoiceClient = (!Editable) && (SelectedItem.ClientId != ModelBase.NewModelId);
        }

        public override void PerformSave()
        {
            try
            {
                // For a new client this will also set the ClientId.
                _repository.SaveClient(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public override void PerformDelete()
        {
            try
            {
                _repository.DeleteClient(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public Experience Experience
        {
            get { return Experience.Clients; }
        }

        public bool ShowActionsPad { get; } = true;
    }
}
