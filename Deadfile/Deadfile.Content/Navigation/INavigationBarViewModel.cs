using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Content.Navigation
{
    public interface INavigationBarViewModel
    {
        /// <summary>
        /// Navigate the ContentRegion back.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Navigate the ContentRegion home.
        /// </summary>
        ICommand HomeCommand { get; }

        /// <summary>
        /// Navigate the ContentRegion forward.
        /// </summary>
        ICommand ForwardCommand { get; }

        /// <summary>
        /// Undo any undoable actions in the ContentRegion.
        /// </summary>
        ICommand UndoCommand { get; }

        /// <summary>
        /// Redo any redoable actions in the ContentRegion.
        /// </summary>
        ICommand RedoCommand { get; }

    }
}
