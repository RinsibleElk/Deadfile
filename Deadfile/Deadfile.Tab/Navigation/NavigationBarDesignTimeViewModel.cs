using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab.Navigation
{
    class NavigationBarDesignTimeViewModel : INavigationBarViewModel
    {
        public void Home()
        {
            throw new NotImplementedException();
        }

        public bool CanHome { get; } = true;
        public void Back()
        {
            throw new NotImplementedException();
        }

        public bool CanBack { get; } = true;
        public void Forward()
        {
            throw new NotImplementedException();
        }

        public bool CanForward { get; } = false;
        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool CanUndo { get; } = false;
        public void Redo()
        {
            throw new NotImplementedException();
        }

        public bool CanRedo { get; } = false;
    }
}
