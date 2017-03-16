using System;
using System.Linq;
using Deadfile.Tab.Events;
using Deadfile.Tab.Invoices;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestInvoicesActionsPadViewModel
    {
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);
        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock;
            public readonly Mock<EditActionEvent> EditClientEventMock;
            public readonly InvoicesActionsPadViewModel ViewModel;
            public readonly LockedForEditingEvent LockedForEditingEvent;
            public readonly PageStateEvent<InvoicesPageState> PageStateEvent;
            public Host()
            {
                EventAggregatorMock = new Mock<IEventAggregator>();
                EditClientEventMock = new Mock<EditActionEvent>();
                EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(EditClientEventMock.Object)
                    .Verifiable();
                ViewModel = new InvoicesActionsPadViewModel(TabIdentity, EventAggregatorMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                PageStateEvent = new PageStateEvent<InvoicesPageState>();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>())
                    .Returns(PageStateEvent)
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
            var viewModel = new InvoicesActionsPadViewModel(TabIdentity, eventAggregatorMock.Object);

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
                host.EditClientEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });

                // Checks.
                Assert.False(host.ViewModel.CanPrintItem);
                Assert.False(host.ViewModel.CanSaveItem);
                Assert.False(host.ViewModel.EditItemIsVisible);
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.DeleteItemIsVisible);
                Assert.True(host.ViewModel.CanDiscardItem);
            }
        }

        [Fact]
        public void TestEndEditing_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.EditClientEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = false });

                // Checks.
                Assert.True(host.ViewModel.CanPrintItem);
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
                host.EditClientEventMock
                    .Setup((ev) => ev.Publish(EditActionMessage.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItem();
                host.LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
//                host.CanSaveEvent.Publish(CanSaveMessage.CannotSave);

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
            }
        }
    }
}
