using System;
using System.ComponentModel;
using System.Linq;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Tab.Events;
using Deadfile.Tab.Navigation;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestNavigationBarViewModel
    {
        private NavigationBarViewModel _viewModel = null;

        private class RealEventsHost : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<INavigationService> NavigationServiceMock = new Mock<INavigationService>();
            public readonly CanUndoEvent CanUndoEvent = new CanUndoEvent();
            public readonly LockedForEditingEvent LockedForEditingEvent = new LockedForEditingEvent();
            public readonly DiscardChangesEvent DiscardChangesEvent = new DiscardChangesEvent();
            public readonly NavigationBarViewModel ViewModel;
            public int NumberOfTimesBackCanExecuteChangedFired = 0;
            public int NumberOfTimesHomeCanExecuteChangedFired = 0;
            public int NumberOfTimesForwardCanExecuteChangedFired = 0;

            public RealEventsHost()
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(CanUndoEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(DiscardChangesEvent)
                    .Verifiable();
                ViewModel = new NavigationBarViewModel(NavigationServiceMock.Object, EventAggregatorMock.Object);
                ViewModel.PropertyChanged += (s, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case nameof(ViewModel.CanBack):
                            ++NumberOfTimesBackCanExecuteChangedFired;
                            break;
                        case nameof(ViewModel.CanHome):
                            ++NumberOfTimesHomeCanExecuteChangedFired;
                            break;
                        case nameof(ViewModel.CanForward):
                            ++NumberOfTimesForwardCanExecuteChangedFired;
                            break;
                    }
                };
                ViewModel.OnNavigatedTo(null);
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
                Assert.False(host.ViewModel.CanBack);
                Assert.False(host.ViewModel.CanHome);
                Assert.False(host.ViewModel.CanForward);
                Assert.False(host.ViewModel.CanUndo);
                Assert.False(host.ViewModel.CanRedo);
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
                // Fire an event from the NavigationService to say that we can go back.
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoBack)
                    .Returns(true)
                    .Verifiable();
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoForward)
                    .Returns(false)
                    .Verifiable();
                host.NavigationServiceMock.Raise(((n) => n.PropertyChanged += null),
                    new PropertyChangedEventArgs("CanGoBack"));

                // Verify.
                Assert.True(host.ViewModel.CanBack);
                Assert.True(host.ViewModel.CanHome);
                Assert.Equal(0, host.NumberOfTimesForwardCanExecuteChangedFired);
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
                // Fire in an event that indicates that navigation forward is cool.
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoBack)
                    .Returns(false)
                    .Verifiable();
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoForward)
                    .Returns(true)
                    .Verifiable();
                host.NavigationServiceMock.Raise(((n) => n.PropertyChanged += null),
                    new PropertyChangedEventArgs("CanGoForward"));

                // Verify.
                Assert.True(host.ViewModel.CanForward);
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(0, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(0, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestLockedForEditing_CantGoBack()
        {
            using (var host = new RealEventsHost())
            {
                // Fire an event from the NavigationService to say that we can go back.
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoBack)
                    .Returns(true)
                    .Verifiable();
                host.NavigationServiceMock
                    .Setup((n) => n.CanGoForward)
                    .Returns(false)
                    .Verifiable();
                host.NavigationServiceMock.Raise(((n) => n.PropertyChanged += null),
                    new PropertyChangedEventArgs("CanGoBack"));
                host.LockedForEditingEvent.Publish(LockedForEditingMessage.Locked);

                // Verify.
                Assert.False(host.ViewModel.CanBack);
                Assert.False(host.ViewModel.CanHome);
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }
    }
}
