using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Service for printing using a WPF PrintDialog.
    /// </summary>
    public interface IPrintService
    {
        /// <summary>
        /// Print a fixed document.
        /// </summary>
        /// <param name="document"></param>
        void PrintDocument(FixedDocument document);
    }
}
