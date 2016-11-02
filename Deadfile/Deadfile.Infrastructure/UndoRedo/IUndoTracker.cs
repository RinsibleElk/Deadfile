using System.ComponentModel;
using Deadfile.Model;

namespace Deadfile.Infrastructure.UndoRedo
{
    /// <summary>
    /// Provides the functionality of an already active undo/redo object.
    /// </summary>
    public interface IUndoTracker : INotifyPropertyChanged
    {
        /// <summary>
        /// Deactivate this <see cref="IUndoTracker"/>.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Whether there are changes to undo.
        /// </summary>
        bool CanUndo { get; set; }

        /// <summary>
        /// Whether there are changes to redo.
        /// </summary>
        bool CanRedo { get; set; }

        /// <summary>
        /// Undo a change.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo a change.
        /// </summary>
        void Redo();
    }
}