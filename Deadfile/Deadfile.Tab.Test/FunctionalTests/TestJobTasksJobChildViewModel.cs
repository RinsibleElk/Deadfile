using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Browser;
using MahApps.Metro.Controls.Dialogs;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestJobTasksJobChildViewModel
    {
        [Theory]
        [InlineData(null)]
        [InlineData(JobTaskPriority.High)]
        [InlineData(JobTaskPriority.Low)]
        [InlineData(JobTaskPriority.Medium)]
        public void TestAddTask(JobTaskPriority? priority)
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something.", null, priority);

                // Check that there is one job task saved.
                Assert.Equal(1, setup.Repository.GetJobTasksForJob(setup.JobsPageViewModel.SelectedItem.JobId, null).Count());
            }
        }

        [Theory]
        [InlineData(null, JobTaskPriority.Low)]
        [InlineData(null, JobTaskPriority.Medium)]
        [InlineData(null, JobTaskPriority.High)]
        [InlineData(JobTaskPriority.High, JobTaskPriority.Low)]
        [InlineData(JobTaskPriority.High, JobTaskPriority.Medium)]
        [InlineData(JobTaskPriority.Low, JobTaskPriority.Medium)]
        [InlineData(JobTaskPriority.Low, JobTaskPriority.High)]
        [InlineData(JobTaskPriority.Medium, JobTaskPriority.Low)]
        [InlineData(JobTaskPriority.Medium, JobTaskPriority.High)]
        public void TestAddTaskThenEditAndChangePriority(JobTaskPriority? priority1, JobTaskPriority priority2)
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something", "Some notes about this task", priority1);
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                Assert.True(setup.JobTasksJobChildViewModel.EditCommand.CanExecute(null));
                setup.JobTasksJobChildViewModel.EditCommand.Execute(null);
                while (setup.JobTasksJobChildViewModel.SelectedItem.Priority != priority2)
                {
                    Assert.True(setup.JobTasksJobChildViewModel.TogglePriorityCommand.CanExecute(null));
                    setup.JobTasksJobChildViewModel.TogglePriorityCommand.Execute(null);
                }
                Assert.True(setup.JobTasksJobChildViewModel.SaveCommand.CanExecute(null));
                setup.JobTasksJobChildViewModel.SaveCommand.Execute(null);

                // Check that there is one job task saved.
                Assert.Equal(1, setup.Repository.GetJobTasksForJob(setup.JobsPageViewModel.SelectedItem.JobId, null).Count());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestDeleteTask(bool allowDelete)
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something", "Some notes about this task", null);

                // Delete Job Task.
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                Assert.True(setup.JobTasksJobChildViewModel.DeleteCommand.CanExecute(null));
                var dialogResult = allowDelete ? MessageDialogResult.Affirmative : MessageDialogResult.Negative;
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(dialogResult);
                setup.JobTasksJobChildViewModel.DeleteCommand.Execute(null);

                // Check that there are zero job task saved.
                var remaining = allowDelete ? 0 : 1;
                Assert.Equal(remaining, setup.Repository.GetJobTasksForJob(setup.JobsPageViewModel.SelectedItem.JobId, null).Count());
            }
        }

        [Fact]
        public void TestCompleteTask()
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something", "Some notes about this task", null);

                // Complete Job Task.
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                Assert.True(setup.JobTasksJobChildViewModel.EditCommand.CanExecute(null));
                setup.JobTasksJobChildViewModel.EditCommand.Execute(null);
                Assert.Equal(JobTaskState.Active, setup.JobTasksJobChildViewModel.SelectedItem.State);
                setup.JobTasksJobChildViewModel.SelectedItem.State = JobTaskState.Completed;
                Assert.True(setup.JobTasksJobChildViewModel.SaveCommand.CanExecute(null));
                setup.JobTasksJobChildViewModel.SaveCommand.Execute(null);
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                Assert.Equal(JobTaskState.Completed, setup.JobTasksJobChildViewModel.SelectedItem.State);
            }
        }

        [Fact]
        public void TestMakeTaskBillable()
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something", "Some notes about this task", null);

                // Complete Job Task. Visibility of button linked to can delete.
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                var jobId = setup.JobTasksJobChildViewModel.SelectedItem.JobId;
                var jobTaskId = setup.JobTasksJobChildViewModel.SelectedItem.JobTaskId;
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Affirmative);
                Assert.True(setup.JobTasksJobChildViewModel.CanDeleteItem);
                Assert.True(setup.JobTasksJobChildViewModel.MakeJobTaskBillableCommand.CanExecute(null));
                var numBillableHoursBefore = setup.Repository.GetBillableHoursForJob(jobId, null).Count();
                setup.JobTasksJobChildViewModel.MakeJobTaskBillableCommand.Execute(null);

                // Check that it raised the dialog to ask the user.
                // Check that the job task has been deleted.
                // There should also be a new billable hour model.
                // And we should have navigated to that model.
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                Assert.Empty(setup.Repository.GetJobTasksForJob(jobId, null).Where((jobTask) => jobTask.JobTaskId == jobTaskId));
                var numBillableHoursAfter = setup.Repository.GetBillableHoursForJob(jobId, null).Count();
                Assert.Equal(numBillableHoursBefore + 1, numBillableHoursAfter);
                Assert.Equal(setup.JobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(setup.BillableHoursJobChildViewModel, setup.JobsPageViewModel.JobChildViewModel);
                var lastBillableHourId = setup.Repository.GetBillableHoursForJob(jobId, null).Select((billableHour) => billableHour.BillableHourId).Max();
                Assert.NotNull(setup.BillableHoursJobChildViewModel.SelectedItem);
                Assert.Equal(lastBillableHourId, setup.BillableHoursJobChildViewModel.SelectedItem.BillableHourId);
            }
        }

        [Fact]
        public void TestMakeTaskBillable_UserCancels()
        {
            using (var setup = new MockSetup(true))
            {
                // Navigate to a suitable job.
                setup.BrowseToPrivetDrive();

                // Add Job Task.
                setup.AddJobTask("Do something", "Some notes about this task", null);

                // Complete Job Task. Visibility of button linked to can delete.
                setup.JobTasksJobChildViewModel.SelectedItem = setup.JobTasksJobChildViewModel.Items[0];
                var jobId = setup.JobTasksJobChildViewModel.SelectedItem.JobId;
                var jobTaskId = setup.JobTasksJobChildViewModel.SelectedItem.JobTaskId;
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Negative);
                Assert.True(setup.JobTasksJobChildViewModel.CanDeleteItem);
                Assert.True(setup.JobTasksJobChildViewModel.MakeJobTaskBillableCommand.CanExecute(null));
                var numBillableHoursBefore = setup.Repository.GetBillableHoursForJob(jobId, null).Count();
                setup.JobTasksJobChildViewModel.MakeJobTaskBillableCommand.Execute(null);

                // Check that it raised the dialog to ask the user.
                // Check that nothing has changed since the user cancelled.
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                var numBillableHoursAfter = setup.Repository.GetBillableHoursForJob(jobId, null).Count();
                Assert.Equal(numBillableHoursBefore, numBillableHoursAfter);
                Assert.Equal(setup.JobsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.Equal(setup.JobTasksJobChildViewModel, setup.JobsPageViewModel.JobChildViewModel);
                Assert.Equal(jobTaskId, setup.JobTasksJobChildViewModel.SelectedItem.JobTaskId);
            }
        }
    }
}
