using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Core;
using Deadfile.Model.Interfaces;
using Prism.Mvvm;

namespace Deadfile.Model.Browser
{
    public abstract class BrowserModel : BindableBase
    {
        private sealed class BrowserDummy : BrowserModel
        {
            protected override void LoadChildren()
            {
                throw new NotImplementedException();
            }

            public override BrowserModelType ModelType => BrowserModelType.Dummy;
        }

        private static readonly BrowserDummy DummyChild = new BrowserDummy();

        private ObservableCollection<BrowserModel> _children;
        public ObservableCollection<BrowserModel> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }

        protected BrowserModel()
        {
            _children = new ObservableCollection<BrowserModel>();
            Id = ModelBase.NewModelId;
            ParentId = ModelBase.NewModelId;
        }

        protected IDeadfileRepository Repository;
        protected BrowserMode Mode;
        protected bool IncludeInactiveEnabled;
        internal void SetRepository(BrowserMode mode, bool includeInactiveEnabled, bool isLeaf, IDeadfileRepository repository)
        {
            Mode = mode;
            IncludeInactiveEnabled = includeInactiveEnabled;
            Repository = repository;
            if (!isLeaf)
                _children.Add(DummyChild);
        }

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
        public bool HasDummyChild => Children.Count == 1 && Children[0] == DummyChild;

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected abstract void LoadChildren();
        public abstract BrowserModelType ModelType { get; }

        private int _parentId;
        public int ParentId
        {
            get { return _parentId; }
            set { SetProperty(ref _parentId, value); }
        }
    }
}
