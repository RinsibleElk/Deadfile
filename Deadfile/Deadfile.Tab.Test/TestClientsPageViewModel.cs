using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestClientsPageViewModel
    {
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);

        private class Host : IDisposable
        {
            private readonly Mock<IEventAggregator> _eventAggregatorMock = new Mock<IEventAggregator>();
            private readonly Mock<IDeadfileRepository> _deadfileRepositoryMock = new Mock<IDeadfileRepository>();
            private readonly Mock<IUrlNavigationService> _urlNavigationServiceMock = new Mock<IUrlNavigationService>();
            private readonly Mock<IDeadfileDialogCoordinator> _dialogCoordinatorMock = new Mock<IDeadfileDialogCoordinator>();

            public readonly ClientsPageViewModel ViewModel;

            private readonly UndoEvent _undoEvent = new UndoEvent();
            private readonly DeleteEvent _deleteEvent = new DeleteEvent();
            private readonly EditActionEvent _editActionEvent = new EditActionEvent();
            private ClientsPageState _currentState = 0;
            private readonly PageStateEvent<ClientsPageState> _pageStateEvent = new PageStateEvent<ClientsPageState>();
            private readonly List<CanUndoMessage> _receivedCanUndoMessages = new List<CanUndoMessage>();
            private readonly CanUndoEvent _canUndoEvent = new CanUndoEvent();
            private readonly SaveEvent _saveEvent = new SaveEvent();
            private readonly DiscardChangesEvent _discardChangesEvent = new DiscardChangesEvent();
            private readonly List<RefreshBrowserMessage> _receivedRefreshBrowserMessages = new List<RefreshBrowserMessage>();
            private readonly RefreshBrowserEvent _refreshBrowserEvent = new RefreshBrowserEvent();
            private readonly AddNewJobEvent _addNewJobEvent = new AddNewJobEvent();
            private readonly List<string> _receivedDisplayNames = new List<string>();
            private readonly DisplayNameEvent _displayNameEvent = new DisplayNameEvent();
            private readonly List<LockedForEditingMessage> _receivedLockedForEditingMessages = new List<LockedForEditingMessage>();
            private readonly LockedForEditingEvent _lockedForEditingEvent = new LockedForEditingEvent();
            private readonly List<int> _receivedInvoiceClientMessages = new List<int>();
            private readonly InvoiceClientEvent _invoiceClientEvent = new InvoiceClientEvent();

            public Host()
            {
                ViewModel = new ClientsPageViewModel(TabIdentity, _eventAggregatorMock.Object, _deadfileRepositoryMock.Object, _dialogCoordinatorMock.Object, _urlNavigationServiceMock.Object);
                _displayNameEvent.Subscribe((m) => _receivedDisplayNames.Add(m));
                _refreshBrowserEvent.Subscribe((m) => _receivedRefreshBrowserMessages.Add(m));
                _lockedForEditingEvent.Subscribe((m) => _receivedLockedForEditingMessages.Add(m));
                _pageStateEvent.Subscribe((m) => _currentState = m);
                _canUndoEvent.Subscribe((m) => _receivedCanUndoMessages.Add(m));
                _invoiceClientEvent.Subscribe((m) => _receivedInvoiceClientMessages.Add(m));
            }

            public void NavigateToExisting(ClientModel model)
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(_undoEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DeleteEvent>())
                    .Returns(_deleteEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _deadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(model.Id))
                    .Returns(model)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientNavigationKey(model.Id));
                ViewModel.CompleteNavigation();
                VerifyDisplayName(model.FullName);
                Assert.True(ViewModel.CanInvoiceClient);
                VerifyAll();
            }

            public void NavigateToNewClient()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(_undoEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DeleteEvent>())
                    .Returns(_deleteEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(_displayNameEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(new ClientNavigationKey(Int32.MinValue));
                // He'll subscribe to the save event and discard changes event.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(_saveEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                // And he'll publish that we're locked for editing.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                // And he'll publish that it is allowed to save.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                ViewModel.CompleteNavigation();
                VerifyDisplayName("New Client");
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.True(_receivedLockedForEditingMessages[0].IsLocked);
                _receivedLockedForEditingMessages.Clear();
                Assert.True(ViewModel.UnderEdit);
                Assert.False(ViewModel.CanInvoiceClient);
                VerifyAll();
            }

            public void NavigateFrom()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<UndoEvent>())
                    .Returns(_undoEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DeleteEvent>())
                    .Returns(_deleteEvent)
                    .Verifiable();
                ViewModel.OnNavigatedFrom();
                VerifyAll();
            }

            private void VerifyDisplayName(string expected)
            {
                Assert.Equal(1, _receivedDisplayNames.Count);
                Assert.Equal(expected, _receivedDisplayNames[0]);
                _receivedDisplayNames.Clear();
            }

            public void StartEditing()
            {
                // He'll subscribe to the save event and discard changes event.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SaveEvent>())
                    .Returns(_saveEvent)
                    .Verifiable();
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DiscardChangesEvent>())
                    .Returns(_discardChangesEvent)
                    .Verifiable();
                // And he'll publish that we're locked for editing.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(_lockedForEditingEvent)
                    .Verifiable();
                // And he'll publish that it is allowed to save.
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();
                // only makes sense if using real events
                _editActionEvent.Publish(EditActionMessage.StartEditing);
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.True(_receivedLockedForEditingMessages[0].IsLocked);
                _receivedLockedForEditingMessages.Clear();
                Assert.True(ViewModel.UnderEdit);
            }

            public void ExpectCanUndoPublish()
            {
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanUndoEvent>())
                    .Returns(_canUndoEvent)
                    .Verifiable();
            }

            public void ExpectCanUndo(bool canUndo)
            {
                Assert.NotEmpty(_receivedCanUndoMessages);
                Assert.Equal((canUndo ? CanUndoMessage.CanUndo : CanUndoMessage.CannotUndo), _receivedCanUndoMessages[0]);
                _receivedCanUndoMessages.RemoveAt(0);
            }

            public void ExpectCanRedo(bool canRedo)
            {
                Assert.NotEmpty(_receivedCanUndoMessages);
                Assert.Equal((canRedo ? CanUndoMessage.CanRedo : CanUndoMessage.CannotRedo), _receivedCanUndoMessages[0]);
                _receivedCanUndoMessages.RemoveAt(0);
            }

            public void DeleteClient(MessageDialogResult dialogResult)
            {
                Assert.True(ViewModel.CanDelete);
                _dialogCoordinatorMock
                    .Setup((dc) => dc.ConfirmDeleteAsync(ViewModel, It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(dialogResult))
                    .Verifiable();
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    _eventAggregatorMock
                        .Setup((ea) => ea.GetEvent<RefreshBrowserEvent>())
                        .Returns(_refreshBrowserEvent)
                        .Verifiable();
                    _deadfileRepositoryMock
                        .Setup((repository) => repository.SaveClient(ViewModel.SelectedItem))
                        .Verifiable();
                }
                _deleteEvent.Publish(DeleteMessage.Delete);
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    Assert.Equal(1, _receivedRefreshBrowserMessages.Count);
                    Assert.Equal(RefreshBrowserMessage.Refresh, _receivedRefreshBrowserMessages[0]);
                    _receivedRefreshBrowserMessages.Clear();
                }
                VerifyAll();
            }

            private void VerifyAll()
            {
                _eventAggregatorMock.VerifyAll();
                _deadfileRepositoryMock.VerifyAll();
                _eventAggregatorMock.Reset();
                _deadfileRepositoryMock.Reset();
                Assert.Equal(0, _receivedDisplayNames.Count);
                Assert.Equal(0, _receivedRefreshBrowserMessages.Count);
                Assert.Equal(0, _receivedLockedForEditingMessages.Count);
                Assert.Equal(0, _receivedInvoiceClientMessages.Count);
            }

            public void EmailClient()
            {
                Assert.True(ViewModel.CanEmailClient);
                _urlNavigationServiceMock
                    .Setup((uns) => uns.SendEmail(ViewModel.SelectedItem.EmailAddress))
                    .Verifiable();
                ViewModel.EmailClient();
            }

            public void InvoiceClient()
            {
                Assert.True(ViewModel.CanInvoiceClient);
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<InvoiceClientEvent>())
                    .Returns(_invoiceClientEvent)
                    .Verifiable();
                ViewModel.InvoiceClient();
                Assert.Equal(1, _receivedInvoiceClientMessages.Count);
                Assert.Equal(ViewModel.SelectedItem.ClientId, _receivedInvoiceClientMessages[0]);
                _receivedInvoiceClientMessages.Clear();
            }

            public void AddNewJob()
            {
                Assert.True(ViewModel.CanAddNewJob);
                var li = new List<int>();
                _addNewJobEvent.Subscribe((clientId) => li.Add(clientId));
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddNewJobEvent>())
                    .Returns(_addNewJobEvent)
                    .Verifiable();
                ViewModel.AddNewJob();
                Assert.Equal(1, li.Count);
                Assert.Equal(ViewModel.SelectedItem.ClientId, li[0]);
            }

            public bool CanSave => ViewModel.State.HasFlag(ClientsPageState.CanSave);

            public void Undo()
            {
                _undoEvent.Publish(UndoMessage.Undo);
            }

            public void Redo()
            {
                _undoEvent.Publish(UndoMessage.Redo);
            }

            public void Dispose()
            {
                VerifyAll();
            }

            public void Discard(ClientNavigationKey navigationKey)
            {
                _discardChangesEvent.Publish(DiscardChangesMessage.Discard);
                _editActionEvent.Publish(EditActionMessage.EndEditing);
                Assert.Equal(1, _receivedLockedForEditingMessages.Count);
                Assert.True(!_receivedLockedForEditingMessages[0].IsLocked);
                var key = (ClientNavigationKey)_receivedLockedForEditingMessages[0].NewParameters;
                Assert.Equal(navigationKey, key);
                _receivedLockedForEditingMessages.Clear();
            }
        }

        [Fact]
        public void TestCreation()
        {
            using (var host = new Host())
            {
            }
        }

        private ClientModel MakeRinsibleElk()
        {
            return new ClientModel
            {
                ClientId = 116,
                AddressFirstLine = "1 Dummy Road",
                AddressSecondLine = "London",
                AddressPostCode = "N1 1AA",
                Company = "RinsibleElk",
                EmailAddress = "rinsible.elk@gmail.com",
                Title = "Sir",
                FirstName = "Rinsible",
                LastName = "Elk",
                PhoneNumber1 = "07193265784",
                Status = ClientStatus.Active,
                Id = 116
            };
        }

        [Fact]
        public void TestNavigateToExistingClient()
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                Assert.Equal(Experience.Clients, host.ViewModel.Experience);
                Assert.True(host.ViewModel.ShowActionsPad);
                Assert.Equal(ClientsPageState.CanEdit | ClientsPageState.CanDelete, host.ViewModel.State);
            }
        }

        [Fact]
        public void TestEmailExistingClient()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanEdit);
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                Assert.True(host.ViewModel.CanEdit);
                host.EmailClient();
            }
        }

        [Fact]
        public void TestExistingClient_AddNewJob_SendsAddNewJobEvent()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanEdit);
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.AddNewJob();
            }
        }

        [Fact]
        public void TestDeleteExistingClient_UserSaysYes()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanDelete);
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.DeleteClient(MessageDialogResult.Affirmative);
            }
        }

        [Fact]
        public void TestDeleteExistingClient_UserSaysNo()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanDelete);
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.DeleteClient(MessageDialogResult.Negative);
            }
        }

        [Fact]
        public void TestStartEditingExistingClient()
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
            }
        }

        [Theory]
        [InlineData("LastName", "")]
        [InlineData("AddressFirstLine", "")]
        [InlineData("EmailAddress", "jack.bauer@@@yahoo.com")]
        [InlineData("PhoneNumber1", "asbgg")]
        [InlineData("PhoneNumber1", "dfgdf")]
        [InlineData("PhoneNumber2", "edbdj")]
        [InlineData("PhoneNumber3", "abcde")]
        public void TestInvalidInput_SendsCannotSave(string propertyName, object newValue)
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.ExpectCanUndoPublish();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                host.ExpectCanUndo(true);
                Assert.Equal(ClientsPageState.CanEdit | ClientsPageState.CanDiscard | ClientsPageState.UnderEdit, host.ViewModel.State);
                Assert.False(host.CanSave);
                Assert.False(host.ViewModel.CanSave);
            }
        }

        [Theory]
        [InlineData("LastName", "Moose")]
        [InlineData("AddressFirstLine", "1 Yemen Road")]
        [InlineData("EmailAddress", "jack.bauer@yahoo.com")]
        [InlineData("PhoneNumber1", "07192567842")]
        [InlineData("PhoneNumber2", "07192567842")]
        [InlineData("PhoneNumber3", "07192567842")]
        public void TestValidInput_CanSave(string propertyName, object newValue)
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.ExpectCanUndoPublish();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                host.ExpectCanUndo(true);
                Assert.Equal(ClientsPageState.CanSave | ClientsPageState.CanEdit | ClientsPageState.CanDiscard | ClientsPageState.UnderEdit, host.ViewModel.State);
                Assert.True(host.CanSave);
                Assert.True(host.ViewModel.CanSave);
            }
        }

        [Theory]
        [InlineData("LastName", "")]
        [InlineData("AddressFirstLine", "")]
        [InlineData("EmailAddress", "jack.bauer@@@yahoo.com")]
        [InlineData("PhoneNumber1", "asbgg")]
        [InlineData("PhoneNumber1", "dfgdf")]
        [InlineData("PhoneNumber2", "edbdj")]
        [InlineData("PhoneNumber3", "abcde")]
        public void TestUndoAfterInvalidInput_SendsCanSaveCannotUndoAndCanRedo(string propertyName, object newValue)
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.ExpectCanUndoPublish();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                host.ExpectCanUndo(true);
                host.ExpectCanUndoPublish();
                host.Undo();
                host.ExpectCanUndo(false);
                host.ExpectCanRedo(true);
                Assert.True(host.CanSave);
            }
        }

        [Theory]
        [InlineData("LastName", "Moose")]
        [InlineData("AddressFirstLine", "1 Yemen Road")]
        [InlineData("EmailAddress", "jack.bauer@yahoo.com")]
        [InlineData("PhoneNumber1", "07192567842")]
        [InlineData("PhoneNumber2", "07192567842")]
        [InlineData("PhoneNumber3", "07192567842")]
        public void TestUndoAfterValidInput_SendsCanSaveCannotUndoAndCanRedo(string propertyName, object newValue)
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.ExpectCanUndoPublish();
                typeof(ClientModel).GetProperty(propertyName)
                    .SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { newValue });
                host.ExpectCanUndo(true);
                host.ExpectCanUndoPublish();
                host.Undo();
                host.ExpectCanUndo(false);
                host.ExpectCanRedo(true);
                Assert.True(host.CanSave);
            }
        }

        [Fact]
        public void TestDiscardNoChanges()
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.Discard(new ClientNavigationKey(rinsibleElk.ClientId));
            }
        }

        [Theory]
        [InlineData("LastName", "Moose", "AddressFirstLine", "1 Yemen Road")]
        [InlineData("EmailAddress", "jack.bauer@yahoo.com", "PhoneNumber1", "07192567842")]
        [InlineData("PhoneNumber2", "07192567842", "PhoneNumber3", "07192567842")]
        public void TestDiscardAfterChanges(string property1, object value1, string property2, object value2)
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.StartEditing();
                host.ExpectCanUndoPublish();
                typeof(ClientModel).GetProperty(property1).SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { value1 });
                host.ExpectCanUndo(true);
                typeof(ClientModel).GetProperty(property2).SetMethod.Invoke(host.ViewModel.SelectedItem, new object[1] { value2 });
                Assert.True(host.CanSave);
                host.Discard(new ClientNavigationKey(rinsibleElk.ClientId));
            }
        }

        [Fact]
        public void TestUnsubscribeFromEventsOnNavigatedFrom()
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.NavigateFrom();
            }
        }

        [Fact]
        public void TestNavigateToNewClient()
        {
            using (var host = new Host())
            {
                host.NavigateToNewClient();
            }
        }

        [Fact]
        public void TestInvoiceClient()
        {
            using (var host = new Host())
            {
                var rinsibleElk = MakeRinsibleElk();
                host.NavigateToExisting(rinsibleElk);
                host.InvoiceClient();
            }
        }
    }
}
