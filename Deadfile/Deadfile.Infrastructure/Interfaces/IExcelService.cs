using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Service for exporting a DataGrid into Excel.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Export a DataGrid.
        /// </summary>
        /// <param name="dataGrid"></param>
        void Export(IDataGridPresenter dataGrid);
    }
}
