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
    public sealed class UndoTracker<T> : BindableBase
        where T : ModelBase
    {
        private class UndoValue
        {
            public PropertyInfo Property { get; set; }
            public object PreviousValue { get; set; }
            public object NewValue { get; set; }
        }

        private readonly IReadOnlyDictionary<string, PropertyInfo> _settableProperties;
        private readonly Dictionary<string, object> _changing = new Dictionary<string, object>();
        private T _model = null;
        private bool _disableTracking = false;
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

        public void Activate(T model)
        {
            if (_model != null)
                throw new InvalidOperationException("Attempt to Activate an UndoTracker for type " + typeof(T) +
                                                    " (new value " + model + ") when it is already running for " +
                                                    _model);
            _model = model;
            _model.PropertyChanged += OnPropertyChanged;
            _model.PropertyChanging += OnPropertyChanging;
            CanUndo = false;
            CanRedo = false;
            _disableTracking = false;
            _undoStack.Clear();
            _redoStack.Clear();
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (_disableTracking) return;
            if (!_settableProperties.ContainsKey(e.PropertyName)) return;
            if (_changing.ContainsKey(e.PropertyName))
                throw new ApplicationException("Called PropertyChanging twice in a row for " + typeof(T) + " property " + e.PropertyName);
            _changing.Add(e.PropertyName, _settableProperties[e.PropertyName].GetMethod.Invoke(_model, new object[0]));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disableTracking) return;
            if (!_settableProperties.ContainsKey(e.PropertyName)) return;
            if (!_changing.ContainsKey(e.PropertyName))
                throw new ApplicationException("Called PropertyChanged without calling PropertyChanging for " + typeof(T) + " property " + e.PropertyName);
            var previousValue = _changing[e.PropertyName];
            _changing.Remove(e.PropertyName);
            var property = _settableProperties[e.PropertyName];
            var newValue = property.GetMethod.Invoke(_model, new object[0]);
            _undoStack.Push(new UndoValue() { Property = property, PreviousValue = previousValue, NewValue = newValue });
            CanUndo = true;
            _redoStack.Clear();
            CanRedo = false;
        }

        public void Deactivate()
        {
            if (_model == null)
                throw new InvalidOperationException("Attempt to Deactivate an UndoTracker for type " + typeof(T) +
                                                    " when there is nothing to deactivate");
            _model.PropertyChanged -= OnPropertyChanged;
            _model.PropertyChanging -= OnPropertyChanging;
            _model = null;
            CanUndo = false;
            CanRedo = false;
            _disableTracking = false;
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
            if (!_canUndo) throw new InvalidOperationException("Attempt to Undo for type " + typeof(T) + " when CanUndo is false");
            _disableTracking = true;
            if (_undoStack.Count == 0) throw new ApplicationException("Nothing to Undo for type " + typeof(T));
            var undoValue = _undoStack.Pop();
            _redoStack.Push(undoValue);
            undoValue.Property.SetMethod.Invoke(_model, new object[1] { undoValue.PreviousValue });
            CanUndo = _undoStack.Count > 0;
            CanRedo = true;
            _disableTracking = false;
        }

        public void Redo()
        {
            if (!_canRedo) throw new InvalidOperationException("Attempt to Redo for type " + typeof(T) + " when CanRedo is false");
            _disableTracking = true;
            if (_redoStack.Count == 0) throw new ApplicationException("Nothing to Redo for type " + typeof(T));
            var redoValue = _redoStack.Pop();
            _undoStack.Push(redoValue);
            redoValue.Property.SetMethod.Invoke(_model, new object[1] { redoValue.NewValue });
            CanRedo = _redoStack.Count > 0;
            CanUndo = true;
            _disableTracking = false;
        }
    }
}
