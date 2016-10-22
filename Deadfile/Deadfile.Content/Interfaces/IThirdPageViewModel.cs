using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Content.Interfaces
{
    public interface IThirdPageViewModel : IDeadfileViewModel
    {
        ICommand NavigateCommand { get; }
    }
}
