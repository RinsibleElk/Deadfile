using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dragablz;

namespace Deadfile
{
    class ShellDesignTimeViewModel : IShellViewModel
    {
        public ItemActionCallback ClosingItemActionCallback { get; } = null;
        public IInterTabClient InterTabClient { get; } = null;
        public ICommand OpenNewTab { get; } = null;
        public ICommand OpenNewTabToBrowserModelCommand { get; } = null;
        public ICommand OpenNewTabToNewClientCommand { get; } = null;
    }
}
