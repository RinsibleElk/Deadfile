﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Represents a simple editable item that may have a strong relationship with a job or may be only loosely related to other entities.
    /// </summary>
    public interface ISimpleEditableItemViewModel<T> : IScreen, ISimpleEditableItemViewModel where T : ModelBase
    {
        /// <summary>
        /// The (possibly filtered) collection of items in the database.
        /// </summary>
        ObservableCollection<T> Items { get; set; }

        /// <summary>
        /// The selected item, or a new one, ready for editing.
        /// </summary>
        T SelectedItem { get; set; }

        /// <summary>
        /// The undo tracker for this editable item.
        /// </summary>
        new UndoTracker<T> UndoTracker { get; }
    }

    /// <summary>
    /// Represents a simple editable item that may have a strong relationship with a job or may be only loosely related to other entities.
    /// </summary>
    public interface ISimpleEditableItemViewModel
    {
        /// <summary>
        /// Whether this item is editable.
        /// </summary>
        bool Editable { get; }

        /// <summary>
        /// The current errors.
        /// </summary>
        List<string> Errors { get; }

        /// <summary>
        /// A user filter applied to the collection of items.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Edit the currently selected item.
        /// </summary>
        ICommand EditCommand { get; }

        /// <summary>
        /// Discard changes to the currently selected item and cease editing.
        /// </summary>
        ICommand DiscardCommand { get; }

        /// <summary>
        /// Save changes to the currently selected item and cease editing.
        /// </summary>
        ICommand SaveCommand { get; }

        /// <summary>
        /// The undo tracker for this editable item.
        /// </summary>
        IUndoTracker UndoTracker { get; }
    }
}