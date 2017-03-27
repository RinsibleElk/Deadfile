using System;
using System.ComponentModel;
using System.Linq;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Events;
using Deadfile.Tab.Navigation;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestNavigationBarViewModel
    {
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);

        private class RealEventsHost : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<INavigationService> NavigationServiceMock = new Mock<INavigationService>();
            public readonly Mock<IDeadfileRepository> DeadfileRepositoryMock = new Mock<IDeadfileRepository>();
            public readonly UndoEvent UndoEvent = new UndoEvent();
            public readonly CanUndoEvent CanUndoEvent = new CanUndoEvent();
            public readonly NavigateFallBackEvent NavigateFallBackEvent = new NavigateFallBackEvent();
            public readonly LockedForEditingEvent LockedForEditingEvent = new LockedForEditingEvent();
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
                    .Setup((ea) => ea.GetEvent<NavigateFallBackEvent>())
                    .Returns(NavigateFallBackEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                ViewModel = new NavigationBarViewModel(TabIdentity, NavigationServiceMock.Object, EventAggregatorMock.Object, DeadfileRepositoryMock.Object);
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

            public void Undo()
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(UndoEvent)
                    .Verifiable();
                ViewModel.Undo();
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
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = true});

                // Verify.
                Assert.False(host.ViewModel.CanBack);
                Assert.False(host.ViewModel.CanHome);
                Assert.Equal(1, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestLockedForEditing_CantGoForward()
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
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = true});

                // Verify.
                Assert.False(host.ViewModel.CanForward);
                Assert.Equal(2, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(1, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestUnlockedForEditing_CanGoBack()
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
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = false });

                // Verify.
                Assert.True(host.ViewModel.CanBack);
                Assert.True(host.ViewModel.CanHome);
                Assert.Equal(2, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(3, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(3, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestUnlockedForEditing_CanGoForward()
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
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = false });

                // Verify.
                Assert.True(host.ViewModel.CanForward);
                Assert.Equal(3, host.NumberOfTimesForwardCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesHomeCanExecuteChangedFired);
                Assert.Equal(2, host.NumberOfTimesBackCanExecuteChangedFired);
            }
        }

        [Fact]
        public void TestLockedForEditing_CannotUndo()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });

                // Verify.
                Assert.False(host.ViewModel.CanUndo);
                Assert.False(host.ViewModel.CanRedo);
            }
        }

        [Fact]
        public void TestLockedForEditing_MakeEdit_CanUndo()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.CanUndoEvent.Publish(CanUndoMessage.CanUndo);

                // Verify.
                Assert.True(host.ViewModel.CanUndo);
                Assert.False(host.ViewModel.CanRedo);
            }
        }

        [Fact]
        public void TestLockedForEditing_UndoEdit_FiresUndo()
        {
            // Setup.
            using (var host = new RealEventsHost())
            {
                var numUndos = 0;
                var numRedos = 0;
                host.UndoEvent.Subscribe((m) =>
                {
                    if (m == UndoMessage.Undo) ++numUndos;
                    else ++numRedos;
                });
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.CanUndoEvent.Publish(CanUndoMessage.CanUndo);
                host.Undo();

                // Verify.
                Assert.True(host.ViewModel.CanUndo);
                Assert.False(host.ViewModel.CanRedo);
                Assert.Equal(1, numUndos);
                Assert.Equal(0, numRedos);
            }
        }
    }
}
