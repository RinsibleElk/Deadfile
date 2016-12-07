using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Core;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Tab.Events;

namespace Deadfile.Tab.Common
{
    public interface IEditableItemViewModel<T> : IUndoTrackerActivatable
        where T : ModelBase
    {
        T SelectedItem { get; set; }
        bool Editable { get; }
        List<string> Errors { get; }
    }
}
