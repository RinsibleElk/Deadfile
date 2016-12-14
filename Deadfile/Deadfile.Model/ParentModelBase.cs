using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public abstract class ParentModelBase<T> : StateManagedModelBase where T : ChildModelBase
    {
        public abstract List<T> ChildrenList { get; set; }
        private ObservableCollection<T> _children = new ObservableCollection<T>();
        public ObservableCollection<T> Children
        {
            get { return _children; }
            private set
            {
                // no validation needed here and explicit undo tracking undesirable
                if (object.Equals((object)_children, (object)value)) return;
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }

        public virtual void ChildrenUpdated()
        {
            Children = new ObservableCollection<T>(ChildrenList.Where((child) => !child.DeletePending));
        }
    }
}
