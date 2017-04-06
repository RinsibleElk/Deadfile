using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Abstraction of a DataGrid for testing.
    /// </summary>
    public interface IDataGridPresenter
    {
        DataGrid DataGrid { get; }
    }
}
