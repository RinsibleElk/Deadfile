using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Abstraction of a visual for testing.
    /// </summary>
    public interface IVisualPresenter
    {
        Visual Visual { get; }
    }
}
