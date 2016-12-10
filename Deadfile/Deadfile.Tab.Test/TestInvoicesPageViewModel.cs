using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            private readonly bool _useRealEvents;
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<IDeadfileRepository> DeadfileRepositoryMock = new Mock<IDeadfileRepository>();
            public readonly Mock<IDialogCoordinator> DialogCoordinatorMock = new Mock<IDialogCoordinator>();
            public readonly Mock<LockedForEditingEvent> LockedForEditingMock = new Mock<LockedForEditingEvent>();
            public readonly Mock<CanUndoEvent> CanUndoEventMock = new Mock<CanUndoEvent>();
            public readonly Mock<DisplayNameEvent> DisplayNameEventMock = new Mock<DisplayNameEvent>();
            public readonly InvoicesPageViewModel ViewModel;

            public readonly UndoEvent UndoEvent = new UndoEvent();
            public readonly DeleteEvent DeleteEvent = new DeleteEvent();
            public readonly EditActionEvent EditActionEvent = new EditActionEvent();
            public readonly CanEditEvent CanEditEvent = new CanEditEvent();
            public readonly CanDeleteEvent CanDeleteEvent = new CanDeleteEvent();
            public readonly CanSaveEvent CanSaveEvent = new CanSaveEvent();
            public readonly SaveEvent SaveEvent = new SaveEvent();
            public readonly PrintEvent PrintEvent = new PrintEvent();
            public readonly DiscardChangesEvent DiscardChangesEvent = new DiscardChangesEvent();

            public readonly Mock<UndoEvent> UndoEventMock = new Mock<UndoEvent>();
            public readonly Mock<DeleteEvent> DeleteEventMock = new Mock<DeleteEvent>();
            public readonly Mock<EditActionEvent> EditActionEventMock = new Mock<EditActionEvent>();
            public readonly Mock<CanDeleteEvent> CanDeleteEventMock = new Mock<CanDeleteEvent>();
            public readonly Mock<CanEditEvent> CanEditEventMock = new Mock<CanEditEvent>();
            public readonly Mock<CanSaveEvent> CanSaveEventMock = new Mock<CanSaveEvent>();
            public readonly Mock<SaveEvent> SaveEventMock = new Mock<SaveEvent>();
            public readonly Mock<PrintEvent> PrintEventMock = new Mock<PrintEvent>();

            public Host(bool useRealEvents)
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

                _useRealEvents = useRealEvents;
                ViewModel = new InvoicesPageViewModel(DeadfileRepositoryMock.Object, EventAggregatorMock.Object, DialogCoordinatorMock.Object);
            }

            public void NavigateTo(InvoiceModel model)
            {
                if (_useRealEvents)
                {
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<UndoEvent>())
                        .Returns(UndoEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<DeleteEvent>())
                        .Returns(DeleteEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<EditActionEvent>())
                        .Returns(EditActionEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<PrintEvent>())
                        .Returns(PrintEvent)
                        .Verifiable();
                }
                else
                {
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<UndoEvent>())
                        .Returns(UndoEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<DeleteEvent>())
                        .Returns(DeleteEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<EditActionEvent>())
                        .Returns(EditActionEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<PrintEvent>())
                        .Returns(PrintEventMock.Object)
                        .Verifiable();
                }
                if (model.Id != ModelBase.NewModelId)
                {
                    if (_useRealEvents)
                    {
                        EventAggregatorMock
                            .Setup((ea) => ea.GetEvent<CanEditEvent>())
                            .Returns(CanEditEvent)
                            .Verifiable();
                        EventAggregatorMock
                            .Setup((ea) => ea.GetEvent<CanDeleteEvent>())
                            .Returns(CanDeleteEvent)
                            .Verifiable();
                    }
                    else
                    {
                        EventAggregatorMock
                            .Setup((ea) => ea.GetEvent<CanEditEvent>())
                            .Returns(CanEditEventMock.Object)
                            .Verifiable();
                        EventAggregatorMock
                            .Setup((ea) => ea.GetEvent<CanDeleteEvent>())
                            .Returns(CanDeleteEventMock.Object)
                            .Verifiable();
                    }
                    DeadfileRepositoryMock
                        .Setup((dr) => dr.GetInvoiceById(model.Id))
                        .Returns(model)
                        .Verifiable();
                }
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(DisplayNameEventMock.Object)
                    .Verifiable();
                DisplayNameEventMock
                    .Setup((ev) => ev.Publish(""))
                    .Verifiable();
                DeadfileRepositoryMock
                    .Setup((r) => r.GetClientById(model.ClientId))
                    .Returns(Clients[model.ClientId])
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientAndInvoiceNavigationKey(model.ClientId, model.Id));
                VerifyAll();
            }

            public void VerifyAll()
            {
            }

            public void Dispose()
            {
                VerifyAll();
            }

            public void SetBillablesForClient(int clientId, int invoiceId, IEnumerable<BillableJob> billables)
            {
                DeadfileRepositoryMock
                    .Setup((r) => r.GetBillableModelsForClientAndInvoice(clientId, invoiceId))
                    .Returns(billables)
                    .Verifiable();
            }
        }

        [Fact]
        public void TestCreation()
        {
            using (var host = new Host(true))
            {
            }
        }

        [Fact]
        public void TestNavigateToNewInvoice()
        {
            using (var host = new Host(true))
            {
                host.SetBillablesForClient(0, ModelBase.NewModelId, new BillableJob[0]);
                host.NavigateTo(new InvoiceModel() { ClientId = 0 });
            }
        }

        [Fact]
        public void TestNavigateToNewInvoice_SetBillables()
        {
            using (var host = new Host(true))
            {
                var billables = new List<BillableJob>();
                billables.Add(MakeBillableJob1());
                host.SetBillablesForClient(0, ModelBase.NewModelId, billables);
                host.NavigateTo(new InvoiceModel() {ClientId = 0});
                host.ViewModel.Jobs[0].State = BillableModelState.FullyIncluded;
                Assert.Equal(190.0, host.ViewModel.NetAmount);
            }
        }

        private BillableJob MakeBillableJob1()
        {
            var billableJob = new BillableJob
            {
                FullAddress = "1 Some Address Road",
                JobId = 0,
                InvoiceId = ModelBase.NewModelId
            };
            var billableItem1 = new BillableApplication
            {
                InvoiceId = null,
                ApplicationId = 0,
                LocalAuthorityReference = "INVOICEREF0",
                NetAmount = 100.0,
                State = BillableModelState.Excluded
            };
            billableJob.Children.Add(billableItem1);
            var billableItem2 = new BillableExpense
            {
                InvoiceId = null,
                ExpenseId = 0,
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
                BillableHourId = 0,
                Description = "Billable Hour Description",
                NetAmount = 35.0,
                State = BillableModelState.Claimed
            };
            billableJob.Children.Add(billableItem4);
            return billableJob;
        }
    }
}
