using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly IUrlNavigationService _urlNavigationService;

        public ClientsPageViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator,
            IDeadfileRepository repository,
            IDialogCoordinator dialogCoordinator,
            IUrlNavigationService urlNavigationService) : base(tabIdentity, eventAggregator, dialogCoordinator, new UndoTracker<ClientModel>())
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _urlNavigationService = urlNavigationService;
        }

        /// <summary>
        /// User hits the button for Add New Job.
        /// </summary>
        public void AddNewJob()
        {
            // Navigate to the Jobs page with the specified Client and no job given.
            Logger.Info("Event,AddNewJobEvent,Send,{0},{1}", _tabIdentity.TabIndex, SelectedItem.ClientId);
            EventAggregator.GetEvent<AddNewJobEvent>().Publish(SelectedItem.ClientId);
        }

        /// <summary>
        /// User hits the button for Invoice Client.
        /// </summary>
        public void InvoiceClient()
        {
            // Navigate to the Invoices page with the specified Client and no invoice given.
            Logger.Info("Event,InvoiceClientEvent,Send,{0},{1}", _tabIdentity.TabIndex, SelectedItem.ClientId);
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

        public void EmailClient()
        {
            if (!String.IsNullOrWhiteSpace(SelectedItem.EmailAddress))
                _urlNavigationService.SendEmail(SelectedItem.EmailAddress);
            else
                CanEmailClient = false;
        }

        private bool _canEmailClient = false;
        public bool CanEmailClient
        {
            get { return _canEmailClient; }
            set
            {
                if (value == _canEmailClient) return;
                _canEmailClient = value;
                NotifyOfPropertyChange(() => CanEmailClient);
            }
        }

        public override void OnNavigatedTo(ClientNavigationKey selectedClient)
        {
            Logger.Info("Navigated to Clients {0}, for client {1}", _tabIdentity.TabIndex, selectedClient);
            var idToSelect = selectedClient.ClientId == 0 ? ModelBase.NewModelId : selectedClient.ClientId;
            base.OnNavigatedTo(new ClientNavigationKey(idToSelect));

            CanAddNewJob = (!Editable) && (idToSelect != ModelBase.NewModelId);
            CanInvoiceClient = (!Editable) && (idToSelect != ModelBase.NewModelId);
            CanEmailClient = (!Editable) && (idToSelect != ModelBase.NewModelId) &&
                             (!String.IsNullOrWhiteSpace(SelectedItem.EmailAddress));
        }

        public override void OnNavigatedFrom()
        {
            Logger.Info("Navigated to Clients {0}, for client {1}", _tabIdentity.TabIndex, SelectedItem.ClientId);
            base.OnNavigatedFrom();
        }

        protected override ClientModel GetModel(ClientNavigationKey key)
        {
            ClientModel clientModel;
            if (key.ClientId == 0 || key.ClientId == ModelBase.NewModelId)
            {
                clientModel = new ClientModel();
                DisplayName = "New Client";
            }
            else
            {
                clientModel = _repository.GetClientById(key.ClientId);
                if (clientModel.ClientId == ModelBase.NewModelId)
                    DisplayName = "New Client";
                else
                    DisplayName = clientModel.FullName;
            }
            Logger.Info("Event,DisplayNameEvent,Send,{0},{1}", _tabIdentity.TabIndex, DisplayName);
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
            CanEmailClient = (!Editable) && (SelectedItem.ClientId != ModelBase.NewModelId) &&
                 (!String.IsNullOrWhiteSpace(SelectedItem.EmailAddress));
        }

        protected override void PerformSave(SaveMessage message)
        {
            try
            {
                // For a new client this will also set the ClientId.
                _repository.SaveClient(SelectedItem);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while saving {0}, {1}, {2}, {3}", _tabIdentity, SelectedItem, e, e.StackTrace);
                throw;
            }
        }

        protected override bool MayDelete(out string details)
        {
            details = null;
            return true;
        }

        protected override void PerformDelete()
        {
            try
            {
                SelectedItem.Status = ClientStatus.Inactive;
                PerformSave(SaveMessage.Save);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while deleting {0}, {1}, {2}, {3}", _tabIdentity, SelectedItem, e, e.StackTrace);
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
