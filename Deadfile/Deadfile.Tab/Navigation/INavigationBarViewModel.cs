using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Tab.Events;

namespace Deadfile.Tab.Navigation
{
    interface INavigationBarViewModel
    {
        void Home();
        bool CanHome { get; }
        void Back();
        bool CanBack { get; }
        void Forward();
        bool CanForward { get; }
        void Undo();
        bool CanUndo { get; }
        void Redo();
        bool CanRedo { get; }
    }
}
