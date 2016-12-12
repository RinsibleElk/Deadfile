using System;
using System.Linq;
using Deadfile.Tab.Events;
using Deadfile.Tab.Jobs;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestJobsActionsPadViewModel
    {
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);
        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock;
            public readonly Mock<EditActionEvent> EditActionEventMock;
            public readonly JobsActionsPadViewModel ViewModel;
            public readonly LockedForEditingEvent LockedForEditingEvent;
            public readonly CanDiscardEvent CanDiscardEvent;
            public readonly CanDeleteEvent CanDeleteEvent;
            public readonly CanSaveEvent CanSaveEvent;
            public readonly CanEditEvent CanEditEvent;
            public Host()
            {
                EventAggregatorMock = new Mock<IEventAggregator>();
                EditActionEventMock = new Mock<EditActionEvent>();
                ViewModel = new JobsActionsPadViewModel(TabIdentity, EventAggregatorMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                CanSaveEvent = new CanSaveEvent();
                CanDeleteEvent = new CanDeleteEvent();
                CanDiscardEvent = new CanDiscardEvent();
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
                    .Setup((ea) => ea.GetEvent<CanDeleteEvent>())
                    .Returns(CanDeleteEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanDiscardEvent>())
                    .Returns(CanDiscardEvent)
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
                EditActionEventMock.VerifyAll();
            }
        }

        [Fact]
        public void TestRaisesEditActionEvent()
        {
            // Setup.
            using (var host = new Host())
            {
                // Hit the Edit button.
                host.EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(host.EditActionEventMock.Object)
                    .Verifiable();
                host.EditActionEventMock
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
            var viewModel = new JobsActionsPadViewModel(TabIdentity, eventAggregatorMock.Object);

            // Checks.
            Assert.True(viewModel.EditItemIsVisible);
            Assert.False(viewModel.SaveItemIsVisible);
            Assert.True(viewModel.DeleteItemIsVisible);
            Assert.True(viewModel.CanDiscardItem);
            Assert.False(viewModel.DiscardItemIsVisible);
            eventAggregatorMock.VerifyAll();
        }

        [Fact]
        public void TestSwitchToEditingMode_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(host.EditActionEventMock.Object)
                    .Verifiable();
                host.EditActionEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = true});

                // Checks.
                Assert.False(host.ViewModel.EditItemIsVisible);
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.DeleteItemIsVisible);
                Assert.True(host.ViewModel.CanDiscardItem);
                Assert.True(host.ViewModel.DiscardItemIsVisible);
            }
        }

        [Fact]
        public void TestEndEditing_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(host.EditActionEventMock.Object)
                    .Verifiable();
                host.EditActionEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = true});
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() {IsLocked = false});

                // Checks.
                Assert.True(host.ViewModel.EditItemIsVisible);
                Assert.False(host.ViewModel.SaveItemIsVisible);
                Assert.True(host.ViewModel.DeleteItemIsVisible);
                Assert.True(host.ViewModel.CanDiscardItem);
                Assert.False(host.ViewModel.DiscardItemIsVisible);
            }
        }

        [Fact]
        public void TestEditingCannotSave_SaveButtonDisabled()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(host.EditActionEventMock.Object)
                    .Verifiable();
                host.EditActionEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.CanSaveEvent.Publish(CanSaveMessage.CannotSave);

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
            }
        }

        [Fact]
        public void TestEditingJobChild()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.CanDiscardEvent.Publish(CanDiscardMessage.CannotDiscard);

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
                Assert.True(host.ViewModel.DiscardItemIsVisible);
                Assert.False(host.ViewModel.CanDiscardItem);
            }
        }
    }
}
