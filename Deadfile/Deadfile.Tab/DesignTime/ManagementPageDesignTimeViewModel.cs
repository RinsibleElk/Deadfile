using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.DesignTime
{
    class ManagementPageDesignTimeViewModel<T> : PageDesignTimeViewModel, IManagementViewModel<T> where T : ModelBase
    {
        public ObservableCollection<T> Items { get; set; }
        public T SelectedItem { get; set; }
        public bool Editable { get; }
        public List<string> Errors { get; }
        public string Filter { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DiscardCommand { get; }
        public ICommand SaveCommand { get; }
    }
}
