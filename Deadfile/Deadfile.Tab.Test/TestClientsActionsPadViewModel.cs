using System;
using System.Collections.Generic;
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
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);
        private class Host : IDisposable
        {
            private readonly Mock<IEventAggregator> _eventAggregatorMock;
            private readonly List<EditActionMessage> _receivedEditActionMessages = new List<EditActionMessage>();
            private readonly EditActionEvent _editClientEvent = new EditActionEvent();
            private readonly List<DiscardChangesMessage> _receiveDiscardChangesMessages = new List<DiscardChangesMessage>();
            private readonly DiscardChangesEvent _discardChangesEvent = new DiscardChangesEvent();
            public readonly ClientsActionsPadViewModel ViewModel;
            private readonly LockedForEditingEvent LockedForEditingEvent;
            private readonly PageStateEvent<ClientsPageState> _pageStateEvent;
            public Host()
            {
                _eventAggregatorMock = new Mock<IEventAggregator>();
                _editClientEvent.Subscribe((m) => _receivedEditActionMessages.Add(m));
                _discardChangesEvent.Subscribe((m) => _receiveDiscardChangesMessages.Add(m));
                _eventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditActionEvent>())
                    .Returns(_editClientEvent)
                    .Verifiable();
                ViewModel = new ClientsActionsPadViewModel(TabIdentity, _eventAggregatorMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                _pageStateEvent = new PageStateEvent<ClientsPageState>();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(null);
            }

            public void Edit()
            {
                Assert.True(ViewModel.CanEditItem);
                ViewModel.EditItem();
                Assert.Equal(1, _receivedEditActionMessages.Count);
                Assert.Equal(EditActionMessage.StartEditing, _receivedEditActionMessages[0]);
                _receivedEditActionMessages.Clear();
                LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
            }

            public void Discard()
            {
                Assert.True(ViewModel.CanDiscardItem);
                ViewModel.DiscardItem();
                Assert.Equal(1, _receiveDiscardChangesMessages.Count);
                Assert.Equal(DiscardChangesMessage.Discard, _receiveDiscardChangesMessages[0]);
                _receiveDiscardChangesMessages.Clear();
                Assert.Equal(1, _receivedEditActionMessages.Count);
                Assert.Equal(EditActionMessage.EndEditing, _receivedEditActionMessages[0]);
                _receivedEditActionMessages.Clear();
                LockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = false });
            }

            public void Dispose()
            {
                _eventAggregatorMock.VerifyAll();
                Assert.Empty(_receivedEditActionMessages);
                Assert.Empty(_receiveDiscardChangesMessages);
            }
        }

        [Fact]
        public void TestDefaultButtonVisibilities()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var viewModel = new ClientsActionsPadViewModel(TabIdentity, eventAggregatorMock.Object);

            // Checks.
            Assert.True(viewModel.AddItemIsVisible);
            Assert.True(viewModel.EditItemIsVisible);
            Assert.False(viewModel.SaveItemIsVisible);
            Assert.True(viewModel.DeleteItemIsVisible);
            Assert.True(viewModel.CanDiscardItem);
            Assert.False(viewModel.DiscardItemIsVisible);
            eventAggregatorMock.VerifyAll();
        }

        [Fact]
        public void TestEditAndLock_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.Edit();

                // Checks.
                Assert.False(host.ViewModel.AddItemIsVisible);
                Assert.False(host.ViewModel.CanAddItem);
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
                host.Edit();
                host.Discard();

                // Checks.
                Assert.True(host.ViewModel.AddItemIsVisible);
                Assert.True(host.ViewModel.CanAddItem);
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
                host.Edit();
                //                host.PageStateEvent.Publish(CanSaveMessage.CannotSave);

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
            }
        }
    }
}
