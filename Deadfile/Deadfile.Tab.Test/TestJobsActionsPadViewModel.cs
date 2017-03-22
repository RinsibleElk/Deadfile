using System;
using System.Collections.Generic;
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
            private readonly Mock<IEventAggregator> _eventAggregatorMock;
            private readonly List<EditActionMessage> _receivedEditActionMessages = new List<EditActionMessage>();
            private readonly EditActionEvent _editActionEvent = new EditActionEvent();
            public readonly JobsActionsPadViewModel ViewModel;
            private readonly PageStateEvent<JobsPageState> _pageStateEvent = new PageStateEvent<JobsPageState>();
            public Host()
            {
                _eventAggregatorMock = new Mock<IEventAggregator>();
                _editActionEvent.Subscribe((m) => _receivedEditActionMessages.Add(m));
                ViewModel = new JobsActionsPadViewModel(TabIdentity, _eventAggregatorMock.Object);
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<PageStateEvent<JobsPageState>>())
                    .Returns(_pageStateEvent)
                    .Verifiable();

                // Navigate to it.
                ViewModel.OnNavigatedTo(null);

                // Initial state.
                _pageStateEvent.Publish(JobsPageState.CanDelete | JobsPageState.CanEdit);
            }

            public void Dispose()
            {
                _eventAggregatorMock.VerifyAll();
                Assert.Equal(0, _receivedEditActionMessages.Count);
            }

            public void Edit()
            {
                Assert.True(ViewModel.PageState.HasFlag(JobsPageState.CanEdit));
                _eventAggregatorMock
                    .Setup((ea) => ea.GetEvent<EditActionEvent>())
                    .Returns(_editActionEvent)
                    .Verifiable();
                ViewModel.EditItem();
                Assert.Equal(1, _receivedEditActionMessages.Count);
                Assert.Equal(EditActionMessage.StartEditing, _receivedEditActionMessages[0]);
                _receivedEditActionMessages.Clear();
            }

            public void LockForEditing()
            {
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanDelete | JobsPageState.CanEdit);
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanEdit);
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanEdit | JobsPageState.CanDiscard);
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanEdit | JobsPageState.CanDiscard | JobsPageState.UnderEdit);
            }

            public void UnlockForEditing()
            {
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanDelete | JobsPageState.CanEdit | JobsPageState.CanDiscard | JobsPageState.UnderEdit);
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanDelete | JobsPageState.CanEdit | JobsPageState.UnderEdit);
                _pageStateEvent.Publish(JobsPageState.CanSave | JobsPageState.CanDelete | JobsPageState.CanEdit);
                _pageStateEvent.Publish(JobsPageState.CanDelete | JobsPageState.CanEdit);
            }

            public void CannotSave()
            {
                _pageStateEvent.Publish(ViewModel.PageState & ~JobsPageState.CanSave);
            }

            public void CannotDiscard()
            {
                _pageStateEvent.Publish(ViewModel.PageState & ~JobsPageState.CanDiscard);
            }
        }

        [Fact]
        public void TestDefaultButtonVisibilities()
        {
            // Setup.
            using (var host = new Host())
            {
                // Checks.
                Assert.True(host.ViewModel.EditItemIsVisible);
                Assert.False(host.ViewModel.SaveItemIsVisible);
                Assert.True(host.ViewModel.DeleteItemIsVisible);
                Assert.False(host.ViewModel.CanDiscardItem);
                Assert.False(host.ViewModel.DiscardItemIsVisible);
            }
        }

        [Fact]
        public void TestRaisesEditActionEvent()
        {
            // Setup.
            using (var host = new Host())
            {
                host.Edit();
            }
        }

        [Fact]
        public void TestSwitchToEditingMode_VisibilitiesChange()
        {
            // Setup.
            using (var host = new Host())
            {
                // Act.
                host.Edit();
                host.LockForEditing();

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
                host.Edit();
                host.LockForEditing();
                host.UnlockForEditing();

                // Checks.
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
                host.Edit();
                host.LockForEditing();
                host.CannotSave();

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
                host.Edit();
                host.LockForEditing();
                host.CannotSave();
                host.CannotDiscard();

                // Checks.
                Assert.True(host.ViewModel.SaveItemIsVisible);
                Assert.False(host.ViewModel.CanSaveItem);
                Assert.True(host.ViewModel.DiscardItemIsVisible);
                Assert.False(host.ViewModel.CanDiscardItem);
            }
        }
    }
}
