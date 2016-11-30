using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Infrastructure.UndoRedo
{
    public sealed class ChildUndoTracker<T> : UndoTracker<T>
        where T : ChildModelBase
    {
        private readonly IUndoTracker _parentUndoTracker;

        public ChildUndoTracker(IUndoTracker parentUndoTracker)
        {
            _parentUndoTracker = parentUndoTracker;
        }

        protected override void UndoablePropertyChanged(PropertyInfo property, object previousValue, object newValue)
        {
            // Only ever set this from false to true.
            if (property.Name == nameof(ChildModelBase.DeletePending))
            {
                _parentUndoTracker.DeleteChild(Model.Context);
            }
            else
                _parentUndoTracker.ChildChanged(new UndoValue() { Property = property, PreviousValue = previousValue, NewValue = newValue, Type = UndoType.Property, Context = Model.Context });
        }

    }
}
