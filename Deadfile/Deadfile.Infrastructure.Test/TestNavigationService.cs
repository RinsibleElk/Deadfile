using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Moq;
using Xunit;

namespace Deadfile.Infrastructure.Test
{
    public class TestNavigationService
    {
        private class NavigationAware : INavigationAware
        {
            public int NavigatedToCount = 0;
            public int NavigatedFromCount = 0;
            public void OnNavigatedTo(object parameters)
            {
                ++NavigatedToCount;
            }

            public void OnNavigatedFrom()
            {
                ++NavigatedFromCount;
            }
        }

        private class NavigationUnawareJournaled : IJournaled
        {
        }

        private class NavigationAwareJournaled : INavigationAware, IJournaled
        {
            public int NavigatedToCount = 0;
            public int NavigatedFromCount = 0;
            public void OnNavigatedTo(object parameters)
            {
                ++NavigatedToCount;
            }

            public void OnNavigatedFrom()
            {
                ++NavigatedFromCount;
            }
        }

        private class NavigationUnaware
        {
        }

        private class Host
        {
            public object Target { get; set; }
        }

        [Fact]
        public void TestCantGoBackOrForwardToStart()
        {
            var simpleContainerMock = new Mock<INavigationContainer>();
            var navigationService = new NavigationService(simpleContainerMock.Object);
            Assert.False(navigationService.CanGoBack);
            Assert.False(navigationService.CanGoForward);
            simpleContainerMock.VerifyAll();
        }

        private const string NavigationKey = "TestNavigationKey";

        [Fact]
        public void TestCannotGoBackAfterNavigationToJournaled()
        {
            var simpleContainerMock = new Mock<INavigationContainer>();
            var navigationService = new NavigationService(simpleContainerMock.Object);
            var host = new Host();
            var target = new NavigationUnawareJournaled();
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target)
                .Verifiable();
            var propertiesChanged = new List<string>();
            navigationService.PropertyChanged += (s, e) =>
            {
                propertiesChanged.Add(e.PropertyName);
            };
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            Assert.Same(target, host.Target);
            Assert.False(navigationService.CanGoBack);
            Assert.False(navigationService.CanGoForward);
            Assert.Equal(0, propertiesChanged.Count);
        }

        [Fact]
        public void TestCanGoBackAfterNavigationToSecondJournaled()
        {
            var simpleContainerMock = new Mock<INavigationContainer>();
            var navigationService = new NavigationService(simpleContainerMock.Object);
            var host = new Host();
            var target1 = new NavigationUnawareJournaled();
            var target2 = new NavigationUnawareJournaled();
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target1)
                .Verifiable();
            var propertiesChanged = new List<string>();
            navigationService.PropertyChanged += (s, e) =>
            {
                propertiesChanged.Add(e.PropertyName);
            };
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target2)
                .Verifiable();
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            Assert.Same(target2, host.Target);
            Assert.True(navigationService.CanGoBack);
            Assert.False(navigationService.CanGoForward);
            Assert.Equal(1, propertiesChanged.Count);
            Assert.Equal(nameof(navigationService.CanGoBack), propertiesChanged[0]);
        }

        [Fact]
        public void TestCanGoForwardAfterNavigationBack()
        {
            var simpleContainerMock = new Mock<INavigationContainer>();
            var navigationService = new NavigationService(simpleContainerMock.Object);
            var host = new Host();
            var target1 = new NavigationUnawareJournaled();
            var target2 = new NavigationUnawareJournaled();
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target1)
                .Verifiable();
            var propertiesChanged = new List<string>();
            navigationService.PropertyChanged += (s, e) =>
            {
                propertiesChanged.Add(e.PropertyName);
            };
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target2)
                .Verifiable();
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            navigationService.GoBack();
            Assert.Same(target1, host.Target);
            Assert.False(navigationService.CanGoBack);
            Assert.True(navigationService.CanGoForward);
            Assert.Equal(3, propertiesChanged.Count);
            Assert.Equal(nameof(navigationService.CanGoBack), propertiesChanged[0]);
            Assert.Equal(nameof(navigationService.CanGoBack), propertiesChanged[1]);
            Assert.Equal(nameof(navigationService.CanGoForward), propertiesChanged[2]);
        }

        [Fact]
        public void TestCannotGoBackAfterNavigationToUnjournaled()
        {
            var simpleContainerMock = new Mock<INavigationContainer>();
            var navigationService = new NavigationService(simpleContainerMock.Object);
            var host = new Host();
            var target = new NavigationUnaware();
            simpleContainerMock
                .Setup((sc) => sc.GetInstance(NavigationKey))
                .Returns(target)
                .Verifiable();
            var propertiesChanged = new List<string>();
            navigationService.PropertyChanged += (s, e) =>
            {
                propertiesChanged.Add(e.PropertyName);
            };
            navigationService.RequestNavigate(host, nameof(host.Target), NavigationKey, null);
            Assert.Same(target, host.Target);
            Assert.False(navigationService.CanGoBack);
            Assert.False(navigationService.CanGoForward);
            Assert.Equal(0, propertiesChanged.Count);
        }
    }
}
