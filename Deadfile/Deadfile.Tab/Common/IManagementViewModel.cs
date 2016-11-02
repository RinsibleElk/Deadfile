using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    public interface IManagementViewModel<T> : IPageViewModel
        where T : ModelBase
    {
        ObservableCollection<T> Items { get; set; }
        T SelectedItem { get; set; }
        bool Editable { get; }
        List<string> Errors { get; }
        string Filter { get; set; }
        ICommand EditCommand { get; }
        ICommand DiscardCommand { get; }
        ICommand SaveCommand { get; }
    }
}
