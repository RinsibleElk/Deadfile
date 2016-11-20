﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageViewModel : EditableItemViewModel<ClientAndInvoice, InvoiceModel>, IInvoicesPageViewModel
    {
        private readonly IDeadfileRepository _repository;

        public InvoicesPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        public override void OnNavigatedTo(ClientAndInvoice clientAndInvoice)
        {
            base.OnNavigatedTo(clientAndInvoice);

            // Find all the billable items for this client, attributing them by whether they are included in this invoice
            // or any other invoice.
            Jobs = new ObservableCollection<BillableModel>(_repository.GetBillableModelsForClient(clientAndInvoice.ClientId, clientAndInvoice.InvoiceId));

            // Hook up to changes in editing state.
            SelectedItem.PropertyChanged += SelectedItemOnPropertyChanged;
        }

        private void SelectedItemOnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(InvoiceModel.CreationState))
            {
                NotifyOfPropertyChange(nameof(CanSetCompany));
                NotifyOfPropertyChange(nameof(CanSetBillableItems));
                NotifyOfPropertyChange(nameof(InvoiceEditable));
            }
        }

        void INavigationAware.OnNavigatedFrom()
        {
            Jobs = new ObservableCollection<BillableModel>();
            SelectedItem.PropertyChanged -= SelectedItemOnPropertyChanged;
        }

        protected override InvoiceModel GetModel(ClientAndInvoice clientAndInvoice)
        {
            InvoiceModel invoiceModel;
            if (clientAndInvoice.Equals(default(ClientAndInvoice)) || clientAndInvoice.InvoiceId == 0 || clientAndInvoice.InvoiceId == ModelBase.NewModelId)
            {
                invoiceModel = new InvoiceModel();
                DisplayName = "New Invoice";
            }
            else
            {
                invoiceModel = _repository.GetInvoiceById(clientAndInvoice.InvoiceId);
                if (invoiceModel.InvoiceId == ModelBase.NewModelId)
                    DisplayName = "New Invoice";
                else
                    DisplayName = CompanyUtils.GetShortName(invoiceModel.Company) + " " + invoiceModel.InvoiceReference;
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return invoiceModel;
        }

        protected override bool ShouldEditOnNavigate(ClientAndInvoice clientAndInvoice)
        {
            return clientAndInvoice.InvoiceId == ModelBase.NewModelId;
        }

        public override void EditingStatusChanged(bool editable)
        {
            NotifyOfPropertyChange(nameof(CanSetCompany));
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
            NotifyOfPropertyChange(nameof(InvoiceEditable));
        }

        public bool CanSetCompany
        {
            get { return Editable && SelectedItem.CreationState == InvoiceCreationState.DefineCompany; }
        }

        public bool CanSetBillableItems
        {
            get { return Editable && SelectedItem.CreationState == InvoiceCreationState.DefineBillables; }
        }

        public bool InvoiceEditable
        {
            get { return Editable && SelectedItem.CreationState == InvoiceCreationState.DefineInvoice; }
        }

        public override void PerformSave()
        {
            try
            {
                _repository.SaveInvoice(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public Experience Experience { get; } = Experience.Invoices;
        public bool ShowActionsPad { get; } = true;

        private string _filterText = "";
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (value == _filterText) return;
                _filterText = value;
                NotifyOfPropertyChange(() => FilterText);
            }
        }

        private ObservableCollection<BillableModel> _jobs;
        public ObservableCollection<BillableModel> Jobs
        {
            get { return _jobs; }
            set
            {
                if (Equals(value, _jobs)) return;
                _jobs = value;
                NotifyOfPropertyChange(() => Jobs);
            }
        }

        public void SetCompany()
        {
            SelectedItem.CreationState = InvoiceCreationState.DefineBillables;
            NotifyOfPropertyChange(nameof(CanSetCompany));
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
        }

        public void SetBillableItems()
        {
            SelectedItem.CreationState = InvoiceCreationState.DefineInvoice;
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
            NotifyOfPropertyChange(nameof(InvoiceEditable));
        }
    }
}
