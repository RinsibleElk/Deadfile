using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Deadfile.Model.Billable
{
    public abstract class BillableModel : BindableBase
    {
        private ObservableCollection<BillableModel> _children;
        public ObservableCollection<BillableModel> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }

        protected BillableModel()
        {
            _children = new ObservableCollection<BillableModel>();
        }

        public abstract BillableModelType ModelType { get; }

        private BillableModelState _state;
        public BillableModelState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
    }
}
