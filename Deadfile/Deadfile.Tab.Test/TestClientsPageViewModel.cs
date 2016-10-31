using System;
using System.Linq;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Events;
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
            public readonly EditActionEvent EditItemEvent = new EditActionEvent();
            public readonly CanEditEvent CanEditEvent = new CanEditEvent();
            public readonly CanSaveEvent CanSaveEvent = new CanSaveEvent();
            public readonly SaveEvent SaveEvent = new SaveEvent();
            public Host()
            {
                ViewModel = new ClientsPageViewModel(EventAggregatorMock.Object, DeadfileRepositoryMock.Object);
            }

            public void VerifyAll()
            {
                EventAggregatorMock.VerifyAll();
                DeadfileRepositoryMock.VerifyAll();
                LockedForEditingMock.VerifyAll();
                CanUndoEventMock.VerifyAll();
            }

            public void Dispose()
            {
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

        private static void NavigateTo(Host host, ClientModel model)
        {
            // Expect subscriptions to UndoEvent, CanEditEvent, EditActionEvent
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<UndoEvent>())
                .Returns(host.UndoEvent)
                .Verifiable();
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<EditActionEvent>())
                .Returns(host.EditItemEvent)
                .Verifiable();
            if (model.Id != ModelBase.NewModelId)
            {
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                    .Returns(host.CanSaveEvent)
                    .Verifiable();
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanEditEvent>())
                    .Returns(host.CanEditEvent)
                    .Verifiable();
                host.DeadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(model.Id))
                    .Returns(model)
                    .Verifiable();
            }
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                .Returns(host.DisplayNameEventMock.Object)
                .Verifiable();
            host.DisplayNameEventMock
                .Setup((ev) => ev.Publish(""))
                .Verifiable();
            host.ViewModel.OnNavigatedTo(model.Id);
            host.VerifyAll();
        }

        [Fact]
        public void TestNavigateToNewClient()
        {
            using (var host = new Host())
            {
                NavigateTo(host, new ClientModel());
            }
        }

        [Fact]
        public void TestNavigateToExistingClient_SendsCanEditMessageToActionsPad()
        {
            using (var host = new Host())
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
                NavigateTo(host,
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                Assert.Equal(1, a);
                Assert.Equal(1, c);
                Assert.Equal(CanEditMessage.CanEdit, ce);
                Assert.Equal(CanSaveMessage.CanSave, cs);
                Assert.True(host.ViewModel.CanEdit);
            }
        }

        void StartEditing(Host host)
        {
            // He'll subscribe to the save event.
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<SaveEvent>())
                .Returns(host.SaveEvent)
                .Verifiable();
            // And he'll publish that we're locked for editing.
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                .Returns(host.LockedForEditingMock.Object)
                .Verifiable();
            host.LockedForEditingMock
                .Setup((ev) => ev.Publish(LockedForEditingMessage.Locked))
                .Verifiable();
            host.EditItemEvent.Publish(EditActionMessage.StartEditing);
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
            using (var host = new Host())
            {
                NavigateTo(host,
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                StartEditing(host);
                var c = 0;
                var m = CanSaveMessage.CanSave;
                host.CanSaveEvent.Subscribe((message) =>
                {
                    ++c;
                    m = message;
                });
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(host.CanUndoEventMock.Object)
                    .Verifiable();
                host.CanUndoEventMock
                    .Setup((cu) => cu.Publish(CanUndoMessage.CanUndo))
                    .Verifiable();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                Assert.Equal(1, c);
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
            using (var host = new Host())
            {
                NavigateTo(host,
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                StartEditing(host);
                var c = 0;
                var m = CanSaveMessage.CanSave;
                host.CanSaveEvent.Subscribe((message) =>
                {
                    ++c;
                    m = message;
                });
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
    }
}
