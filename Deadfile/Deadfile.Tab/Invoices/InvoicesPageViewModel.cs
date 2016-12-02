using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageViewModel : EditableItemViewModel<ClientAndInvoiceNavigationKey, InvoiceModel>, IInvoicesPageViewModel, IBillableModelContainer
    {
        private readonly IDeadfileRepository _repository;

        public InvoicesPageViewModel(IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator) : base(eventAggregator, dialogCoordinator, new ParentUndoTracker<InvoiceModel, InvoiceItemModel>())
        {
            _repository = repository;
            AddItemCommand = new DelegateCommand(AddItemAction);
        }

        private void AddItemAction()
        {
            UndoTracker.AddChild();
        }

        public override void OnNavigatedTo(ClientAndInvoiceNavigationKey clientAndInvoiceNavigationKey)
        {
            base.OnNavigatedTo(clientAndInvoiceNavigationKey);

            SuggestedInvoiceReferences = new ObservableCollection<int>(new int[] {SelectedItem.InvoiceReference});

            // Find all the billable items for this client, attributing them by whether they are included in this invoice
            // or any other invoice.
            var jobs = new ObservableCollection<BillableModel>(_repository.GetBillableModelsForClientAndInvoice(clientAndInvoiceNavigationKey.ClientId, clientAndInvoiceNavigationKey.InvoiceId));

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
        }

        private ObservableCollection<int> _suggestedInvoiceReferences = new ObservableCollection<int>();
        public ObservableCollection<int> SuggestedInvoiceReferences
        {
            get { return _suggestedInvoiceReferences; }
            set
            {
                if (Equals(value, _suggestedInvoiceReferences)) return;
                _suggestedInvoiceReferences = value;
                NotifyOfPropertyChange(() => SuggestedInvoiceReferences);
            }
        }

        private const string VariousPropertyName = "Various";
        private const string ProfessionalFeesDescription = "Professional Fees";
        private const string InspectionAndPreparationOfPlansForAlterations = "Inspection And Preparation Of Plans For Alterations";

        private void SelectedItemOnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(InvoiceModel.CreationState))
            {
                NotifyOfPropertyChange(nameof(CanSetCompany));
                NotifyOfPropertyChange(nameof(CanSetBillableItems));
                NotifyOfPropertyChange(nameof(InvoiceEditable));

                // Set up some of the defaults from what's been selected.
                if (SelectedItem.IsNewInvoice && SelectedItem.CreationState == InvoiceCreationState.DefineInvoice)
                {
                    // Disable undo tracking while we handle automated changes.
                    SelectedItem.DisableUndoTracking = true;

                    // VAT rate is handled by the invoice model already.

                    // Offer the user some help in deciding a new invoice reference.
                    var suggestedInvoiceReferences = _repository.GetSuggestedInvoiceReferenceIdsForCompany(SelectedItem.Company);
                    SuggestedInvoiceReferences = new ObservableCollection<int>(suggestedInvoiceReferences);
                    SelectedItem.InvoiceReference = suggestedInvoiceReferences[suggestedInvoiceReferences.Length - 1];

                    // Set the property.
                    var selectedJobs =
                        Jobs.Where((m) => m.State != BillableModelState.Claimed && m.State != BillableModelState.Excluded)
                            .Cast<BillableJob>()
                            .ToArray();
                    if (selectedJobs.Length == 1)
                    {
                        SelectedItem.Project = selectedJobs[0].FullAddress;
                    }
                    else
                    {
                        SelectedItem.Project = VariousPropertyName;
                    }

                    // Set the description.
                    SelectedItem.Description = ProfessionalFeesDescription;

                    // Create a single invoice item.
                    UndoTracker.ResetChildren();
                    UndoTracker.AddChild();
                    SelectedItem.ChildrenList[0].NetAmount = NetAmount;
                    SelectedItem.ChildrenList[0].Description = InspectionAndPreparationOfPlansForAlterations;

                    // Re-enable undo tracking.
                    SelectedItem.DisableUndoTracking = false;
                }
            }
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            SuggestedInvoiceReferences = new ObservableCollection<int>();
            Jobs = new ObservableCollection<BillableModel>();
            SelectedItem.PropertyChanged -= SelectedItemOnPropertyChanged;
        }

        private InvoiceModel MakeNewModel(int clientId)
        {
            var clientModel = _repository.GetClientById(clientId);
            return new InvoiceModel()
            {
                ClientId = clientId,
                ClientName = clientModel.FullName,
                ClientAddressFirstLine = clientModel.AddressFirstLine,
                ClientAddressSecondLine = clientModel.AddressSecondLine,
                ClientAddressThirdLine = clientModel.AddressThirdLine,
                ClientAddressPostCode = clientModel.AddressPostCode,
                IsNewInvoice = true
            };
        }

        protected override InvoiceModel GetModel(ClientAndInvoiceNavigationKey clientAndInvoiceNavigationKey)
        {
            InvoiceModel invoiceModel;
            if (clientAndInvoiceNavigationKey.Equals(default(ClientAndInvoiceNavigationKey)) || clientAndInvoiceNavigationKey.InvoiceId == 0 || clientAndInvoiceNavigationKey.InvoiceId == ModelBase.NewModelId)
            {
                invoiceModel = MakeNewModel(clientAndInvoiceNavigationKey.ClientId);
                DisplayName = "New Invoice";
            }
            else
            {
                invoiceModel = _repository.GetInvoiceById(clientAndInvoiceNavigationKey.InvoiceId);
                if (invoiceModel.InvoiceId == ModelBase.NewModelId)
                {
                    invoiceModel = MakeNewModel(clientAndInvoiceNavigationKey.ClientId);
                    DisplayName = "New Invoice";
                }
                else
                    DisplayName = CompanyUtils.GetShortName(invoiceModel.Company) + " " + invoiceModel.InvoiceReference;
            }
            invoiceModel.Repository = _repository;
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return invoiceModel;
        }

        protected override bool ShouldEditOnNavigate(ClientAndInvoiceNavigationKey clientAndInvoiceNavigationKey)
        {
            return clientAndInvoiceNavigationKey.InvoiceId == ModelBase.NewModelId;
        }

        protected override ClientAndInvoiceNavigationKey GetLookupParameters()
        {
            return new ClientAndInvoiceNavigationKey(SelectedItem.ClientId, SelectedItem.InvoiceId);
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
                _repository.SaveInvoice(SelectedItem, Jobs.Cast<BillableJob>());
            }
            catch (Exception e)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public override void PerformDelete()
        {
            try
            {
                _repository.DeleteInvoice(SelectedItem);
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

        private double _netAmount;
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

        public ICommand AddItemCommand { get; }

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
