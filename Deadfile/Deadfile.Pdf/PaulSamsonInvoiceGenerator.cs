using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Deadfile.Entity;
using Deadfile.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Deadfile.Pdf
{
    /// <summary>
    /// Invoice generator for <see cref="Company.PaulSamsonCharteredSurveyorLtd"/>.
    /// </summary>
    internal sealed class PaulSamsonInvoiceGenerator : IInvoiceGenerator
    {
        public void Generate(InvoiceModel invoiceModel, string outputFile)
        {
            using (var invoiceStream = GetPaulSamsonInvoiceStream())
            using (var inputDocument = PdfReader.Open(invoiceStream, PdfDocumentOpenMode.Import))
            {
                var outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title = inputDocument.Info.Title;
                outputDocument.Info.Creator = inputDocument.Info.Creator;
                outputDocument.AddPage(inputDocument.Pages[0]);

                var page = outputDocument.Pages[0];
                var gfx = XGraphics.FromPdfPage(page);
                var fontName = "Calibri";
                var addressFont = new XFont(fontName, 11, XFontStyle.Regular);
                double referenceX = 200;
                double addressX = 403;
                double rowSpacingY = 31;
                double firstRowY = 230;
                double secondRowY = firstRowY + rowSpacingY;
                double thirdRowY = secondRowY + rowSpacingY;
                double fourthRowY = thirdRowY + rowSpacingY;
                gfx.DrawString(string.Format("{0}", invoiceModel.InvoiceReference), addressFont, XBrushes.Black, referenceX, firstRowY);
                gfx.DrawString(string.Format("{0:dd/MM/yyyy}", invoiceModel.CreatedDate), addressFont, XBrushes.Black, referenceX, secondRowY);
                gfx.DrawString(invoiceModel.ClientName, addressFont, XBrushes.Black, addressX, firstRowY);
                var addressY = secondRowY;
                var addressSpacingY = 13;
                foreach (var addressLine in new string[] {invoiceModel.ClientAddressFirstLine,invoiceModel.ClientAddressSecondLine,invoiceModel.ClientAddressThirdLine,invoiceModel.ClientAddressPostCode}.Where((s) => !String.IsNullOrWhiteSpace(s)))
                {
                    gfx.DrawString(addressLine, addressFont, XBrushes.Black, addressX, addressY);
                    addressY += addressSpacingY;
                }
                var projectFont = new XFont(fontName, 11, XFontStyle.Bold);
                gfx.DrawString(invoiceModel.Project, projectFont, XBrushes.Black, referenceX, thirdRowY);
                gfx.DrawString(invoiceModel.Description, addressFont, XBrushes.Black, referenceX, fourthRowY);

                double itemSpacing = 22;
                double newlineSpacing = 12;
                double lineWidth = 80;
                double itemY = 364;
                double itemDescriptionX = 115;
                var itemFont = new XFont(fontName, 11, XFontStyle.Regular);
                foreach (var itemModel in invoiceModel.ChildrenList)
                {
                    var description = itemModel.Description;
                    while (description.Length > lineWidth)
                    {
                        var stuff = description.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var length = 0;
                        var i = 0;
                        while (length < lineWidth)
                            length += stuff[i++].Length;
                        var toWrite = string.Join(" ", stuff.Take(i - 1));
                        description = string.Join(" ", stuff.Skip(i - 1));
                        gfx.DrawString(toWrite, itemFont, XBrushes.Black, itemDescriptionX, itemY);
                        itemY += newlineSpacing;
                    }
                    gfx.DrawString(description, itemFont, XBrushes.Black, itemDescriptionX, itemY);
                    itemY += itemSpacing;
                }

                double amountsX = 408;
                double amountsY = 479;
                double amountsSpacingY = 31;
                var netAmountFont = new XFont(fontName, 11, XFontStyle.Regular);
                var grossAmountFont = new XFont(fontName, 14, XFontStyle.Bold);
                gfx.DrawString(invoiceModel.NetAmount.ToString("C", CultureInfo.CurrentCulture), netAmountFont, XBrushes.Black, amountsX, amountsY);
                amountsY += amountsSpacingY;
                gfx.DrawString(invoiceModel.VatValue.ToString("C", CultureInfo.CurrentCulture), netAmountFont, XBrushes.Black, amountsX, amountsY);
                amountsY += amountsSpacingY;
                gfx.DrawString(invoiceModel.GrossAmount.ToString("C", CultureInfo.CurrentCulture), grossAmountFont, XBrushes.Black, amountsX, amountsY);
                outputDocument.Save(outputFile);
            }
        }

        public FixedDocument GenerateDocument(InvoiceModel invoiceModel)
        {
            throw new NotImplementedException();
        }

        private static Stream GetPaulSamsonInvoiceStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.PKS.pdf";
            return assembly.GetManifestResourceStream(resourceName);
        }

    }
}
