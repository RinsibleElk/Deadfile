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
            private readonly EditActionEvent _editActionEvent = new EditActionEvent();
            private readonly List<DiscardChangesMessage> _receiveDiscardChangesMessages = new List<DiscardChangesMessage>();
            private readonly DiscardChangesEvent _discardChangesEvent = new DiscardChangesEvent();
            public readonly ClientsActionsPadViewModel ViewModel;
            private readonly LockedForEditingEvent _lockedForEditingEvent = new LockedForEditingEvent();
            private readonly PageStateEvent<ClientsPageState> _pageStateEvent = new PageStateEvent<ClientsPageState>();
            private readonly List<AddClientMessage> _receivedAddClientMessages = new List<AddClientMessage>();
            private readonly AddClientEvent _addClientEvent = new AddClientEvent();
            public Host()
            {
                _eventAggregatorMock = new Mock<IEventAggregator>();
                _editActionEvent.Subscribe((m) => _receivedEditActionMessages.Add(m));
                _discardChangesEvent.Subscribe((m) => _receiveDiscardChangesMessages.Add(m));
                _addClientEvent.Subscribe((m) => _receivedAddClientMessages.Add(m));
                ViewModel = new ClientsActionsPadViewModel(TabIdentity, _eventAggregatorMock.Object);
            }

            public void NavigateToExisting()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(null);
                _pageStateEvent.Publish(ClientsPageState.CanEdit | ClientsPageState.CanDelete);
            }

            public void Edit()
            {
                Assert.True(ViewModel.CanEditItem);
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                ViewModel.EditItem();
                Assert.Equal(1, _receivedEditActionMessages.Count);
                Assert.Equal(EditActionMessage.StartEditing, _receivedEditActionMessages[0]);
                _receivedEditActionMessages.Clear();
                _lockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = true });
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDelete);
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit);
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDiscard);
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDiscard | ClientsPageState.UnderEdit);
            }

            public void Discard()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                Assert.True(ViewModel.CanDiscardItem);
                ViewModel.DiscardItem();
                Assert.Equal(1, _receiveDiscardChangesMessages.Count);
                Assert.Equal(DiscardChangesMessage.Discard, _receiveDiscardChangesMessages[0]);
                _receiveDiscardChangesMessages.Clear();
                Assert.Equal(1, _receivedEditActionMessages.Count);
                Assert.Equal(EditActionMessage.EndEditing, _receivedEditActionMessages[0]);
                _receivedEditActionMessages.Clear();
                _lockedForEditingEvent.Publish(new LockedForEditingMessage() { IsLocked = false });
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDelete | ClientsPageState.CanDiscard | ClientsPageState.UnderEdit);
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDelete | ClientsPageState.UnderEdit);
                _pageStateEvent.Publish(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDelete);
                _pageStateEvent.Publish(ClientsPageState.CanEdit | ClientsPageState.CanDelete);
            }

            public void Dispose()
            {
                _eventAggregatorMock.VerifyAll();
                Assert.Empty(_receivedEditActionMessages);
                Assert.Empty(_receiveDiscardChangesMessages);
                Assert.Empty(_receivedAddClientMessages);
            }

            public void CannotSave()
            {
                _pageStateEvent.Publish(ViewModel.PageState & ~ClientsPageState.CanSave);
            }

            public void AddNewClient()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddClientEvent>())
                    .Returns(_addClientEvent)
                    .Verifiable();
                ViewModel.AddItem();
                Assert.Equal(1, _receivedAddClientMessages.Count);
                Assert.Equal(AddClientMessage.AddClient, _receivedAddClientMessages[0]);
                _receivedAddClientMessages.Clear();
            }
        }

        [Fact]
        public void TestNavigateToExisting()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting();
                Assert.True(host.ViewModel.CanDeleteItem);
                Assert.True(host.ViewModel.CanAddItem);
                Assert.True(host.ViewModel.CanEditItem);
                Assert.False(host.ViewModel.CanSaveItem);
                Assert.False(host.ViewModel.CanDiscardItem);
                Assert.True(host.ViewModel.DeleteItemIsVisible);
                Assert.True(host.ViewModel.AddItemIsVisible);
                Assert.True(host.ViewModel.EditItemIsVisible);
                Assert.False(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.DiscardItemIsVisible);
            }
        }

        [Fact]
        public void TestEditAndLock_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.NavigateToExisting();
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
                host.NavigateToExisting();
                host.Edit();
                host.Discard();

                // Checks.
                Assert.True(host.ViewModel.AddItemIsVisible);
                Assert.True(host.ViewModel.CanAddItem);
                Assert.True(host.ViewModel.EditItemIsVisible);
                Assert.False(host.ViewModel.SaveItemIsVisible);
                Assert.True(host.ViewModel.DeleteItemIsVisible);
                Assert.False(host.ViewModel.CanDiscardItem);
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
                host.NavigateToExisting();
                host.Edit();
                host.CannotSave();

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
            }
        }

        [Fact]
        public void TestAddNewClient()
        {
            using (var host = new Host())
            {
                host.NavigateToExisting();
                host.AddNewClient();
            }
        }
    }
}
