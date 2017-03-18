using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockPrintService : IPrintService
    {
        public void PrintDocument(FixedDocument document)
        {
        }

        public void PrintVisual(Visual visual)
        {
        }
    }
}
