using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Content.Interfaces
{
    /// <summary>
    /// Every ContentRegion ViewModel must have these things.
    /// </summary>
    public interface IDeadfileViewModel
    {
        string Title { get; set; }
    }
}
