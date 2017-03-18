using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockDeadfileDialogCoordinator : IDeadfileDialogCoordinator
    {
        public Queue<MessageDialogResult> DialogResults = new Queue<MessageDialogResult>();

        public Task<MessageDialogResult> ConfirmDeleteAsync(object viewModel, string title, string message)
        {
            if (DialogResults.Count > 0)
                return Task.FromResult(DialogResults.Dequeue());
            return Task.FromResult(MessageDialogResult.Affirmative);
        }

        public Task ShowMessageAsync(object viewModel, string title, string message)
        {
            return Task.CompletedTask;
        }

        public Task<IDeadfileProgressController> ShowProgressAsync(object viewModel, string title, string message)
        {
            return Task.FromResult((IDeadfileProgressController)new MockDeadfileProgressController());
        }

        private class MockDeadfileProgressController : IDeadfileProgressController
        {
            public async Task CloseAsync()
            {
                await Task.FromResult<string>(null);
            }
        }
    }
}
