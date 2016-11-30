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

        /// <summary>
        /// Report a changed property from a child.
        /// </summary>
        /// <param name="undoValue"></param>
        void ChildChanged(UndoValue undoValue);

        /// <summary>
        /// Relevant only on a parent undo tracker.
        /// </summary>
        void AddChild();

        /// <summary>
        /// Called by the child to delete itself.
        /// </summary>
        /// <param name="context"></param>
        void DeleteChild(int context);

        /// <summary>
        /// Disable tracking.
        /// </summary>
        void DisableTracking();

        /// <summary>
        /// Enable tracking.
        /// </summary>
        void EnableTracking();
    }
}