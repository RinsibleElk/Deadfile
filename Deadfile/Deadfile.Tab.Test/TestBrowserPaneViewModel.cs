using System;
using System.Linq;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Events;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
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
            var refreshBrowserEvent = new RefreshBrowserEvent();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<LockedForEditingEvent>())
                .Returns(lockedForEditingEvent)
                .Verifiable();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<RefreshBrowserEvent>())
                .Returns(refreshBrowserEvent)
                .Verifiable();
            var viewModel = new BrowserPaneViewModel(eventAggregatorMock.Object, deadfileRepositoryMock.Object);

            // Activate.
            viewModel.OnNavigatedTo(null);

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
            var refreshBrowserEvent = new RefreshBrowserEvent();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<LockedForEditingEvent>())
                .Returns(lockedForEditingEvent)
                .Verifiable();
            eventAggregatorMock
                .Setup((eventAggregator) => eventAggregator.GetEvent<RefreshBrowserEvent>())
                .Returns(refreshBrowserEvent)
                .Verifiable();
            var viewModel = new BrowserPaneViewModel(eventAggregatorMock.Object, deadfileRepositoryMock.Object);

            // Activate.
            viewModel.OnNavigatedTo(null);

            // Fire in
            lockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = true});

            // Checks.
            eventAggregatorMock.VerifyAll();
            Assert.False(viewModel.BrowsingEnabled);
        }
    }
}
