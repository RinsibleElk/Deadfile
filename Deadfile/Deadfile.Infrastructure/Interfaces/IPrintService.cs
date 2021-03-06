﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Deadfile.Pdf;

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
        void PrintDocument(IDocumentPresenter document);

        /// <summary>
        /// Print a visual.
        /// </summary>
        /// <param name="visual"></param>
        void PrintVisual(IVisualPresenter visual);
    }
}
