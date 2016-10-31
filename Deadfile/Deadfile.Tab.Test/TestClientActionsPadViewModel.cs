using System;
using System.Linq;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Events;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestClientsActionsPadViewModel
    {
        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock;
            public readonly Mock<EditActionEvent> EditClientEventMock;
            public readonly ClientsActionsPadViewModel ViewModel;
            public readonly LockedForEditingEvent LockedForEditingEvent;
            public readonly CanSaveEvent CanSaveEvent;
            public readonly CanEditEvent CanEditEvent;
            public Host()
            {
                EventAggregatorMock = new Mock<IEventAggregator>();
                EditClientEventMock = new Mock<EditActionEvent>();
                EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(EditClientEventMock.Object)
                    .Verifiable();
                ViewModel = new ClientsActionsPadViewModel(EventAggregatorMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                CanSaveEvent = new CanSaveEvent();
                CanEditEvent = new CanEditEvent();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanSaveEvent>())
                    .Returns(CanSaveEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanEditEvent>())
                    .Returns(CanEditEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(null);
            }

            public void Dispose()
            {
                EventAggregatorMock.VerifyAll();
                EditClientEventMock.VerifyAll();
            }
        }

        [Fact]
        public void TestRaisesEditClientEvent()
        {
            // Setup.
            using (var host = new Host())
            {
                // Hit the Edit button.
                host.EditClientEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();

                // Checks.
            }
        }

        [Fact]
        public void TestDefaultButtonVisibilities()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var viewModel = new ClientsActionsPadViewModel(eventAggregatorMock.Object);

            // Checks.
            Assert.True(viewModel.AddItemIsVisible);
            Assert.True(viewModel.EditItemIsVisible);
            Assert.False(viewModel.SaveItemIsVisible);
            Assert.True(viewModel.DeleteItemIsVisible);
            Assert.False(viewModel.DiscardItemIsVisible);
            eventAggregatorMock.VerifyAll();
        }
    }
}
