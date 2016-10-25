using System;
using System.Linq;
using Deadfile.Content.Events;
using Deadfile.Content.Navigation;
using Moq;
using Prism.Events;
using Prism.Regions;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestNavigationBarViewModel
    {
        private NavigationBarViewModel _viewModel = null;

        private class RealEventsHost : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly NavigationEvent NavigationEvent = new NavigationEvent();
            public readonly CanUndoEvent CanUndoEvent = new CanUndoEvent();
            public readonly CanRedoEvent CanRedoEvent = new CanRedoEvent();
            public readonly LockedForEditingEvent LockedForEditingEvent = new LockedForEditingEvent();
            public readonly NavigationBarViewModel ViewModel;
            public int NumberOfTimesBackCanExecuteChangedFired = 0;
            public int NumberOfTimesHomeCanExecuteChangedFired = 0;
            public int NumberOfTimesForwardCanExecuteChangedFired = 0;

            public RealEventsHost()
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<NavigationEvent>())
                    .Returns(NavigationEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(CanUndoEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanRedoEvent>())
                    .Returns(CanRedoEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                ViewModel = new NavigationBarViewModel(EventAggregatorMock.Object);
                ViewModel.BackCommand.CanExecuteChanged += (s, e) => ++NumberOfTimesBackCanExecuteChangedFired;
                ViewModel.HomeCommand.CanExecuteChanged += (s, e) => ++NumberOfTimesHomeCanExecuteChangedFired;
                ViewModel.ForwardCommand.CanExecuteChanged += (s, e) => ++NumberOfTimesForwardCanExecuteChangedFired;
            }

            public void Dispose()
            {
                EventAggregatorMock.VerifyAll();
            }
        }

        [Fact]
        public void TestCreationAndSubscriptions()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                // Verify.
                Assert.False(host.ViewModel.BackCommand.CanExecute(null));
                Assert.False(host.ViewModel.HomeCommand.CanExecute(null));
                Assert.False(host.ViewModel.ForwardCommand.CanExecute(null));
                Assert.False(host.ViewModel.UndoCommand.CanExecute(null));
                Assert.False(host.ViewModel.RedoCommand.CanExecute(null));
                Assert.Equal(0, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(0, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(0, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestCanGoBack()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                // Fire in a Journal that indicates that navigation back is cool.
                var journalMock = new Mock<IRegionNavigationJournal>();
                journalMock
                    .Setup((j) => j.CanGoBack)
                    .Returns(true)
                    .Verifiable();
                host.NavigationEvent.Publish(journalMock.Object);

                // Verify.
                Assert.True(host.ViewModel.BackCommand.CanExecute(null));
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestCanGoHome()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                // Fire in a Journal that indicates that navigation back is cool.
                var journalMock = new Mock<IRegionNavigationJournal>();
                journalMock
                    .Setup((j) => j.CanGoBack)
                    .Returns(true)
                    .Verifiable();
                host.NavigationEvent.Publish(journalMock.Object);

                // Verify.
                Assert.True(host.ViewModel.HomeCommand.CanExecute(null));
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestCanGoForward()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                // Fire in a Journal that indicates that navigation back is cool.
                var journalMock = new Mock<IRegionNavigationJournal>();
                journalMock
                    .Setup((j) => j.CanGoForward)
                    .Returns(true)
                    .Verifiable();
                host.NavigationEvent.Publish(journalMock.Object);

                // Verify.
                Assert.True(host.ViewModel.ForwardCommand.CanExecute(null));
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }
    }
}
