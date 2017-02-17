using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
using Deadfile.Tab.Tab;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test
{
    public class TestTabViewModel
    {
        private static readonly TabIdentity TabIdentity = new TabIdentity(1);

        private class Host : IDisposable
        {
            public readonly Mock<IEventAggregator> EventAggregatorMock = new Mock<IEventAggregator>();
            public readonly Mock<INavigationService> NavigationServiceMock = new Mock<INavigationService>();
            public readonly TabViewModel ViewModel;

            public readonly NavigateEvent NavigateEvent = new NavigateEvent();
            public readonly DisplayNameEvent DisplayNameEvent = new DisplayNameEvent();
            public readonly AddClientEvent AddClientEvent = new AddClientEvent();
            public readonly SelectedItemEvent SelectedItemEvent = new SelectedItemEvent();
            public readonly AddNewJobEvent AddNewJobEvent = new AddNewJobEvent();
            public readonly InvoiceClientEvent InvoiceClientEvent = new InvoiceClientEvent();

            public Host()
            {
                ViewModel = new TabViewModel(TabIdentity, EventAggregatorMock.Object, NavigationServiceMock.Object);
            }

            public void VerifyAll()
            {
                EventAggregatorMock.VerifyAll();
            }

            public void Dispose()
            {
                VerifyAll();
            }

            public void Activate()
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<NavigateEvent>())
                    .Returns(NavigateEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(DisplayNameEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddClientEvent>())
                    .Returns(AddClientEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SelectedItemEvent>())
                    .Returns(SelectedItemEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddNewJobEvent>())
                    .Returns(AddNewJobEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<InvoiceClientEvent>())
                    .Returns(InvoiceClientEvent)
                    .Verifiable();
                ViewModel.TestOnlyOnActivate();
            }

            public void Deactivate(bool close)
            {
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<NavigateEvent>())
                    .Returns(NavigateEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<DisplayNameEvent>())
                    .Returns(DisplayNameEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddClientEvent>())
                    .Returns(AddClientEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<SelectedItemEvent>())
                    .Returns(SelectedItemEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<AddNewJobEvent>())
                    .Returns(AddNewJobEvent)
                    .Verifiable();
                EventAggregatorMock
                    .Setup((ea) => ea.GetEvent<InvoiceClientEvent>())
                    .Returns(InvoiceClientEvent)
                    .Verifiable();
                ViewModel.TestOnlyOnDeactivate(close);
            }
        }

        [Fact]
        public void TestConstruction()
        {
            using (var host = new Host())
            {
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TestLifecycle(bool close)
        {
            using (var host = new Host())
            {
                host.Activate();
                host.Deactivate(close);
            }
        }
    }
}
