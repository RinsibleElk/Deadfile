using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Infrastructure.Converters;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Interfaces;
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
                ViewModel = new InvoicesPageViewModel(_tabIdentity, _printServiceMock.Object, _deadfileRepositoryMock.Object, _eventAggregatorMock.Object, _dialogCoordinatorMock.Object);
                RinsibleElk = MakeRinsibleElk();
                RinsibleElkInvoice = MakeRinsibleElkInvoiceModel();
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
                _deadfileRepositoryMock
                    .Setup((r) => r.GetClientById(model.ClientId))
                    .Returns(clientModel)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(model.ClientId, model.Id));
                VerifyAll();
            }

            public void NavigateToNew(ClientModel client)
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
                _deadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(client.ClientId))
                    .Returns(client)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(client.ClientId, ModelBase.NewModelId));
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
                l.Add(new BillableJob
                {
                    
                });
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
            }

            public void Dispose()
            {
                VerifyAll();
            }

            public void StartEditing()
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
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(_canUndoEvent)
                    .Verifiable();
                _editActionEvent.Publish(EditActionMessage.StartEditing);
            }

            public void SetBillablesForClient(int clientId, int invoiceId, IEnumerable<BillableJob> billables)
            {
                _deadfileRepositoryMock
                    .Setup((r) => r.GetBillableModelsForClientAndInvoice(clientId, invoiceId))
                    .Returns(billables)
                    .Verifiable();
            }
            public void SetBillables(Company company, int[] suggestedInvoiceReferences)
            {
                _deadfileRepositoryMock
                    .Setup((r) => r.HasUniqueInvoiceReference(ViewModel.SelectedItem, suggestedInvoiceReferences[0]))
                    .Returns(true)
                    .Verifiable();
                _deadfileRepositoryMock
                    .Setup((r) => r.GetSuggestedInvoiceReferenceIdsForCompany(company))
                    .Returns(suggestedInvoiceReferences)
                    .Verifiable();
                ViewModel.SetBillableItems();
            }

            public void SetAcceptableInvoiceReference(int invoiceReference)
            {
                _deadfileRepositoryMock
                    .Setup((r) => r.HasUniqueInvoiceReference(ViewModel.SelectedItem, invoiceReference))
                    .Returns(true)
                    .Verifiable();
                ViewModel.SelectedItem.InvoiceReferenceString = invoiceReference.ToString();
            }

            public void SaveAndEndEditing(int idToSet)
            {
                _deadfileRepositoryMock
                    .Setup((r) => r.SaveInvoice(ViewModel.SelectedItem, It.IsAny<IEnumerable<BillableJob>>()))
                    .Callback(() =>
                    {
                        if (ViewModel.SelectedItem.Id == ModelBase.NewModelId) ViewModel.SelectedItem.Id = idToSet;
                    })
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _saveEvent.Publish(SaveMessage.Save);
                _editActionEvent.Publish(EditActionMessage.EndEditing);
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
        public void TestNavigateToNewInvoice_SetBillables()
        {
            using (var host = new Host())
            {
                var billables = new List<BillableJob>();
                billables.Add(MakeBillableJob1());
                host.SetBillablesForClient(0, ModelBase.NewModelId, billables);
                host.NavigateToExisting(host.RinsibleElk, host.RinsibleElkInvoice);
                host.ViewModel.Jobs[0].State = BillableModelState.FullyIncluded;
                Assert.Equal(190.0, host.ViewModel.NetAmount);
            }
        }

        [Fact]
        public void TestCreateNewValidInvoice_CanSave()
        {
            using (var host = new Host())
            {
                var billables = new List<BillableJob>();
                billables.Add(MakeBillableJob1());
                host.SetBillablesForClient(0, ModelBase.NewModelId, billables);
                host.NavigateToNew(new ClientModel { ClientId = 0, FirstName = "Rinsible", LastName = "Elk", AddressFirstLine = "1 A Road", AddressPostCode = "N1 1AA" });
                host.StartEditing();
                Assert.True(host.ViewModel.CanSetBillableItems);
                Assert.False(host.ViewModel.InvoiceEditable);
                host.ViewModel.Jobs[0].State = BillableModelState.FullyIncluded;
                host.ViewModel.SelectedItem.Description = "Some description";
                Assert.Equal(0, host.ViewModel.SelectedItem.ChildrenList.Count);
                host.SetBillables(Company.Imagine3DLtd, new int[1] { 57 });
                Assert.Equal(1, host.ViewModel.SelectedItem.ChildrenList.Count);
                Assert.True(host.ViewModel.InvoiceEditable);
                host.ViewModel.SelectedItem.Description = "Hello world";
                host.ViewModel.SelectedItem.Project = "Various";
                // Until the child is given a description, cannot save.
                Assert.Equal(1, host.ViewModel.Errors.Count);
                Assert.False(host.ViewModel.CanSave);

                // Should receive a message saying that we can save when no errors.
                var stateMessages = new List<InvoicesPageState>();
//                host.PageStateEvent.Subscribe((message) => stateMessages.Add(message));
                host.ViewModel.SelectedItem.ChildrenList[0].Description = "Some description";
                // Nooooow we can save.
                Assert.Equal(0, host.ViewModel.Errors.Count);
                Assert.True(host.ViewModel.CanSave);
                Assert.Equal(1, stateMessages.Count);
                Assert.True(stateMessages[0].HasFlag(InvoicesPageState.CanSave));
            }
        }

        [Fact]
        public void TestSaveNewValidInvoice_CannotSave()
        {
            using (var host = new Host())
            {
                var li = new List<InvoicesPageState>();
//                host.PageStateEvent.Subscribe((message) => li.Add(message));
                var billables = new List<BillableJob>();
                billables.Add(MakeBillableJob1());
                host.SetBillablesForClient(0, ModelBase.NewModelId, billables);
                host.NavigateToNew(new ClientModel { ClientId = 0, FirstName = "Rinsible", LastName = "Elk", AddressFirstLine = "1 A Road", AddressPostCode = "N1 1AA" });
                host.StartEditing();
                Assert.True(host.ViewModel.CanSetBillableItems);
                Assert.False(host.ViewModel.InvoiceEditable);
                host.ViewModel.Jobs[0].State = BillableModelState.FullyIncluded;
                host.ViewModel.SelectedItem.Description = "Some description";
                Assert.Equal(0, host.ViewModel.SelectedItem.ChildrenList.Count);
                host.SetBillables(Company.Imagine3DLtd, new int[1] { 57 });
                Assert.Equal(1, host.ViewModel.SelectedItem.ChildrenList.Count);
                Assert.True(host.ViewModel.InvoiceEditable);
                host.ViewModel.SelectedItem.Description = "Hello world";
                host.ViewModel.SelectedItem.ChildrenList[0].Description = "Some description";
                host.ViewModel.SelectedItem.Project = "Various";
                Assert.Equal(0, host.ViewModel.Errors.Count);
                Assert.True(host.ViewModel.CanSave);
                host.SaveAndEndEditing(65);
                Assert.Equal(6, li.Count);
                Assert.False(li[5].HasFlag(InvoicesPageState.CanSave));
            }
        }

        private BillableJob MakeBillableJob1()
        {
            var billableJob = new BillableJob
            {
                FullAddress = "1 Some AddressFirstLine Road",
                JobId = 0,
                InvoiceId = ModelBase.NewModelId
            };
            var billableItem1 = new BillableExpense
            {
                InvoiceId = null,
                ExpenseId = 0,
                NetAmount = 100.0,
                State = BillableModelState.Excluded
            };
            billableJob.Children.Add(billableItem1);
            var billableItem2 = new BillableExpense
            {
                InvoiceId = null,
                ExpenseId = 1,
                Description = "Expense Description",
                NetAmount = 50.0,
                State = BillableModelState.Excluded
            };
            billableJob.Children.Add(billableItem2);
            var billableItem3 = new BillableBillableHour
            {
                InvoiceId = null,
                BillableHourId = 0,
                Description = "Billable Hour Description",
                NetAmount = 40.0,
                State = BillableModelState.Excluded
            };
            billableJob.Children.Add(billableItem3);
            var billableItem4 = new BillableBillableHour
            {
                InvoiceId = 27,
                BillableHourId = 1,
                Description = "Billable Hour Description",
                NetAmount = 35.0,
                State = BillableModelState.Claimed
            };
            billableJob.Children.Add(billableItem4);
            return billableJob;
        }
    }
}
