using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Deadfile.Model;
using PdfSharp.Pdf;

namespace Deadfile.Pdf
{
    /// <summary>
    /// Company specific invoice generator.
    /// </summary>
    public interface IInvoiceGenerator
    {
        /// <summary>
        /// Generate a fixed document for this invoice.
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <returns></returns>
        FixedDocument GenerateDocument(InvoiceModel invoiceModel);
    }
}
