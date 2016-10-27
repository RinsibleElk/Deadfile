using System;
using System.Linq;
using System.Windows;
using Deadfile.Content.Clients;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Xunit;
using Moq;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Test
{
    public class TestClientsActionsPadViewModel
    {
        private class Host : IDisposable
        {
            private static readonly NavigationContext NavigateToNavigationContext = CreateNavigateToNavigationContext();

            private static NavigationContext CreateNavigateToNavigationContext()
            {
                var uri = new Uri("ClientsActionsPad", UriKind.Relative);

                var navigationJournalMock = new Mock<IRegionNavigationJournal>();
                var navigationServiceMock = new Mock<IRegionNavigationService>();

                IRegion region = new Region();
                navigationServiceMock.SetupGet(n => n.Region).Returns(region);
                navigationServiceMock.SetupGet(x => x.Journal).Returns(navigationJournalMock.Object);

                return new NavigationContext(navigationServiceMock.Object, uri);
            }
            public readonly Mock<IEventAggregator> EventAggregatorMock;
            public readonly Mock<EditItemEvent> EditClientEventMock;
            public readonly ClientsActionsPadViewModel ViewModel;
            public readonly LockedForEditingEvent LockedForEditingEvent;
            public readonly CanSaveEvent CanSaveEvent;
            public readonly CanEditEvent CanEditEvent;
            public Host()
            {
                EventAggregatorMock = new Mock<IEventAggregator>();
                EditClientEventMock = new Mock<EditItemEvent>();
                EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditItemEvent>())
                    .Returns(EditClientEventMock.Object)
                    .Verifiable();
                ViewModel = new ClientsActionsPadViewModel(EventAggregatorMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                CanSaveEvent = new CanSaveEvent();
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
                    .Setup((ea) => ea.GetEvent<CanEditEvent>())
                    .Returns(CanEditEvent)
                    .Verifiable();
                ViewModel.OnNavigatedTo(NavigateToNavigationContext);
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
                    .Setup((ev) => ev.Publish(EditAction.StartEditing))
                    .Verifiable();
                host.ViewModel.EditItemCommand.Execute(null);

                // Checks.
            }
        }

        [Fact]
        public void TestDefaultButtonVisibilities()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var viewModel = new ClientsActionsPadViewModel(eventAggregatorMock.Object);

            // Checks.
            Assert.Equal(Visibility.Visible, viewModel.AddItemVisibility);
            Assert.Equal(Visibility.Visible, viewModel.EditItemVisibility);
            Assert.Equal(Visibility.Collapsed, viewModel.SaveItemVisibility);
            Assert.Equal(Visibility.Visible, viewModel.DeleteItemVisibility);
            Assert.Equal(Visibility.Collapsed, viewModel.DiscardItemVisibility);
            eventAggregatorMock.VerifyAll();
        }
    }
}
