﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Pdf;

namespace Deadfile.Infrastructure.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="IPrintService"/> that uses the WPF's <see cref="PrintDialog"/> to print a <see cref="FixedDocument"/>.
    /// </summary>
    public class PrintService : IPrintService
    {
        public void PrintDocument(IDocumentPresenter document)
        {
            var printDlg = new PrintDialog();
            var result = printDlg.ShowDialog();
            if (result.Value)
                printDlg.PrintDocument(((IDocumentPaginatorSource)document.Document).DocumentPaginator, "Invoice");
        }

        public void PrintVisual(IVisualPresenter visual)
        {
            var printDlg = new PrintDialog();
            var result = printDlg.ShowDialog();
            if (result.Value)
                printDlg.PrintVisual(visual.Visual, "Report");
        }
    }
}
