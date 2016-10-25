using System;
using System.Linq;
using Deadfile.Content.Clients;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Xunit;
using Moq;
using Prism.Events;

namespace Deadfile.Content.Test
{
    public class TestClientsActionsPadViewModel
    {
        [Fact]
        public void TestRaisesEditClientEvent()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var navigationServiceMock = new Mock<IDeadfileNavigationService>();
            var editClientEventMock = new Mock<EditClientEvent>();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<EditClientEvent>())
                .Returns(editClientEventMock.Object)
                .Verifiable();
            editClientEventMock
                .Setup((ev) => ev.Publish())
                .Verifiable();
            var viewModel = new ClientsActionsPadViewModel(eventAggregatorMock.Object, navigationServiceMock.Object);

            // Hit the Edit button.
            viewModel.EditClientCommand.Execute(null);

            // Checks.
            eventAggregatorMock.VerifyAll();
            editClientEventMock.VerifyAll();
        }
    }
}
