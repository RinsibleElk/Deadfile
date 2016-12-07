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
        /// Generate the PDF file to the specified output file.
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <param name="outputFile"></param>
        void Generate(InvoiceModel invoiceModel, string outputFile);

        /// <summary>
        /// Generate a fixed document for this invoice.
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <returns></returns>
        FixedDocument GenerateDocument(InvoiceModel invoiceModel);
    }
}
