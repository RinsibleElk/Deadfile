using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Content.Interfaces
{
    public interface INavigationBarViewModel
    {
        /// <summary>
        /// Navigate the ContentRegion back.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Navigate the ContentRegion home.
        /// </summary>
        ICommand HomeCommand { get; }

        /// <summary>
        /// Navigate the ContentRegion forward.
        /// </summary>
        ICommand ForwardCommand { get; }
    }
}
