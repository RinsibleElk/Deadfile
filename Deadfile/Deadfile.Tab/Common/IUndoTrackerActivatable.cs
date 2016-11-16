using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    public interface IUndoTrackerActivatable
    {
        /// <summary>
        /// Can be called by a derived class who manages sub-objects that need to manage the current active undo tracker, responding to Undo
        /// events from the navigation bar.
        /// </summary>
        /// <remarks>
        /// The motivation is for the Jobs page, where there are related editable items that might be managed on the same page.
        /// </remarks>
        /// <typeparam name="TObjectUnderEdit"></typeparam>
        /// <param name="newActiveUndoTracker"></param>
        /// <param name="objectUnderEdit"></param>
        void ActivateUndoTracker<TObjectUnderEdit>(UndoTracker<TObjectUnderEdit> newActiveUndoTracker, TObjectUnderEdit objectUnderEdit) where TObjectUnderEdit : ModelBase;

        /// <summary>
        /// Can be called by a derived class who manages sub-objects that need to manage the current active undo tracker, responding to Undo
        /// events from the navigation bar.
        /// </summary>
        /// <remarks>
        /// The motivation is for the Jobs page, where there are related editable items that might be managed on the same page.
        /// </remarks>
        void DeactivateUndoTracker();

    }
}
