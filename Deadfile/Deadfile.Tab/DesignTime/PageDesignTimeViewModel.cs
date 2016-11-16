using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.UndoRedo;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.DesignTime
{
    class PageDesignTimeViewModel : DesignTimeViewModel, IPageViewModel, INavigationAware, IUndoTrackerActivatable
    {
        public void OnNavigatedTo(object parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public virtual Experience Experience { get; }
        public bool ShowActionsPad { get; } = false;
        public void CompleteNavigation()
        {
            throw new NotImplementedException();
        }

        public void ActivateUndoTracker<TObjectUnderEdit>(UndoTracker<TObjectUnderEdit> newActiveUndoTracker, TObjectUnderEdit objectUnderEdit) where TObjectUnderEdit : ModelBase
        {
            throw new NotImplementedException();
        }

        public void DeactivateUndoTracker()
        {
            throw new NotImplementedException();
        }
    }
}
