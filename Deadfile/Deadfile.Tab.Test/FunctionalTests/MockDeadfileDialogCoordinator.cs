using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockDeadfileDialogCoordinator : IDeadfileDialogCoordinator
    {
        private readonly Queue<MessageDialogResult> _dialogResults = new Queue<MessageDialogResult>();

        public void EnqueueDialogResult(MessageDialogResult result)
        {
            _dialogResults.Enqueue(result);
        }

        public void CheckNoMoreQueuedDialogResults()
        {
            Assert.Empty(_dialogResults);
        }

        public Task<MessageDialogResult> ConfirmDeleteAsync(object viewModel, string title, string message)
        {
            if (_dialogResults.Count > 0)
                return Task.FromResult(_dialogResults.Dequeue());
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
