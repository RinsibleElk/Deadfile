using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Infrastructure.UndoRedo
{
    public sealed class ParentUndoTracker<T, K> : UndoTracker<T>
        where T : ParentModelBase<K>
        where K : ChildModelBase, new()
    {
        private readonly List<ChildUndoTracker<K>> _childTrackers = new List<ChildUndoTracker<K>>();

        public override void Activate(T model)
        {
            base.Activate(model);

            for (int i = 0; i < model.ChildrenList.Count; i++)
            {
                var child = model.ChildrenList[i];
                child.Context = i;
                var childTracker = new ChildUndoTracker<K>(this);
                _childTrackers.Add(childTracker);
                childTracker.Activate(child);
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            foreach (var childTracker in _childTrackers)
            {
                childTracker.Deactivate();
            }
            _childTrackers.Clear();
        }

        /// <summary>
        /// Using the <see cref="ParentUndoTracker{T,K}"/> to add a child means that the action will be undoable.
        /// </summary>
        public override void AddChild()
        {
            Change(new UndoValue()
            {
                Context = null,
                NewValue = null,
                PreviousValue = null,
                Property = null,
                Type = UndoType.Add
            });
            var newK = new K() {Context = Model.Children.Count, ParentId = Model.Id};
            Model.ChildrenList.Add(newK);
            Model.ChildrenUpdated();
            var newChildTracker = new ChildUndoTracker<K>(this);
            newChildTracker.Activate(newK);
            _childTrackers.Add(newChildTracker);
        }

        public override void DeleteChild(int context)
        {
            Model.ChildrenUpdated();
            Change(new UndoValue() { Property = null, PreviousValue = null, NewValue = null, Type = UndoType.Delete, Context = context });
        }

        protected internal override void PerformUndo(UndoValue undoValue)
        {
            if (undoValue.Type == UndoType.Add)
            {
                var context = _childTrackers.Count - 1;
                _childTrackers[context].Deactivate();
                _childTrackers.RemoveAt(context);
                Model.ChildrenList.RemoveAt(context);
                Model.ChildrenUpdated();
            }
            else if (undoValue.Type == UndoType.Delete)
            {
                var context = undoValue.Context.Value;
                Model.ChildrenList[context].DeletePending = false;
                Model.ChildrenUpdated();
            }
            else if (undoValue.Context != null)
            {
                _childTrackers[undoValue.Context.Value].PerformUndo(undoValue);
            }
            else
                base.PerformUndo(undoValue);
        }

        protected internal override void PerformRedo(UndoValue undoValue)
        {
            if (undoValue.Type == UndoType.Add)
            {
                var newK = new K() { Context = Model.ChildrenList.Count, ParentId = Model.Id };
                Model.ChildrenList.Add(newK);
                Model.ChildrenUpdated();
                var newChildTracker = new ChildUndoTracker<K>(this);
                newChildTracker.Activate(newK);
                _childTrackers.Add(newChildTracker);
            }
            else if (undoValue.Type == UndoType.Delete)
            {
                Model.ChildrenList[undoValue.Context.Value].DeletePending = true;
                Model.ChildrenUpdated();
            }
            else if (undoValue.Context != null)
            {
                _childTrackers[undoValue.Context.Value].PerformRedo(undoValue);
            }
            else
                base.PerformRedo(undoValue);
        }

        public override void DisableTracking()
        {
            base.DisableTracking();

            foreach (var childTracker in _childTrackers)
            {
                childTracker.DisableTracking();
            }
        }

        public override void EnableTracking()
        {
            base.EnableTracking();

            foreach (var childTracker in _childTrackers)
            {
                childTracker.EnableTracking();
            }
        }

    }
}
