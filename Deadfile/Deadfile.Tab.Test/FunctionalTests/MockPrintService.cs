using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Pdf;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockPrintService : IPrintService
    {
        public void PrintDocument(IDocumentPresenter document)
        {
        }

        public void PrintVisual(IVisualPresenter visual)
        {
        }
    }
}
