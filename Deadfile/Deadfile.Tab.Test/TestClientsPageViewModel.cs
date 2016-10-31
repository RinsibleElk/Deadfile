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
            public readonly Mock<DisplayNameEvent> DisplayNameEventMock = new Mock<DisplayNameEvent>();
            public readonly ClientsPageViewModel ViewModel;
            public readonly UndoEvent UndoEvent = new UndoEvent();
            public readonly EditActionEvent EditItemEvent = new EditActionEvent();
            public readonly CanEditEvent CanEditEvent = new CanEditEvent();
            public Host()
            {
                ViewModel = new ClientsPageViewModel(EventAggregatorMock.Object, DeadfileRepositoryMock.Object);
            }

            public void VerifyAll()
            {
                EventAggregatorMock.VerifyAll();
                DeadfileRepositoryMock.VerifyAll();
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
        public void TestNavigateToExistingClient()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanEdit);
                var a = 0;
                var ce = CanEditMessage.CannotEdit;
                host.CanEditEvent.Subscribe((b) =>
                {
                    ++a;
                    ce = b;
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
                Assert.Equal(CanEditMessage.CanEdit, ce);
                Assert.True(host.ViewModel.CanEdit);
            }
        }
    }
}
