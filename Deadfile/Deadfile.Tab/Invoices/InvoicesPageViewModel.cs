using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Utils;
using Deadfile.Pdf;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using PdfSharp.Drawing;
using Prism.Commands;
using Prism.Events;

namespace Deadfile.Tab.Invoices
{
    class InvoicesPageViewModel : EditableItemViewModel<ClientAndInvoiceNavigationKey, InvoiceModel>, IInvoicesPageViewModel, IBillableModelContainer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IPrintService _printService;
        private readonly IDeadfileRepository _repository;

        public InvoicesPageViewModel(TabIdentity tabIdentity,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator) : base(tabIdentity, eventAggregator, dialogCoordinator, new ParentUndoTracker<InvoiceModel, InvoiceItemModel>())
        {
            _tabIdentity = tabIdentity;
            _printService = printService;
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

            _printEventSubscriptionToken = EventAggregator.GetEvent<PrintEvent>().Subscribe(PerformPrint);
            _paidEventSubscriptionToken = EventAggregator.GetEvent<PaidEvent>().Subscribe(PerformPaid);

            SuggestedInvoiceReferences = new ObservableCollection<string>(new string[] {SelectedItem.InvoiceReferenceString});

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

        private void PerformPaid(PaidMessage message)
        {
            SelectedItem.Status = InvoiceStatus.Paid;
            _repository.SaveInvoice(SelectedItem, Jobs.Cast<BillableJob>());
        }

        private ObservableCollection<string> _suggestedInvoiceReferences = new ObservableCollection<string>();
        public ObservableCollection<string> SuggestedInvoiceReferences
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

        private void SelectedItemOnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(InvoiceModel.CreationState))
            {
                NotifyOfPropertyChange(nameof(CanSetBillableItems));
                NotifyOfPropertyChange(nameof(InvoiceEditable));

                // Set up some of the defaults from what's been selected.
                if (SelectedItem.IsNewInvoice && SelectedItem.CreationState == InvoiceCreationState.DefineInvoice)
                {
                    // Disable undo tracking while we handle automated changes.
                    SelectedItem.DisableUndoTracking = true;

                    // VAT rate is handled by the invoice model already.

                    // Offer the user some help in deciding a new invoice reference.
                    var suggestedInvoiceReferences =
                        _repository.GetSuggestedInvoiceReferenceIdsForCompany(SelectedItem.Company)
                            .Select((s) => s.ToString())
                            .ToArray();
                    SuggestedInvoiceReferences = new ObservableCollection<string>(suggestedInvoiceReferences);
                    SelectedItem.InvoiceReferenceString = suggestedInvoiceReferences[suggestedInvoiceReferences.Length - 1];

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

                    // Set the description. This is not displayed for Imagine3D invoices.
                    SelectedItem.Description = null;

                    // Create a single invoice item.
                    UndoTracker.ResetChildren();
                    UndoTracker.AddChild();
                    SelectedItem.ChildrenList[0].NetAmount = NetAmount;
                    SelectedItem.ChildrenList[0].Description = null;

                    // Re-enable undo tracking.
                    SelectedItem.DisableUndoTracking = false;
                }
            }
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            EventAggregator.GetEvent<PrintEvent>().Unsubscribe(_printEventSubscriptionToken);
            _printEventSubscriptionToken = null;
            EventAggregator.GetEvent<PaidEvent>().Unsubscribe(_paidEventSubscriptionToken);
            _paidEventSubscriptionToken = null;
            SuggestedInvoiceReferences = new ObservableCollection<string>();
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
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
            NotifyOfPropertyChange(nameof(InvoiceEditable));
        }

        public bool CanSetBillableItems
        {
            get { return Editable && SelectedItem.CreationState == InvoiceCreationState.DefineBillables; }
        }

        public bool InvoiceEditable
        {
            get { return Editable && SelectedItem.CreationState == InvoiceCreationState.DefineInvoice; }
        }

        protected override void PerformSave(SaveMessage message)
        {
            try
            {
                _repository.SaveInvoice(SelectedItem, Jobs.Cast<BillableJob>());
                if (message == SaveMessage.SaveAndPrint)
                {
                    PerformPrint(PrintMessage.Print);
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while saving {0}, {1}, {2}, {3}, {4}", _tabIdentity, SelectedItem, message, e, e.StackTrace);
                throw;
            }
        }

        protected override bool MayDelete(out string details)
        {
            details = null;
            return true;
        }

        private void PerformPrint(PrintMessage print)
        {
            var invoiceGenerator = new CompanySwitchingInvoiceGenerator();
            var fixedDocument = invoiceGenerator.GenerateDocument(SelectedItem);
            _printService.PrintDocument(fixedDocument);
        }

        protected override void PerformDelete()
        {
            try
            {
                SelectedItem.Status = InvoiceStatus.Cancelled;
                PerformSave(SaveMessage.Save);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception while deleting {0}, {1}, {2}, {3}", _tabIdentity, SelectedItem, e,
                    e.StackTrace);
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
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
        }

        public void SetBillableItems()
        {
            SelectedItem.CreationState = InvoiceCreationState.DefineInvoice;
            NotifyOfPropertyChange(nameof(CanSetBillableItems));
            NotifyOfPropertyChange(nameof(InvoiceEditable));
        }

        private double _netAmount;
        private SubscriptionToken _printEventSubscriptionToken = null;
        private SubscriptionToken _paidEventSubscriptionToken = null;

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
