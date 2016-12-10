using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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
        /// Generate the document.
        /// </summary>
        /// <param name="invoiceModel"></param>
        /// <returns></returns>
        public FixedDocument GenerateDocument(InvoiceModel invoiceModel)
        {
            var invoiceGenerator = (invoiceModel.Company == Company.Imagine3DLtd)
                ? (IInvoiceGenerator)(new Imagine3DInvoiceGenerator())
                : (IInvoiceGenerator)(new PaulSamsonInvoiceGenerator());
            return invoiceGenerator.GenerateDocument(invoiceModel);
        }
    }
}
