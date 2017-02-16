using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public abstract class ParentModelBase<T> : StateManagedModelBase where T : ChildModelBase
    {
        public abstract List<T> ChildrenList { get; set; }
        private ObservableCollection<ChildModelBase> _children = new ObservableCollection<ChildModelBase>();
        [CustomValidation(typeof(ChildModelValidator), nameof(ChildModelValidator.ChildrenAreValid))]
        public ObservableCollection<ChildModelBase> Children
        {
            get { return _children; }
            private set { SetProperty(ref _children, value); }
        }

        public virtual void ChildrenUpdated()
        {
            Children = new ObservableCollection<ChildModelBase>(ChildrenList.Where((child) => !child.DeletePending));
        }
    }
}
