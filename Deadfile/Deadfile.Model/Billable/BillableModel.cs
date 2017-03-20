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
            set
            {
                if (SetProperty(ref _state, value))
                {
                    Parent?.StateChanged(Index);
                }
            }
        }

        /// <summary>
        /// Index into this billable model from parent.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Parent to call back to set changes.
        /// </summary>
        public IBillableModelContainer Parent { get; set; }

        private double _netAmount;
        public double NetAmount
        {
            get { return _netAmount; }
            set
            {
                if (SetProperty(ref _netAmount, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        private double _hours;
        public double Hours
        {
            get { return _hours; }
            set
            {
                if (SetProperty(ref _hours, value))
                    OnPropertyChanged(nameof(Text));
            }
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

        public abstract int Id { get; set; }

        private int? _invoiceId;
        public int? InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }
    }
}
