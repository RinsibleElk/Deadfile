using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Core;
using Prism.Mvvm;

namespace Deadfile.Model.Browser
{
    public abstract class BrowserModel : BindableBase
    {
        private sealed class BrowserDummy : BrowserModel
        {
            public BrowserDummy() : base(true)
            {
            }

            protected override void LoadChildren()
            {
                throw new NotImplementedException();
            }

            public override BrowserModelType ModelType
            {
                get { return BrowserModelType.Dummy; }
            }
        }

        private static readonly BrowserDummy DummyChild = new BrowserDummy();

        private ObservableCollection<BrowserModel> _children;
        public ObservableCollection<BrowserModel> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }

        protected BrowserModel(bool isLeaf)
        {
            _children = new ObservableCollection<BrowserModel>();
            if (!isLeaf)
                _children.Add(DummyChild);
        }

        private bool _isExpanded = false;
        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                SetProperty(ref _isExpanded, value);

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected abstract void LoadChildren();
        public abstract BrowserModelType ModelType { get; }
    }
}
