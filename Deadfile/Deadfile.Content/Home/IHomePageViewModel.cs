using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;

namespace Deadfile.Content.Home
{
    /// <summary>
    /// The Home. This is the first experience.
    /// </summary>
    public interface IHomePageViewModel : IDeadfileViewModel
    {
        ICommand ClientsCommand { get; }

    }
}
