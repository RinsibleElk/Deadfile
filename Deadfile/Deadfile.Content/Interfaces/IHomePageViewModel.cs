using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Content.Interfaces
{
    /// <summary>
    /// The HomePage. This is the first experience.
    /// </summary>
    public interface IHomePageViewModel : IDeadfileViewModel
    {
        ICommand NavigateCommand { get; }
        ICommand ClientsCommand { get; }
    }
}
