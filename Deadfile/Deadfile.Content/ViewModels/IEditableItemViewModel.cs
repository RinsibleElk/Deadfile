using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;

namespace Deadfile.Content.ViewModels
{
    public interface IEditableItemViewModel<T>
        where T : ValidatableBindableBase, INotifyDataErrorInfo, INotifyPropertyChanged, INotifyPropertyChanging
    {
        T SelectedItem { get; set; }
        bool Editable { get; }
        List<string> Errors { get; }
    }
}
