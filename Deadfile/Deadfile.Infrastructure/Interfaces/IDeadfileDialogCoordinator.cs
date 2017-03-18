using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace Deadfile.Infrastructure.Interfaces
{
    public interface IDeadfileDialogCoordinator
    {
        Task<MessageDialogResult> ConfirmDeleteAsync(object viewModel, string title, string message);
        Task ShowMessageAsync(object viewModel, string title, string message);
        Task<IDeadfileProgressController> ShowProgressAsync(object viewModel, string title, string message);
    }
}
