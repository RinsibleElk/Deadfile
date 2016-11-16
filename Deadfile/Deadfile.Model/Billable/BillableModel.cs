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

        private double _netAmount;
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        public abstract string Text { get; }

        private bool _isExpanded = false;
        /// <summary>
        /// Gets/sets whether the TreeViewItem associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                SetProperty(ref _isExpanded, value);
            }
        }
    }
}
