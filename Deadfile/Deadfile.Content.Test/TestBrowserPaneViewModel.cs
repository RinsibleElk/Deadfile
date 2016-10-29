using System;
using System.Linq;
using Deadfile.Content.Browser;
using Deadfile.Content.Clients;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Interfaces;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestBrowserPaneViewModel
    {
        [Fact]
        public void TestInactiveCreation()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var deadfileRepositoryMock = new Mock<IDeadfileRepository>();
            var viewModel = new BrowserPaneViewModel(eventAggregatorMock.Object, deadfileRepositoryMock.Object);

            // Nothing.

            // Checks.
            Assert.False(viewModel.IsActive);
            eventAggregatorMock.VerifyAll();
        }

        [Fact]
        public void TestBrowsingEnabledByDefault()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var deadfileRepositoryMock = new Mock<IDeadfileRepository>();
            var lockedForEditingEvent = new LockedForEditingEvent();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<LockedForEditingEvent>())
                .Returns(lockedForEditingEvent)
                .Verifiable();
            var viewModel = new BrowserPaneViewModel(eventAggregatorMock.Object, deadfileRepositoryMock.Object);
            viewModel.IsActive = true;

            // Nothing.

            // Checks.
            Assert.True(viewModel.BrowsingEnabled);
            eventAggregatorMock.VerifyAll();
        }

        [Fact]
        public void TestLockedForEditing()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var deadfileRepositoryMock = new Mock<IDeadfileRepository>();
            var lockedForEditingEvent = new LockedForEditingEvent();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<LockedForEditingEvent>())
                .Returns(lockedForEditingEvent)
                .Verifiable();
            var viewModel = new BrowserPaneViewModel(eventAggregatorMock.Object, deadfileRepositoryMock.Object);

            // Activate.
            viewModel.IsActive = true;

            // Fire in
            lockedForEditingEvent.Publish(true);

            // Checks.
            eventAggregatorMock.VerifyAll();
            Assert.False(viewModel.BrowsingEnabled);
        }
    }
}
