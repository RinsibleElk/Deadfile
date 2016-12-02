using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Core;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Prism.Mvvm;

namespace Deadfile.Infrastructure.UndoRedo
{
    /// <summary>
    /// Uses reflection madness to provide client-side Undo-Redo support for a model under user edits.
    /// </summary>
    /// <remarks>
    /// This ensures that PropertyChanging and PropertyChanged are always called in pairs, however, obviously it only pushes an
    /// Undo-Redo event onto the stack(s) for public settable properties.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class UndoTracker<T> : BindableBase, IUndoTracker where T : ModelBase
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _settableProperties;
        private readonly Dictionary<string, object> _changing = new Dictionary<string, object>();
        protected T Model = null;
        protected bool TrackingDisabled = false;
        private readonly Stack<UndoValue> _undoStack = new Stack<UndoValue>();
        private readonly Stack<UndoValue> _redoStack = new Stack<UndoValue>();

        public UndoTracker()
        {
            var modelType = typeof(T);
            var settableProperties = new Dictionary<string, PropertyInfo>();
            foreach (var propertyInfo in modelType.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
            {
                if ((propertyInfo.SetMethod != null) && (propertyInfo.GetMethod != null))
                    settableProperties.Add(propertyInfo.Name, propertyInfo);
            }
            _settableProperties = settableProperties;
        }

        public virtual void Activate(T model)
        {
            if (Model != null)
                throw new InvalidOperationException("Attempt to Activate an UndoTracker for type " + typeof(T) +
                                                    " (new value " + model + ") when it is already running for " +
                                                    Model);
            Model = model;
            Model.PropertyChanged += OnPropertyChanged;
            Model.PropertyChanging += OnPropertyChanging;
            CanUndo = false;
            CanRedo = false;
            EnableTracking();
            _undoStack.Clear();
            _redoStack.Clear();
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (TrackingDisabled) return;
            if (Model.DisableUndoTracking) return;
            if (!_settableProperties.ContainsKey(e.PropertyName)) return;
            if (_changing.ContainsKey(e.PropertyName))
                throw new ApplicationException("Called PropertyChanging twice in a row for " + typeof(T) + " property " + e.PropertyName);
            _changing.Add(e.PropertyName, _settableProperties[e.PropertyName].GetMethod.Invoke(Model, new object[0]));
        }

        protected virtual void UndoablePropertyChanged(PropertyInfo property, object previousValue, object newValue)
        {
            Change(new UndoValue() { Property = property, PreviousValue = previousValue, NewValue = newValue, Type = UndoType.Property, Context = null });
        }

        public void Change(UndoValue undoValue)
        {
            _undoStack.Push(undoValue);
            CanUndo = true;
            _redoStack.Clear();
            CanRedo = false;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (TrackingDisabled) return;
            if (Model.DisableUndoTracking) return;
            if (!_settableProperties.ContainsKey(e.PropertyName)) return;
            if (!_changing.ContainsKey(e.PropertyName)) return;
            var previousValue = _changing[e.PropertyName];
            _changing.Remove(e.PropertyName);
            var property = _settableProperties[e.PropertyName];
            var newValue = property.GetMethod.Invoke(Model, new object[0]);
            UndoablePropertyChanged(property, previousValue, newValue);
        }

        public virtual void Deactivate()
        {
            if (Model == null)
                throw new InvalidOperationException("Attempt to Deactivate an UndoTracker for type " + typeof(T) +
                                                    " when there is nothing to deactivate");
            Model.PropertyChanged -= OnPropertyChanged;
            Model.PropertyChanging -= OnPropertyChanging;
            Model = null;
            CanUndo = false;
            CanRedo = false;
            EnableTracking();
            _undoStack.Clear();
            _redoStack.Clear();
        }

        private bool _canUndo = false;
        public bool CanUndo
        {
            get { return _canUndo; }
            set { SetProperty(ref _canUndo, value); }
        }

        private bool _canRedo = false;
        public bool CanRedo
        {
            get { return _canRedo; }
            set { SetProperty(ref _canRedo, value); }
        }

        public void Undo()
        {
            if (!_canUndo) throw new InvalidOperationException("Attempt to Undo for type " + typeof(T) + " when " + nameof(CanUndo) + " is false");
            DisableTracking();
            if (_undoStack.Count == 0) throw new ApplicationException("Nothing to Undo for type " + typeof(T));
            var undoValue = _undoStack.Pop();
            _redoStack.Push(undoValue);
            PerformUndo(undoValue);
            CanUndo = _undoStack.Count > 0;
            CanRedo = true;
            EnableTracking();
        }

        protected internal virtual void PerformUndo(UndoValue undoValue)
        {
            undoValue.Property.SetMethod.Invoke(Model, new object[1] {undoValue.PreviousValue});
        }

        public void Redo()
        {
            if (!_canRedo) throw new InvalidOperationException("Attempt to Redo for type " + typeof(T) + " when CanRedo is false");
            DisableTracking();
            if (_redoStack.Count == 0) throw new ApplicationException("Nothing to Redo for type " + typeof(T));
            var redoValue = _redoStack.Pop();
            _undoStack.Push(redoValue);
            PerformRedo(redoValue);
            CanRedo = _redoStack.Count > 0;
            CanUndo = true;
            EnableTracking();
        }

        protected internal virtual void PerformRedo(UndoValue redoValue)
        {
            redoValue.Property.SetMethod.Invoke(Model, new object[1] {redoValue.NewValue});
        }

        public void ChildChanged(UndoValue undoValue)
        {
            Change(undoValue);
        }

        public virtual void AddChild()
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteChild(int context)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetChildren()
        {
            throw new NotImplementedException();
        }

        public virtual void DisableTracking()
        {
            TrackingDisabled = true;
        }

        public virtual void EnableTracking()
        {
            TrackingDisabled = false;
        }
    }
}
