using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

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

        /// <summary>
        /// Print a visual.
        /// </summary>
        /// <param name="visual"></param>
        void PrintVisual(Visual visual);
    }
}
