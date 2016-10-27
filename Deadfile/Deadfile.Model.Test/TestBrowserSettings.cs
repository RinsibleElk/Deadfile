using System;
using System.Linq;
using Deadfile.Model.Browser;
using Xunit;

namespace Deadfile.Model.Test
{
    public class TestBrowserSettings
    {
        [Fact]
        public void TestInvokesRefreshOnFilterChange()
        {
            var numTimesRefreshCalled = 0;
            var browserSettings = new BrowserSettings();
            browserSettings.Refresh += (s, e) => ++numTimesRefreshCalled;
            browserSettings.FilterText = "Bru";
            Assert.Equal(1, numTimesRefreshCalled);
        }

        [Fact]
        public void TestInvokesRefreshOnModeChange()
        {
            var numTimesRefreshCalled = 0;
            var browserSettings = new BrowserSettings();
            browserSettings.Refresh += (s, e) => ++numTimesRefreshCalled;
            browserSettings.Mode = BrowserMode.Invoice;
            Assert.Equal(1, numTimesRefreshCalled);
        }

        [Fact]
        public void TestInvokesRefreshOnSortChange()
        {
            var numTimesRefreshCalled = 0;
            var browserSettings = new BrowserSettings();
            browserSettings.Refresh += (s, e) => ++numTimesRefreshCalled;
            browserSettings.Sort = BrowserSort.ClientLastName;
            Assert.Equal(1, numTimesRefreshCalled);
        }
    }
}
