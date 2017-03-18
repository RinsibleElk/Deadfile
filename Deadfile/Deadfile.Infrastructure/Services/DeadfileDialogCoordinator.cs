using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace Deadfile.Infrastructure.Services
{
    public class DeadfileDialogCoordinator : IDeadfileDialogCoordinator
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public DeadfileDialogCoordinator(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }
        public Task<MessageDialogResult> ConfirmDeleteAsync(object viewModel, string title, string message)
        {
            return _dialogCoordinator.ShowMessageAsync(viewModel, title, message, MessageDialogStyle.AffirmativeAndNegative);
        }

        public Task ShowMessageAsync(object viewModel, string title, string message)
        {
            return _dialogCoordinator.ShowMessageAsync(viewModel, title, message);
        }

        private class DeadfileProgressController : IDeadfileProgressController
        {
            private readonly ProgressDialogController _progress;

            public DeadfileProgressController(ProgressDialogController progress)
            {
                _progress = progress;
            }

            public Task CloseAsync()
            {
                return _progress.CloseAsync();
            }
        }

        public async Task<IDeadfileProgressController> ShowProgressAsync(object viewModel, string title, string message)
        {
            var progress = await _dialogCoordinator.ShowProgressAsync(viewModel, title, message);
            return new DeadfileProgressController(progress);
        }
    }
}
