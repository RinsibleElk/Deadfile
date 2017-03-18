using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Deadfile.Entity;
using Deadfile.Infrastructure.Converters;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Utils;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.Invoices;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestInvoicesPageViewModel
    {
        private class Host : IDisposable
        {
            private readonly TabIdentity _tabIdentity = new TabIdentity(1);
            private InvoicesPageState _pageState = 0;
            private readonly Mock<IPrintService> _printServiceMock = new Mock<IPrintService>();
            private readonly Mock<IEventAggregator> _eventAggregatorMock = new Mock<IEventAggregator>();
            private readonly Mock<IDeadfileRepository> _deadfileRepositoryMock = new Mock<IDeadfileRepository>();
            private readonly Mock<IDeadfileDialogCoordinator> _dialogCoordinatorMock = new Mock<IDeadfileDialogCoordinator>();

            public readonly InvoicesPageViewModel ViewModel;
            public readonly ClientModel RinsibleElk;
            public readonly InvoiceModel RinsibleElkInvoice;

            private readonly UndoEvent _undoEvent = new UndoEvent();
            private readonly DeleteEvent _deleteEvent = new DeleteEvent();
            private readonly EditActionEvent _editActionEvent = new EditActionEvent();
            private readonly List<string> _receivedDisplayNameMessages = new List<string>();
            private readonly DisplayNameEvent _displayNameEvent = new DisplayNameEvent();
            private readonly PageStateEvent<InvoicesPageState> _pageStateEvent = new PageStateEvent<InvoicesPageState>();
            private readonly SaveEvent _saveEvent = new SaveEvent();
            private readonly PrintEvent _printEvent = new PrintEvent();
            private readonly PaidEvent _paidEvent = new PaidEvent();
            private readonly DiscardChangesEvent _discardChangesEvent = new DiscardChangesEvent();
            private readonly LockedForEditingEvent _lockedForEditingEvent = new LockedForEditingEvent();
            private readonly CanUndoEvent _canUndoEvent = new CanUndoEvent();

            public Host()
            {
                ViewModel = new InvoicesPageViewModel(_tabIdentity, _printServiceMock.Object,
                    _deadfileRepositoryMock.Object, _eventAggregatorMock.Object, _dialogCoordinatorMock.Object);
                RinsibleElk = MakeRinsibleElk();
                RinsibleElkInvoice = MakeRinsibleElkInvoiceModel();
                _pageStateEvent.Subscribe((s) => _pageState = s);
                _displayNameEvent.Subscribe((n) => _receivedDisplayNameMessages.Add(n));
            }

            public void NavigateToExisting(ClientModel clientModel, InvoiceModel model)
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(_undoEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DeleteEvent>())
                    .Returns(_deleteEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PrintEvent>())
                    .Returns(_printEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PaidEvent>())
                    .Returns(_paidEvent)
                    .Verifiable();
                if (model.Id != ModelBase.NewModelId)
                {
                    _eventAggregatorMock
                        .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                        .Returns(_pageStateEvent)
                        .Verifiable();
                    _deadfileRepositoryMock
                        .Setup((dr) => dr.GetInvoiceById(model.Id))
                        .Returns(model)
                        .Verifiable();
                }
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(model.ClientId, model.Id));
                ViewModel.CompleteNavigation();
                Assert.Equal(InvoicesPageState.CanDelete | InvoicesPageState.CanEdit, _pageState);
                Assert.Equal(InvoicesPageState.CanDelete | InvoicesPageState.CanEdit, ViewModel.State);
                Assert.Equal(1, _receivedDisplayNameMessages.Count);
                Assert.Equal(CompanyUtils.GetShortName(model.Company) + " " + model.InvoiceReference, _receivedDisplayNameMessages[0]);
                Assert.Equal(Experience.Invoices, ViewModel.Experience);
                Assert.False(ViewModel.UnderEdit);
                _receivedDisplayNameMessages.Clear();
                VerifyAll();
            }

            private ClientModel MakeRinsibleElk()
            {
                return new ClientModel
                {
                    ClientId = 116,
                    AddressFirstLine = "1 Dummy Road",
                    AddressSecondLine = "London",
                    AddressPostCode = "N1 1AA",
                    Company = "RinsibleElk",
                    EmailAddress = "rinsible.elk@gmail.com",
                    Title = "Sir",
                    FirstName = "Rinsible",
                    LastName = "Elk",
                    PhoneNumber1 = "07193265784",
                    Status = ClientStatus.Active,
                    Id = 116
                };
            }

            public void RegisterRinsibleElkBillables()
            {
                var l = new List<BillableModel>();
                var job1Expense1 = new BillableExpense
                {
                    NetAmount = 100,
                    Description = "Expense 1 for 1 Dummy Job Address",
                    State = BillableModelState.FullyIncluded,
                    ExpenseId = 1451,
                    InvoiceId = 115
                };
                var job1Expense2 = new BillableExpense
                {
                    NetAmount = 200,
                    Description = "Expense 2 for 1 Dummy Job Address",
                    State = BillableModelState.Claimed,
                    ExpenseId = 1452,
                    InvoiceId = 65
                };
                var job1Expense3 = new BillableExpense
                {
                    NetAmount = 300,
                    Description = "Expense 3 for 1 Dummy Job Address",
                    State = BillableModelState.Excluded,
                    ExpenseId = 1453,
                    InvoiceId = null
                };
                var job1Hours1 = new BillableBillableHour
                {
                    Hours = 5,
                    Description = "Hours 1 for 1 Dummy Job Address",
                    State = BillableModelState.Excluded,
                    BillableHourId = 1451,
                    InvoiceId = null
                };
                var job1Hours2 = new BillableExpense
                {
                    Hours = 2,
                    Description = "Hours 2 for 1 Dummy Job Address",
                    State = BillableModelState.FullyIncluded,
                    ExpenseId = 1452,
                    InvoiceId = 115
                };
                var job1 = new BillableJob
                {
                    FullAddress = "1 Dummy Job Address",
                    JobId = 145,
                    TotalPossibleHours = 7,
                    Hours = 2,
                    TotalPossibleNetAmount = 400,
                    NetAmount = 100,
                    State = BillableModelState.PartiallyIncluded
                };
                job1.Children.Add(job1Expense1);
                job1.Children.Add(job1Expense2);
                job1.Children.Add(job1Expense3);
                job1.Children.Add(job1Hours1);
                job1.Children.Add(job1Hours2);
                var job2 = new BillableJob {FullAddress = "2 Dummy Job Address", JobId = 156};
                l.Add(job1);
                l.Add(job2);
                _deadfileRepositoryMock
                    .Setup((r) => r.GetBillableModelsForClientAndInvoice(116, 115))
                    .Returns(l)
                    .Verifiable();
            }

            private InvoiceModel MakeRinsibleElkInvoiceModel()
            {
                var model = new InvoiceModel
                {
                    ClientId = 116,
                    InvoiceId = 115,
                    Project = "1 Dummy Project Road",
                    ClientAddressFirstLine = "1 Dummy Road",
                    ClientAddressSecondLine = "London",
                    ClientAddressPostCode = "N1 1AA",
                    ClientName = "Rinsible Elk",
                    CreatedDate = new DateTime(2016, 12, 17),
                    Description = "Various work",
                    ChildrenList = new List<InvoiceItemModel>(),
                    NetAmount = 700,
                    Company = Company.Imagine3DLtd,
                    GrossAmount = 700,
                    VatRate = 0,
                    VatValue = 0,
                    InvoiceReference = 52,
                    InvoiceReferenceString = "52",
                    Status = InvoiceStatus.Created,
                    Repository = _deadfileRepositoryMock.Object
                };
                model.ChildrenList.Add(MakeRinsibleElkInvoiceItem());
                model.ChildrenUpdated();
                return model;
            }

            private InvoiceItemModel MakeRinsibleElkInvoiceItem()
            {
                return new InvoiceItemModel
                {
                    Context = 0,
                    InvoiceId = 115,
                    Description = "Some work what we done",
                    InvoiceItemId = 165,
                    NetAmount = 700,
                    ParentId = 115
                };
            }

            public void VerifyAll()
            {
                _eventAggregatorMock.VerifyAll();
                _deadfileRepositoryMock.VerifyAll();
                _printServiceMock.VerifyAll();
                _dialogCoordinatorMock.VerifyAll();
                Assert.Empty(_receivedDisplayNameMessages);
            }

            public void Dispose()
            {
                VerifyAll();
            }

            public void NavigateAway()
            {
                ViewModel.OnNavigatedFrom();
                VerifyAll();
            }
        }

        [Fact]
        public void TestCreation()
        {
            using (var host = new Host())
            {
            }
        }

        [Fact]
        public void TestNavigateToExistingInvoice()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
            }
        }

        [Fact]
        public void TestNavigateAwayFromExistingInvoice()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.NavigateAway();
            }
        }
    }
}

