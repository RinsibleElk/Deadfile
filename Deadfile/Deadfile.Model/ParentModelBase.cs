using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public abstract class ParentModelBase : StateManagedModelBase
    {
        private ObservableCollection<ChildModelBase> _children = new ObservableCollection<ChildModelBase>();
        [CustomValidation(typeof(ChildModelValidator), nameof(ChildModelValidator.ChildrenAreValid))]
        public ObservableCollection<ChildModelBase> Children
        {
            get { return _children; }
            protected set { SetProperty(ref _children, value); }
        }
    }

    public abstract class ParentModelBase<T> : ParentModelBase where T : ChildModelBase
    {
        public abstract List<T> ChildrenList { get; set; }

        public virtual void ChildrenUpdated()
        {
            Children = new ObservableCollection<ChildModelBase>(ChildrenList.Where((child) => !child.DeletePending));
        }
    }
}
