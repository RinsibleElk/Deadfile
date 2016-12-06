using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;
using PdfSharp.Pdf;

namespace Deadfile.Pdf
{
    /// <summary>
    /// Generate the correct invoice based on which company is given.
    /// </summary>
    public sealed class CompanySwitchingInvoiceGenerator : IInvoiceGenerator
    {
        /// <summary>
        /// Generate the invoice.
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <param name="outputFile"></param>
        public void Generate(InvoiceModel invoiceModel, string outputFile)
        {
            var invoiceGenerator = (invoiceModel.Company == Company.Imagine3DLtd)
                ? (IInvoiceGenerator) (new Imagine3DInvoiceGenerator())
                : (IInvoiceGenerator) (new PaulSamsonInvoiceGenerator());
            invoiceGenerator.Generate(invoiceModel, outputFile);
        }
    }
}
