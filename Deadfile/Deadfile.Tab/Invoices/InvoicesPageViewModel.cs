using System;
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
    class InvoicesPageViewModel : EditableItemViewModel<ClientAndInvoice, InvoiceModel>, IInvoicesPageViewModel, IBillableModelContainer
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
            var jobs = new ObservableCollection<BillableModel>(_repository.GetBillableModelsForClientAndInvoice(clientAndInvoice.ClientId, clientAndInvoice.InvoiceId));

            // Listen for changes to job state (whether selected as a billed item in this invoice).
            int index = 0;
            NetAmount = 0;
            foreach (var job in jobs)
            {
                job.Index = index++;
                job.Parent = this;
                NetAmount += job.NetAmount;
                int subIndex = 0;
                foreach (var child in job.Children)
                {
                    child.Index = subIndex++;
                    child.Parent = (BillableJob)job;
                }
            }
            Jobs = jobs;

            // Hook up to changes in editing state.
            SelectedItem.PropertyChanged += SelectedItemOnPropertyChanged;

            // Set up the vat values.
            if ((SelectedItem.NetAmount > 0) && (SelectedItem.GrossAmount > 0))
            {
                VatValue = SelectedItem.GrossAmount - SelectedItem.NetAmount;
                VatRate = 100 * VatValue/SelectedItem.NetAmount;
            }
            else if (SelectedItem.Company == Company.PaulSamsonCharteredSurveyorLtd)
            {
                VatRate = 20; // current default
                VatValue = 0;
            }
            else
            {
                VatRate = 0;
                VatValue = 0;
            }
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
            invoiceModel.Repository = _repository;
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
        private double _netAmount;
        private double _vatValue;
        private double _vatRate;

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

        public double NetAmount
        {
            get { return _netAmount; }
            set
            {
                if (value.Equals(_netAmount)) return;
                _netAmount = value;
                NotifyOfPropertyChange(() => NetAmount);
            }
        }

        public double VatValue
        {
            get { return _vatValue; }
            set
            {
                if (value.Equals(_vatValue)) return;
                _vatValue = value;
                NotifyOfPropertyChange(() => VatValue);
            }
        }

        public double VatRate
        {
            get { return _vatRate; }
            set
            {
                if (value.Equals(_vatRate)) return;
                _vatRate = value;
                NotifyOfPropertyChange(() => VatRate);
            }
        }

        public bool AutomaticEditingInProgress { get; set; } = false;
        public void StateChanged(int index)
        {
            // Received notification that the job at this index has had its state changed.
            // Propagate change down to children.
            if (!AutomaticEditingInProgress)
            {
                AutomaticEditingInProgress = true;
                var job = (BillableJob) Jobs[index];
                if (!job.AutomaticEditingInProgress)
                {
                    job.AutomaticEditingInProgress = true;
                    if (job.State == BillableModelState.FullyIncluded || job.State == BillableModelState.Excluded)
                    {
                        foreach (var child in job.Children)
                        {
                            if (child.State != BillableModelState.Claimed)
                            {
                                child.State = job.State;
                            }
                        }
                    }
                    job.AutomaticEditingInProgress = false;
                }
                AutomaticEditingInProgress = false;
            }
            NetAmountChanged(0);
        }

        public void NetAmountChanged(int index)
        {
            NetAmount = 0;
            foreach (var job in Jobs)
            {
                NetAmount += job.NetAmount;
            }
        }
    }
}
