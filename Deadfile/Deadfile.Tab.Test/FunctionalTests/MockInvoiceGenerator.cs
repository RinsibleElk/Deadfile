using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Pdf;

namespace Deadfile.Tab.Test.FunctionalTests
{
    internal sealed class MockInvoiceGenerator : IInvoiceGenerator
    {
        public IDocumentPresenter GenerateDocument(InvoiceModel invoiceModel)
        {
            return null;
        }
    }
}
