using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.JobChildren.Empty
{
    /// <summary>
    /// Allows the parent job to navigate away from the active job child and free resources when the job is navigated away from.
    /// </summary>
    class EmptyJobChildViewModel : Screen, ISimpleEditableItemViewModel
    {
        public bool Editable { get; } = false;
        public List<string> Errors { get; } = new List<string>();
        public string Filter { get; set; } = null;
        public ICommand EditCommand { get; } = null;
        public ICommand DiscardCommand { get; } = null;
        public ICommand SaveCommand { get; } = null;
        public IUndoTracker UndoTracker { get; } = null;
    }
}
