using System;
using System.Linq;
using Deadfile.Content.Events;
using Deadfile.Content.Navigation;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestNavigationBarViewModel
    {
        [Fact]
        public void TestCreationAndSubscriptions()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var navigationEvent = new NavigationEvent();
            var canUndoEvent = new CanUndoEvent();
            var canRedoEvent = new CanRedoEvent();
            var lockedForEditingEvent = new LockedForEditingEvent();
            eventAggregatorMock
                .Setup((ea) => ea.GetEvent<NavigationEvent>())
                .Returns(navigationEvent)
                .Verifiable();
            eventAggregatorMock
                .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                .Returns(canUndoEvent)
                .Verifiable();
            eventAggregatorMock
                .Setup((ea) => ea.GetEvent<CanRedoEvent>())
                .Returns(canRedoEvent)
                .Verifiable();
            eventAggregatorMock
                .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                .Returns(lockedForEditingEvent)
                .Verifiable();
            var viewModel = new NavigationBarViewModel(eventAggregatorMock.Object);

            // Verify.
            eventAggregatorMock.VerifyAll();
        }
    }
}
