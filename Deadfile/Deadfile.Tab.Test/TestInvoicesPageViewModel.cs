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
            public readonly ClientModel[] Clients;

            public readonly TabIdentity TabIdentity = new TabIdentity(1);
            public readonly Mock<IPrintService> PrintServiceMock = new Mock<IPrintService>();
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<IDeadfileRepository> DeadfileRepositoryMock = new Mock<IDeadfileRepository>();
            public readonly Mock<IDeadfileDialogCoordinator> DialogCoordinatorMock = new Mock<IDeadfileDialogCoordinator>();
            public readonly InvoicesPageViewModel ViewModel;

            private readonly UndoEvent _undoEvent = new UndoEvent();
            private readonly DeleteEvent _deleteEvent = new DeleteEvent();
            private readonly EditActionEvent _editActionEvent = new EditActionEvent();
            private readonly DisplayNameEvent _displayNameEvent = new DisplayNameEvent();
            public readonly PageStateEvent<InvoicesPageState> PageStateEvent = new PageStateEvent<InvoicesPageState>();
            private readonly SaveEvent _saveEvent = new SaveEvent();
            private readonly PrintEvent _printEvent = new PrintEvent();
            private readonly PaidEvent _paidEvent = new PaidEvent();
            private readonly DiscardChangesEvent _discardChangesEvent = new DiscardChangesEvent();
            private readonly LockedForEditingEvent _lockedForEditingEvent = new LockedForEditingEvent();
            private readonly CanUndoEvent _canUndoEvent = new CanUndoEvent();

            public Host()
            {
                var clients = new List<ClientModel>();
                clients.Add(new ClientModel()
                {
                    ClientId = 0,
                    Title = "Herr",
                    FirstName = "Herman",
                    LastName = "German",
                    AddressFirstLine = "1 Yemen Road",
                    AddressSecondLine = "Yemen",
                    AddressPostCode = "AB1 2CD",
                    PhoneNumber1 = "01234567890"
                });
                Clients = clients.ToArray();

                ViewModel = new InvoicesPageViewModel(TabIdentity, PrintServiceMock.Object, DeadfileRepositoryMock.Object, EventAggregatorMock.Object, DialogCoordinatorMock.Object);
            }

            public void NavigateTo(InvoiceModel model)
            {
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<UndoEvent>())
                        .Returns(_undoEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<DeleteEvent>())
                        .Returns(_deleteEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<EditActionEvent>())
                        .Returns(_editActionEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<PrintEvent>())
                        .Returns(_printEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<PaidEvent>())
                        .Returns(_paidEvent)
                        .Verifiable();
                if (model.Id != ModelBase.NewModelId)
                {
                        EventAggregatorMock
                            .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                            .Returns(PageStateEvent)
                            .Verifiable();
                    DeadfileRepositoryMock
                        .Setup((dr) => dr.GetInvoiceById(model.Id))
                        .Returns(model)
                        .Verifiable();
                }
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                DeadfileRepositoryMock
                    .Setup((r) => r.GetClientById(model.ClientId))
                    .Returns(Clients[model.ClientId])
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(model.ClientId, model.Id));
                VerifyAll();
            }

            public void NavigateToNew(ClientModel client)
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(_undoEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DeleteEvent>())
                    .Returns(_deleteEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PrintEvent>())
                    .Returns(_printEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PaidEvent>())
                    .Returns(_paidEvent)
                    .Verifiable();
                DeadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(client.ClientId))
                    .Returns(client)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(client.ClientId, ModelBase.NewModelId));
                VerifyAll();
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
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(_saveEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(PageStateEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(_canUndoEvent)
                    .Verifiable();
                _editActionEvent.Publish(EditActionMessage.StartEditing);
            }

            public void SetBillablesForClient(int clientId, int invoiceId, IEnumerable<BillableJob> billables)
            {
                DeadfileRepositoryMock
                    .Setup((r) => r.GetBillableModelsForClientAndInvoice(clientId, invoiceId))
                    .Returns(billables)
                    .Verifiable();
            }
            public void SetBillables(Company company, int[] suggestedInvoiceReferences)
            {
                DeadfileRepositoryMock
                    .Setup((r) => r.HasUniqueInvoiceReference(ViewModel.SelectedItem, suggestedInvoiceReferences[0]))
                    .Returns(true)
                    .Verifiable();
                DeadfileRepositoryMock
                    .Setup((r) => r.GetSuggestedInvoiceReferenceIdsForCompany(company))
                    .Returns(suggestedInvoiceReferences)
                    .Verifiable();
                ViewModel.SetBillableItems();
            }

            public void SetAcceptableInvoiceReference(int invoiceReference)
            {
                DeadfileRepositoryMock
                    .Setup((r) => r.HasUniqueInvoiceReference(ViewModel.SelectedItem, invoiceReference))
                    .Returns(true)
                    .Verifiable();
                ViewModel.SelectedItem.InvoiceReferenceString = invoiceReference.ToString();
            }

            public void SaveAndEndEditing(int idToSet)
            {
                DeadfileRepositoryMock
                    .Setup((r) => r.SaveInvoice(ViewModel.SelectedItem, It.IsAny<IEnumerable<BillableJob>>()))
                    .Callback(() =>
                    {
                        if (ViewModel.SelectedItem.Id == ModelBase.NewModelId) ViewModel.SelectedItem.Id = idToSet;
                    })
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(PageStateEvent)
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
        public void TestNavigateToNewInvoice()
        {
            using (var host = new Host())
            {
                host.SetBillablesForClient(0, ModelBase.NewModelId, new BillableJob[0]);
                host.NavigateTo(new InvoiceModel() { ClientId = 0 });
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
                host.NavigateTo(new InvoiceModel() {ClientId = 0});
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
                host.PageStateEvent.Subscribe((message) => stateMessages.Add(message));
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
                host.PageStateEvent.Subscribe((message) => li.Add(message));
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
