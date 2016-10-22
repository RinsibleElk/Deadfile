using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;

namespace Deadfile.Content.DesignTime
{
    class NavigationBarDesignTimeViewModel : INavigationBarViewModel
    {
        public ICommand BackCommand { get; } = null;
        public ICommand HomeCommand { get; } = null;
        public ICommand ForwardCommand { get; } = null;
    }
}
