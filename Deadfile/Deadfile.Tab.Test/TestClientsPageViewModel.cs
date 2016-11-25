using System;
using System.Linq;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestClientsPageViewModel
    {
        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<IDeadfileRepository> DeadfileRepositoryMock = new Mock<IDeadfileRepository>();
            public readonly Mock<LockedForEditingEvent> LockedForEditingMock = new Mock<LockedForEditingEvent>();
            public readonly Mock<CanUndoEvent> CanUndoEventMock = new Mock<CanUndoEvent>();
            public readonly Mock<DisplayNameEvent> DisplayNameEventMock = new Mock<DisplayNameEvent>();
            public readonly ClientsPageViewModel ViewModel;

            public readonly UndoEvent UndoEvent = new UndoEvent();
            public readonly DeleteEvent DeleteEvent = new DeleteEvent();
            public readonly EditActionEvent EditActionEvent = new EditActionEvent();
            public readonly CanEditEvent CanEditEvent = new CanEditEvent();
            public readonly CanDeleteEvent CanDeleteEvent = new CanDeleteEvent();
            public readonly CanSaveEvent CanSaveEvent = new CanSaveEvent();
            public readonly SaveEvent SaveEvent = new SaveEvent();
            public readonly DiscardChangesEvent DiscardChangesEvent = new DiscardChangesEvent();

            public readonly Mock<UndoEvent> UndoEventMock = new Mock<UndoEvent>();
            public readonly Mock<DeleteEvent> DeleteEventMock = new Mock<DeleteEvent>();
            public readonly Mock<EditActionEvent> EditActionEventMock = new Mock<EditActionEvent>();
            public readonly Mock<CanDeleteEvent> CanDeleteEventMock = new Mock<CanDeleteEvent>();
            public readonly Mock<CanEditEvent> CanEditEventMock = new Mock<CanEditEvent>();
            public readonly Mock<CanSaveEvent> CanSaveEventMock = new Mock<CanSaveEvent>();
            public readonly Mock<SaveEvent> SaveEventMock = new Mock<SaveEvent>();

            public readonly SubscriptionToken EditActionEventSubscriptionToken = new SubscriptionToken((a) => { });
            public readonly SubscriptionToken UndoEventSubscriptionToken = new SubscriptionToken((a) => { });
            public readonly SubscriptionToken DeleteEventSubscriptionToken = new SubscriptionToken((a) => { });

            public readonly Mock<IDialogCoordinator> DialogCoordinatorMock = new Mock<IDialogCoordinator>();

            private readonly bool _useRealEvents;

            public Host(bool useRealEvents)
            {
                _useRealEvents = useRealEvents;
                ViewModel = new ClientsPageViewModel(EventAggregatorMock.Object, DeadfileRepositoryMock.Object, DialogCoordinatorMock.Object);
            }

            public void NavigateTo(ClientModel model)
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
                        .Setup((dr) => dr.GetClientById(model.Id))
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
                ViewModel.OnNavigatedTo(model.Id);
                VerifyAll();
            }

            public void NavigateFrom()
            {
                if (!_useRealEvents)
                {
                    EventAggregatorMock
                        .Setup((ev) => ev.GetEvent<UndoEvent>())
                        .Returns(UndoEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ev) => ev.GetEvent<DeleteEvent>())
                        .Returns(DeleteEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ev) => ev.GetEvent<EditActionEvent>())
                        .Returns(EditActionEventMock.Object)
                        .Verifiable();
                }
                ViewModel.OnNavigatedFrom();
                VerifyAll();
            }

            public void StartEditing()
            {
                // He'll subscribe to the save event and discard changes event.
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(SaveEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(DiscardChangesEvent)
                    .Verifiable();
                // And he'll publish that we're locked for editing.
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingMock.Object)
                    .Verifiable();
                LockedForEditingMock
                    .Setup((ev) => ev.Publish(LockedForEditingMessage.Locked))
                    .Verifiable();
                // And he'll publish that it is allowed to save.
                if (_useRealEvents)
                {
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                        .Returns(CanSaveEvent)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<CanDeleteEvent>())
                        .Returns(CanDeleteEvent)
                        .Verifiable();
                }
                else
                {
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<CanDeleteEvent>())
                        .Returns(CanDeleteEventMock.Object)
                        .Verifiable();
                    EventAggregatorMock
                        .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                        .Returns(CanSaveEventMock.Object)
                        .Verifiable();
                }
                // only makes sense if using real events
                Assert.True(_useRealEvents);
                EditActionEvent.Publish(EditActionMessage.StartEditing);
            }

            public void VerifyAll()
            {
                EventAggregatorMock.VerifyAll();
                DeadfileRepositoryMock.VerifyAll();
                LockedForEditingMock.VerifyAll();
                CanUndoEventMock.VerifyAll();
                EventAggregatorMock.Reset();
                DeadfileRepositoryMock.Reset();
                LockedForEditingMock.Reset();
                CanUndoEventMock.Reset();
                if (!_useRealEvents)
                {
                    UndoEventMock.VerifyAll();
                    DeleteEventMock.VerifyAll();
                    EditActionEventMock.VerifyAll();
                    CanEditEventMock.VerifyAll();
                    CanDeleteEventMock.VerifyAll();
                    CanSaveEventMock.VerifyAll();
                    SaveEventMock.VerifyAll();
                    UndoEventMock.Reset();
                    DeleteEventMock.Reset();
                    EditActionEventMock.Reset();
                    CanEditEventMock.Reset();
                    CanDeleteEventMock.Reset();
                    CanSaveEventMock.Reset();
                    SaveEventMock.Reset();
                }
            }

            public void Dispose()
            {
                VerifyAll();
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
        public void TestNavigateToNewClient()
        {
            using (var host = new Host(true))
            {
                host.NavigateTo(new ClientModel());
            }
        }

        [Fact]
        public void TestNavigateToExistingClient_SendsCanEditMessageToActionsPad()
        {
            using (var host = new Host(true))
            {
                Assert.False(host.ViewModel.CanEdit);
                var a = 0;
                var c = 0;
                var ce = CanEditMessage.CannotEdit;
                var cs = CanSaveMessage.CannotSave;
                host.CanEditEvent.Subscribe((b) =>
                {
                    ++a;
                    ce = b;
                });
                host.CanSaveEvent.Subscribe((b) =>
                {
                    ++c;
                    cs = b;
                });
                host.NavigateTo(
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                Assert.Equal(1, a);
                Assert.Equal(0, c);
                Assert.Equal(CanEditMessage.CanEdit, ce);
                Assert.Equal(CanSaveMessage.CannotSave, cs);
                Assert.True(host.ViewModel.CanEdit);
            }
        }

        [Theory]
        [InlineData("LastName", "")]
        [InlineData("AddressFirstLine", "")]
        [InlineData("EmailAddress", "jack.bauer@@@yahoo.com")]
        [InlineData("PhoneNumber1", "")]
        [InlineData("PhoneNumber1", "dfgdf")]
        [InlineData("PhoneNumber2", "edbdj")]
        [InlineData("PhoneNumber3", "abcde")]
        public void TestInvalidInput_SendsCannotSave(string propertyName, object newValue)
        {
            using (var host = new Host(true))
            {
                host.NavigateTo(
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                var c = 0;
                var m = CanSaveMessage.CannotSave;
                host.CanSaveEvent.Subscribe((message) =>
                {
                    ++c;
                    m = message;
                });
                host.StartEditing();
                Assert.Equal(1, c);
                Assert.Equal(m, CanSaveMessage.CanSave);
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                    .Returns(host.CanSaveEvent)
                    .Verifiable();
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(host.CanUndoEventMock.Object)
                    .Verifiable();
                host.CanUndoEventMock
                    .Setup((cu) => cu.Publish(CanUndoMessage.CanUndo))
                    .Verifiable();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                Assert.Equal(2, c);
                Assert.Equal(m, CanSaveMessage.CannotSave);
            }
        }

        [Theory]
        [InlineData("LastName", "")]
        [InlineData("AddressFirstLine", "")]
        [InlineData("EmailAddress", "jack.bauer@@@yahoo.com")]
        [InlineData("PhoneNumber1", "")]
        [InlineData("PhoneNumber1", "dfgdf")]
        [InlineData("PhoneNumber2", "edbdj")]
        [InlineData("PhoneNumber3", "abcde")]
        public void TestUndoAfterInvalidInput_SendsCanSaveCannotUndoAndCanRedo(string propertyName, object newValue)
        {
            using (var host = new Host(true))
            {
                host.NavigateTo(
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                host.StartEditing();
                var c = 0;
                var m = CanSaveMessage.CanSave;
                host.CanSaveEvent.Subscribe((message) =>
                {
                    ++c;
                    m = message;
                });
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                    .Returns(host.CanSaveEvent)
                    .Verifiable();
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(host.CanUndoEventMock.Object)
                    .Verifiable();
                host.CanUndoEventMock
                    .Setup((cu) => cu.Publish(CanUndoMessage.CanUndo))
                    .Verifiable();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                host.CanUndoEventMock
                    .Setup((cu) => cu.Publish(CanUndoMessage.CannotUndo))
                    .Verifiable();
                host.CanUndoEventMock
                    .Setup((cu) => cu.Publish(CanUndoMessage.CanRedo))
                    .Verifiable();
                host.UndoEvent.Publish(UndoMessage.Undo);
                Assert.Equal(2, c);
                Assert.Equal(m, CanSaveMessage.CanSave);
            }
        }

        [Fact]
        public void TestUnsubscribeFromEventsOnNavigatedFrom()
        {
            using (var host = new Host(false))
            {
                host.NavigateTo(
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                host.NavigateFrom();
            }
        }
    }
}
