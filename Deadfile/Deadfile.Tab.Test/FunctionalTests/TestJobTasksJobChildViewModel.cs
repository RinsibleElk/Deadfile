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
    }
}
