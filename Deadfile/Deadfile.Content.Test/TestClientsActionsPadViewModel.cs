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
            public readonly Mock<IDeadfileNavigationService> NavigationServiceMock;
            public readonly Mock<EditClientEvent> EditClientEventMock;
            public readonly ClientsActionsPadViewModel ViewModel;
            public readonly LockedForEditingEvent LockedForEditingEvent;
            public Host()
            {
                EventAggregatorMock = new Mock<IEventAggregator>();
                NavigationServiceMock = new Mock<IDeadfileNavigationService>();
                EditClientEventMock = new Mock<EditClientEvent>();
                EventAggregatorMock
                    .Setup((eventAggregator) => eventAggregator.GetEvent<EditClientEvent>())
                    .Returns(EditClientEventMock.Object)
                    .Verifiable();
                ViewModel = new ClientsActionsPadViewModel(EventAggregatorMock.Object, NavigationServiceMock.Object);
                LockedForEditingEvent = new LockedForEditingEvent();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<LockedForEditingEvent>())
                    .Returns(LockedForEditingEvent)
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
                    .Setup((ev) => ev.Publish())
                    .Verifiable();
                host.ViewModel.EditClientCommand.Execute(null);

                // Checks.
            }
        }

        [Fact]
        public void TestDefaultButtonVisibilities()
        {
            // Setup.
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var navigationServiceMock = new Mock<IDeadfileNavigationService>();
            var viewModel = new ClientsActionsPadViewModel(eventAggregatorMock.Object, navigationServiceMock.Object);

            // Checks.
            Assert.Equal(Visibility.Visible, viewModel.AddClientVisibility);
            Assert.Equal(Visibility.Visible, viewModel.EditClientVisibility);
            Assert.Equal(Visibility.Collapsed, viewModel.SaveClientVisibility);
            Assert.Equal(Visibility.Visible, viewModel.DeleteClientVisibility);
            Assert.Equal(Visibility.Collapsed, viewModel.DiscardClientVisibility);
            eventAggregatorMock.VerifyAll();
        }
    }
}
