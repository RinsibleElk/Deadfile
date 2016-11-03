using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Prism.Commands;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// These are parameterised by the job id of the parent job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class SimpleEditableItemViewModel<T> : ParameterisedViewModel<int>, ISimpleEditableItemViewModel<T> where T : ModelBase, new()
    {
        private readonly DelegateCommand _editCommand;
        private readonly DelegateCommand _discardCommand;
        private readonly DelegateCommand _saveCommand;
        private List<string> _errors;

        public SimpleEditableItemViewModel()
        {
            _editCommand = new DelegateCommand(StartEditing);
            _discardCommand = new DelegateCommand(DiscardEdits);
            _saveCommand = new DelegateCommand(PerformSaveAction);
        }

        //TODO - need to communicate via the job model really

        private void DiscardEdits()
        {
            while (UndoTracker.CanUndo)
                UndoTracker.Undo();
            Editable = false;
        }

        private void StartEditing()
        {
            Editable = true;
        }

        private void PerformSaveAction()
        {
            PerformSave();
            Editable = false;
        }

        protected abstract void PerformSave();

        private bool _editable;
        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (value == _editable) return;
                _editable = value;
                NotifyOfPropertyChange(() => Editable);
            }
        }

        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                if (Equals(value, _errors)) return;
                _errors = value;
                NotifyOfPropertyChange(() => Errors);
            }
        }

        private void SelectedItemErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            Errors = FlattenErrors();
        }

        private List<string> FlattenErrors()
        {
            List<string> errors = new List<string>();
            Dictionary<string, List<string>> allErrors = SelectedItem.GetAllErrors();
            foreach (string propertyName in allErrors.Keys)
            {
                foreach (var errorString in allErrors[propertyName])
                {
                    errors.Add(propertyName + ": " + errorString);
                }
            }
            return errors;
        }

        private string _filter = null;
        /// <summary>
        /// The user's filter for the table of items.
        /// </summary>
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) return;
                _filter = value;
                NotifyOfPropertyChange(() => Filter);

                // Populate the table.
                // We always add one more, to represent the user wanting to add a new one.
                SelectedItem = new T();
                Items = new ObservableCollection<T>(GetModelsForJobId(_jobId, _filter));
                Items.Add(SelectedItem);
            }
        }

        public ICommand EditCommand { get { return _editCommand; } }
        public ICommand DiscardCommand { get { return _discardCommand; } }
        public ICommand SaveCommand { get { return _saveCommand; } }

        /// <summary>
        /// The type-specific undo tracker.
        /// </summary>
        public UndoTracker<T> UndoTracker { get; } = new UndoTracker<T>();

        /// <summary>
        /// The table of items.
        /// </summary>
        public ObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        private T _selectedItem;
        private int _jobId = ModelBase.NewModelId;
        private ObservableCollection<T> _items;

        /// <summary>
        /// The item selected by the user.
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Provides access to the parent job to perform undos.
        /// </summary>
        IUndoTracker ISimpleEditableItemViewModel.UndoTracker
        {
            get { return UndoTracker; }
        }

        /// <summary>
        /// The user is navigating away.
        /// </summary>
        /// <param name="jobId"></param>
        public override void OnNavigatedTo(int jobId)
        {
            base.OnNavigatedTo(jobId);

            // Hold on to the parent job.
            _jobId = jobId;

            // Populate the table.
            // We always add one more, to represent the user wanting to add a new one.
            SelectedItem = new T();
            Items = new ObservableCollection<T>(GetModelsForJobId(jobId, null));
            Items.Add(SelectedItem);

            // Cheese the filter. We don't want to load the models twice, that's wasteful.
            _filter = null;
            NotifyOfPropertyChange(nameof(Filter));
        }

        /// <summary>
        /// Unsubscribe from events.
        /// </summary>
        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();

            _jobId = ModelBase.NewModelId;

            // Bin the table.
            SelectedItem = new T();
            Items = new ObservableCollection<T>();
            Items.Add(SelectedItem);

            // Same again.
            _filter = null;
            NotifyOfPropertyChange(nameof(Filter));
        }

        /// <summary>
        /// Ask the sub class to get all the models for this job, given a user-entered filter (which may be null or empty).
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetModelsForJobId(int jobId, string filter);
    }
}
