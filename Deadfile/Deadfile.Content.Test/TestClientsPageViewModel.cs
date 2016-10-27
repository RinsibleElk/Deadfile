using System;
using System.Linq;
using Deadfile.Content.Clients;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Moq;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestClientsPageViewModel
    {
        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<IDeadfileNavigationService> NavigationServiceMock = new Mock<IDeadfileNavigationService>();
            public readonly Mock<IDeadfileRepository> DeadfileRepositoryMock = new Mock<IDeadfileRepository>();
            public readonly NavigationParameterMapper NavigationParameterMapper = new NavigationParameterMapper();
            public readonly ClientsPageViewModel ViewModel;
            public readonly SelectedItemEvent SelectedItemEvent = new SelectedItemEvent();
            public readonly UndoEvent UndoEvent = new UndoEvent();
            public readonly RedoEvent RedoEvent = new RedoEvent();
            public readonly EditItemEvent EditItemEvent = new EditItemEvent();
            public readonly CanEditEvent CanEditEvent = new CanEditEvent();
            public Host()
            {
                ViewModel = new ClientsPageViewModel(EventAggregatorMock.Object, NavigationServiceMock.Object, DeadfileRepositoryMock.Object, NavigationParameterMapper);
            }

            public void VerifyAll()
            {
                EventAggregatorMock.VerifyAll();
                NavigationServiceMock.VerifyAll();
                DeadfileRepositoryMock.VerifyAll();
            }

            public void Dispose()
            {
                VerifyAll();
            }
        }

        [Fact]
        public void TestCreation()
        {
            using (var host = new Host())
            {
            }
        }

        private static NavigationContext CreateNavigateToNavigationContext(NavigationParameterMapper mapper, int selectedIndex)
        {
            var uri = new Uri("ClientsPage", UriKind.Relative);

            var navigationJournalMock = new Mock<IRegionNavigationJournal>();
            var navigationServiceMock = new Mock<IRegionNavigationService>();

            IRegion region = new Region();
            navigationServiceMock.SetupGet(n => n.Region).Returns(region);
            navigationServiceMock.SetupGet(x => x.Journal).Returns(navigationJournalMock.Object);

            var parameters = mapper.ConvertToNavigationParameters(selectedIndex);

            return new NavigationContext(navigationServiceMock.Object, uri, parameters);
        }

        private static void NavigateTo(Host host, ClientModel model)
        {
            // Expect to receive journal via navigation event and be asked to move the actions pad to the clients experience
            var navigationEventMock = new Mock<NavigationEvent>();
            navigationEventMock
                .Setup((ns) => ns.Publish(It.IsAny<IRegionNavigationJournal>()))
                .Verifiable();
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<NavigationEvent>())
                .Returns(navigationEventMock.Object)
                .Verifiable();
            host.NavigationServiceMock
                .Setup((ns) => ns.NavigateActionsTo(Experience.Clients))
                .Verifiable();

            // Expect subscriptions to SelectedItemEvent, UndoEvent, RedoEvent, EditItemEvent
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<SelectedItemEvent>())
                .Returns(host.SelectedItemEvent)
                .Verifiable();
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<UndoEvent>())
                .Returns(host.UndoEvent)
                .Verifiable();
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<RedoEvent>())
                .Returns(host.RedoEvent)
                .Verifiable();
            host.EventAggregatorMock
                .Setup((ea) => ea.GetEvent<EditItemEvent>())
                .Returns(host.EditItemEvent)
                .Verifiable();
            if (model.Id != ModelBase.NewModelId)
            {
                host.EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<CanEditEvent>())
                    .Returns(host.CanEditEvent)
                    .Verifiable();
                host.DeadfileRepositoryMock
                    .Setup((dr) => dr.GetClientById(model.Id))
                    .Returns(model)
                    .Verifiable();
            }
            host.ViewModel.OnNavigatedTo(CreateNavigateToNavigationContext(host.NavigationParameterMapper, model.Id));
            host.VerifyAll();
        }

        [Fact]
        public void TestNavigateToNewClient()
        {
            using (var host = new Host())
            {
                NavigateTo(host, new ClientModel());
            }
        }

        [Fact]
        public void TestNavigateToExistingClient()
        {
            using (var host = new Host())
            {
                Assert.False(host.ViewModel.CanEdit);
                var a = 0;
                var ce = false;
                host.CanEditEvent.Subscribe((b) =>
                {
                    ++a;
                    ce = b;
                });
                NavigateTo(host,
                    new ClientModel()
                    {
                        ClientId = 1,
                        AddressFirstLine = "1 Yemen Road",
                        LastName = "Johnson",
                        PhoneNumber1 = "07544454514"
                    });
                Assert.Equal(1, a);
                Assert.True(ce);
                Assert.True(host.ViewModel.CanEdit);
            }
        }
    }
}
