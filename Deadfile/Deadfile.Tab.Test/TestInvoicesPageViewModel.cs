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

            private readonly List<CanUndoMessage> _receivedCanUndoMessages = new List<CanUndoMessage>();
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
            private readonly List<LockedForEditingMessage> _receivedLockedForEditingMessages = new List<LockedForEditingMessage>();
            private readonly LockedForEditingEvent _lockedForEditingEvent = new LockedForEditingEvent();
            private readonly CanUndoEvent _canUndoEvent = new CanUndoEvent();
            private readonly List<NavigateFallBackMessage> _receivedNavigateFallBackMessages = new List<NavigateFallBackMessage>();
            private readonly NavigateFallBackEvent _navigateFallBackEvent = new NavigateFallBackEvent();

            public Host()
            {
                ViewModel = new InvoicesPageViewModel(_tabIdentity, _printServiceMock.Object,
                    _deadfileRepositoryMock.Object, _eventAggregatorMock.Object, _dialogCoordinatorMock.Object);
                RinsibleElk = MakeRinsibleElk();
                RinsibleElkInvoice = MakeRinsibleElkInvoiceModel();
                _pageStateEvent.Subscribe((s) => _pageState = s);
                _displayNameEvent.Subscribe((n) => _receivedDisplayNameMessages.Add(n));
                _lockedForEditingEvent.Subscribe((m) => _receivedLockedForEditingMessages.Add(m));
                _canUndoEvent.Subscribe((m) => _receivedCanUndoMessages.Add(m));
                _navigateFallBackEvent.Subscribe((m) => _receivedNavigateFallBackMessages.Add(m));
            }

            public void NavigateToNew(ClientModel clientModel)
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
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                _deadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(clientModel.Id))
                    .Returns(clientModel)
                    .Verifiable();
                if (clientModel == RinsibleElk)
                {
                    RegisterRinsibleElkBillables(true);
                }
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(clientModel.ClientId, ModelBase.NewModelId));
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(_saveEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                ViewModel.CompleteNavigation();
                Assert.Equal(InvoicesPageState.CanDiscard | InvoicesPageState.UnderEdit, _pageState);
                Assert.Equal(InvoicesPageState.CanDiscard | InvoicesPageState.UnderEdit, ViewModel.State);
                Assert.Equal(1, _receivedDisplayNameMessages.Count);
                Assert.Equal("New Invoice", _receivedDisplayNameMessages[0]);
                _receivedDisplayNameMessages.Clear();
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.True(_receivedLockedForEditingMessages[0].IsLocked);
                _receivedLockedForEditingMessages.Clear();
                Assert.Equal(Experience.Invoices, ViewModel.Experience);
                Assert.True(ViewModel.UnderEdit);
                Assert.False(ViewModel.CanDelete);
                VerifyAll();
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
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _deadfileRepositoryMock
                    .Setup((dr) => dr.GetInvoiceById(model.Id))
                    .Returns(model)
                    .Verifiable();
                if (model == RinsibleElkInvoice)
                {
                    RegisterRinsibleElkBillables(false);
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
                Assert.True(ViewModel.CanDelete);
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

            private void RegisterRinsibleElkBillables(bool isNew)
            {
                var l = new List<BillableModel>();
                var job1Expense1 = new BillableExpense
                {
                    NetAmount = 100,
                    Description = "Expense 1 for 1 Dummy Job Address",
                    State = (isNew ? BillableModelState.Claimed : BillableModelState.FullyIncluded),
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
                    State = (isNew ? BillableModelState.Claimed : BillableModelState.FullyIncluded),
                    ExpenseId = 1452,
                    InvoiceId = 115
                };
                var job1 = new BillableJob
                {
                    FullAddress = "1 Dummy Job Address",
                    JobId = 145,
                    TotalPossibleHours = (isNew ? 5 : 7),
                    Hours = (isNew ? 0 : 2),
                    TotalPossibleNetAmount = (isNew ? 300 : 400),
                    NetAmount = (isNew ? 0 : 100),
                    State = (isNew ? BillableModelState.Excluded : BillableModelState.PartiallyIncluded)
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
                    .Setup((r) => r.GetBillableModelsForClientAndInvoice(116, (isNew ? ModelBase.NewModelId : 115)))
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
                Assert.Empty(_receivedLockedForEditingMessages);
                Assert.Empty(_receivedCanUndoMessages);
                Assert.Empty(_receivedNavigateFallBackMessages);
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

            public void Edit()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(_saveEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                _deadfileRepositoryMock
                    .Setup((dr) => dr.HasUniqueInvoiceReference(ViewModel.SelectedItem, ViewModel.SelectedItem.InvoiceReference))
                    .Returns(true)
                    .Verifiable();
                _editActionEvent.Publish(EditActionMessage.StartEditing);
                Assert.True(ViewModel.CanSave);
                Assert.True(ViewModel.CanEdit);
                Assert.True(ViewModel.State.HasFlag(InvoicesPageState.CanDiscard));
                Assert.True(ViewModel.UnderEdit);
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.True(_receivedLockedForEditingMessages[0].IsLocked);
                Assert.False(ViewModel.InvoiceEditable);
                Assert.True(ViewModel.Editable);
                Assert.True(ViewModel.CanSetBillableItems);
                Assert.Equal(InvoiceCreationState.DefineBillables, ViewModel.SelectedItem.CreationState);
                _receivedLockedForEditingMessages.Clear();
                VerifyAll();
            }

            public void Discard(ClientAndInvoiceNavigationKey? navigationKey)
            {
                _discardChangesEvent.Publish(DiscardChangesMessage.Discard);
                _editActionEvent.Publish(EditActionMessage.EndEditing);
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.False(_receivedLockedForEditingMessages[0].IsLocked);
                if (navigationKey != null)
                {
                    var key = (ClientAndInvoiceNavigationKey)_receivedLockedForEditingMessages[0].NewParameters;
                    Assert.Equal(navigationKey, key);
                }
                else
                {
                    Assert.Equal(null, _receivedLockedForEditingMessages[0].NewParameters);
                }
                _receivedLockedForEditingMessages.Clear();
                VerifyAll();
            }

            public void SetBillableItems(bool registerCanUndo)
            {
                if (registerCanUndo)
                {
                    _eventAggregatorMock
                        .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                        .Returns(_canUndoEvent)
                        .Verifiable();
                }
                ViewModel.SetBillableItems();
                if (registerCanUndo)
                {
                    Assert.Equal(1, _receivedCanUndoMessages.Count);
                    Assert.Equal(CanUndoMessage.CanUndo, _receivedCanUndoMessages[0]);
                    _receivedCanUndoMessages.Clear();
                }
                Assert.Equal(InvoiceCreationState.DefineInvoice, ViewModel.SelectedItem.CreationState);
                VerifyAll();
            }

            public void Undo(CanUndoMessage[] expectedCanUndoMessages)
            {
                _undoEvent.Publish(UndoMessage.Undo);
                Assert.Equal(expectedCanUndoMessages.Length, _receivedCanUndoMessages.Count);
                for (int i = 0; i < expectedCanUndoMessages.Length; i++)
                {
                    Assert.Equal(expectedCanUndoMessages[i], _receivedCanUndoMessages[i]);
                }
                _receivedCanUndoMessages.Clear();
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

        [Fact]
        public void TestStartEditingExistingInvoice()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
            }
        }

        [Fact]
        public void TestDiscardExistingInvoice()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.Discard(new ClientAndInvoiceNavigationKey(host.RinsibleElk.ClientId, host.RinsibleElkInvoice.InvoiceId));
            }
        }

        [Fact]
        public void TestStartEditingExistingInvoice_SetBillableItems()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.SetBillableItems(true);
            }
        }

        [Fact]
        public void TestStartEditingExistingInvoice_SetBillableItems_Undo()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.SetBillableItems(true);
                host.Undo(new CanUndoMessage[2] { CanUndoMessage.CannotUndo, CanUndoMessage.CanRedo });
            }
        }

        [Fact]
        public void TestStartEditingExistingInvoice_SetBillableItems_SetCompany_Undo()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.SetBillableItems(true);
                host.ViewModel.SelectedItem.Company = Company.PaulSamsonCharteredSurveyorLtd;
                host.Undo(new CanUndoMessage[1] {CanUndoMessage.CanRedo});
                Assert.Equal(Company.Imagine3DLtd, host.ViewModel.SelectedItem.Company);
                Assert.Equal(InvoiceCreationState.DefineInvoice, host.ViewModel.SelectedItem.CreationState);
            }
        }

        [Theory]
        [InlineData(nameof(InvoiceModel.InvoiceReferenceString), "abcde")]
        [InlineData(nameof(InvoiceModel.ClientName), "")]
        // Over 100 characters long
        [InlineData(nameof(InvoiceModel.ClientName), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        [InlineData(nameof(InvoiceModel.Project), "")]
        // Over 200 characters long
        [InlineData(nameof(InvoiceModel.Project), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        [InlineData(nameof(InvoiceModel.Description), "")]
        // Over 200 characters long
        [InlineData(nameof(InvoiceModel.Description), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        [InlineData(nameof(InvoiceModel.ClientAddressFirstLine), "")]
        // Over 200 characters long
        [InlineData(nameof(InvoiceModel.ClientAddressFirstLine), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        [InlineData(nameof(InvoiceModel.ClientAddressSecondLine), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        [InlineData(nameof(InvoiceModel.ClientAddressThirdLine), "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde")]
        // Over 20 characters long
        [InlineData(nameof(InvoiceModel.ClientAddressPostCode), "abcdefabcdefabcdefabcdefabcdef")]
        public void TestEditSetInvalidValue_CantSave(string propertyName, object value)
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.SetBillableItems(true);
                typeof(InvoiceModel).GetProperty(propertyName).SetValue(host.ViewModel.SelectedItem, value);
                Assert.False(host.ViewModel.CanSave);
            }
        }

        [Theory]
        [InlineData(nameof(InvoiceModel.ClientName), "Mickey Mouse")]
        [InlineData(nameof(InvoiceModel.Project), "4 Privet Drive")]
        [InlineData(nameof(InvoiceModel.Description), "Some work that was done")]
        [InlineData(nameof(InvoiceModel.ClientAddressFirstLine), "4 Privet Drive")]
        [InlineData(nameof(InvoiceModel.ClientAddressSecondLine), "Berkshire")]
        [InlineData(nameof(InvoiceModel.ClientAddressThirdLine), "London")]
        [InlineData(nameof(InvoiceModel.ClientAddressPostCode), "N1 1AA")]
        public void TestEditSetValidValue_CanSave(string propertyName, object value)
        {
            using (var host = new Host())
            {
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.Edit();
                host.SetBillableItems(true);
                typeof(InvoiceModel).GetProperty(propertyName).SetValue(host.ViewModel.SelectedItem, value);
                Assert.True(host.ViewModel.CanSave);
            }
        }

        [Fact]
        public void TestAddNewInvoice()
        {
            using (var host = new Host())
            {
                host.NavigateToNew(host.RinsibleElk);
            }
        }
    }
}

