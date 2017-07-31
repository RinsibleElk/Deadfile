using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.JobChildren;
using MahApps.Metro.Controls.Dialogs;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestExpensesJobChildViewModel
    {
        [Fact]
        public void TestDeleteExpense()
        {
            using (var setup = new MockSetup(true))
            {
                setup.BrowseToWindsorGardens();
                setup.JobsPageViewModel.SelectedJobChild = JobChildExperience.Expenses;
                Assert.Equal(2, setup.ExpensesJobChildViewModel.Items.Count);
                Assert.Equal(250, setup.ExpensesJobChildViewModel.Items[0].NetAmount);
                setup.ExpensesJobChildViewModel.SelectedItem = setup.ExpensesJobChildViewModel.Items[0];
                Assert.True(setup.ExpensesJobChildViewModel.CanDeleteItem);
                setup.ExpensesJobChildViewModel.DeleteCommand.CanExecute(null);
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Affirmative);
                setup.ExpensesJobChildViewModel.DeleteCommand.Execute(null);
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                Assert.Empty(setup.Repository.GetExpensesForJob(setup.JobsPageViewModel.SelectedItem.JobId, null));
            }
        }

        [Fact]
        public void TestDeleteExpense_UserCancels()
        {
            using (var setup = new MockSetup(true))
            {
                setup.BrowseToWindsorGardens();
                setup.JobsPageViewModel.SelectedJobChild = JobChildExperience.Expenses;
                Assert.Equal(2, setup.ExpensesJobChildViewModel.Items.Count);
                Assert.Equal(250, setup.ExpensesJobChildViewModel.Items[0].NetAmount);
                setup.ExpensesJobChildViewModel.SelectedItem = setup.ExpensesJobChildViewModel.Items[0];
                Assert.True(setup.ExpensesJobChildViewModel.CanDeleteItem);
                setup.ExpensesJobChildViewModel.DeleteCommand.CanExecute(null);
                setup.DeadfileDialogCoordinator.EnqueueDialogResult(MessageDialogResult.Negative);
                setup.ExpensesJobChildViewModel.DeleteCommand.Execute(null);
                setup.DeadfileDialogCoordinator.CheckNoMoreQueuedDialogResults();
                Assert.NotEmpty(setup.Repository.GetExpensesForJob(setup.JobsPageViewModel.SelectedItem.JobId, null));
            }
        }
    }
}