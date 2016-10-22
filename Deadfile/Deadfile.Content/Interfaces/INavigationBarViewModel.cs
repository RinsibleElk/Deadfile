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
        ICommand BackCommand { get; }
        ICommand HomeCommand { get; }
        ICommand ForwardCommand { get; }
    }
}
