using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestUnbilledJobsPageViewModel
    {
        [Fact]
        public void TestNavigateToReport()
        {
            using (var setup = new MockSetup(true))
            {
                Assert.Equal(setup.HomePageViewModel, setup.TabViewModel.ContentArea);
                setup.HomePageViewModel.UnbilledJobs();
                Assert.Equal(setup.UnbilledJobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(Experience.UnbilledJobs, setup.UnbilledJobsPageViewModel.Experience);
                Assert.Equal("Unbilled Jobs", setup.TabViewModel.DisplayName);
            }
        }

        [Fact]
        public void TestNavigateToClientForUnbilledJob()
        {
            using (var setup = new MockSetup(true))
            {
                Assert.Equal(setup.HomePageViewModel, setup.TabViewModel.ContentArea);
                setup.HomePageViewModel.UnbilledJobs();
                Assert.Equal(setup.UnbilledJobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(2, setup.UnbilledJobsPageViewModel.Items.Count);
                Assert.Equal("32 Windsor Gardens", setup.UnbilledJobsPageViewModel.Items[0].AddressFirstLine);
                Assert.Equal(5, setup.UnbilledJobsPageViewModel.Items[0].ClientId);
                Assert.Equal(6, setup.UnbilledJobsPageViewModel.Items[0].JobId);
                Assert.Equal("Nicole Bryant", setup.UnbilledJobsPageViewModel.Items[0].FullName);
                Assert.Equal("Nicole Bryant (£250.00, 0 hours)", setup.UnbilledJobsPageViewModel.Items[0].HeaderString);
                Assert.Equal(250, setup.UnbilledJobsPageViewModel.Items[0].UnbilledAmount);
                Assert.Equal(0, setup.UnbilledJobsPageViewModel.Items[0].UnbilledHours);
                setup.UnbilledJobsPageViewModel.NavigateToClient.Execute(setup.UnbilledJobsPageViewModel.Items[0]);
                Assert.Equal(setup.ClientsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(5, setup.ClientsPageViewModel.SelectedItem.ClientId);
            }
        }

        [Fact]
        public void TestNavigateToJobForUnbilledJob()
        {
            using (var setup = new MockSetup(true))
            {
                Assert.Equal(setup.HomePageViewModel, setup.TabViewModel.ContentArea);
                setup.HomePageViewModel.UnbilledJobs();
                Assert.Equal(setup.UnbilledJobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(2, setup.UnbilledJobsPageViewModel.Items.Count);
                Assert.Equal("221B Baker Street", setup.UnbilledJobsPageViewModel.Items[1].AddressFirstLine);
                Assert.Equal(5, setup.UnbilledJobsPageViewModel.Items[1].ClientId);
                Assert.Equal(5, setup.UnbilledJobsPageViewModel.Items[1].JobId);
                Assert.Equal("Nicole Bryant", setup.UnbilledJobsPageViewModel.Items[1].FullName);
                Assert.Equal("Nicole Bryant (£0.00, 2 hours)", setup.UnbilledJobsPageViewModel.Items[1].HeaderString);
                Assert.Equal(0, setup.UnbilledJobsPageViewModel.Items[1].UnbilledAmount);
                Assert.Equal(2, setup.UnbilledJobsPageViewModel.Items[1].UnbilledHours);
                setup.UnbilledJobsPageViewModel.NavigateToJob.Execute(setup.UnbilledJobsPageViewModel.Items[1]);
                Assert.Equal(setup.JobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(5, setup.JobsPageViewModel.SelectedItem.JobId);
            }
        }
    }
}
