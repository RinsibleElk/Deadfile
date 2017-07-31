using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using MahApps.Metro.Controls.Dialogs;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestJobsPageViewModel
    {
        [Fact]
        public void TestDeleteJob()
        {
            using (var setup = new MockSetup(true))
            {
                setup.BrowseToCemeteryRidge();
                Assert.Equal(setup.JobsActionsPadViewModel, setup.TabViewModel.ActionsPad);
                Assert.True(setup.JobsActionsPadViewModel.CanDeleteItem);
                Assert.True(setup.JobsActionsPadViewModel.DeleteItemIsVisible);
                var jobId = setup.JobsPageViewModel.SelectedItem.JobId;
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Affirmative);
                setup.JobsActionsPadViewModel.DeleteItem();
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                var jobModel = setup.Repository.GetJobById(jobId);
                Assert.Equal(JobStatus.Cancelled, jobModel.Status);
            }
        }

        [Fact]
        public void TestDeleteJob_UserCancelled()
        {
            using (var setup = new MockSetup(true))
            {
                setup.BrowseToCemeteryRidge();
                Assert.Equal(setup.JobsActionsPadViewModel, setup.TabViewModel.ActionsPad);
                Assert.True(setup.JobsActionsPadViewModel.CanDeleteItem);
                Assert.True(setup.JobsActionsPadViewModel.DeleteItemIsVisible);
                var jobId = setup.JobsPageViewModel.SelectedItem.JobId;
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Negative);
                setup.JobsActionsPadViewModel.DeleteItem();
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                var jobModel = setup.Repository.GetJobById(jobId);
                Assert.Equal(JobStatus.Active, jobModel.Status);
            }
        }
    }
}
