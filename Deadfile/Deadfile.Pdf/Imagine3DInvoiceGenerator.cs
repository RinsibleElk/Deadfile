using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Deadfile.Pdf
{
    /// <summary>
    /// Invoice generator for <see cref="Company.Imagine3DLtd"/>.
    /// </summary>
    internal sealed class Imagine3DInvoiceGenerator : IInvoiceGenerator
    {
        public void Generate(InvoiceModel invoiceModel, string outputFile)
        {
            using (var invoiceStream = GetImagine3DInvoiceStream())
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
                double addressX = 403;
                gfx.DrawString(string.Format("{0:0000}", invoiceModel.InvoiceReference), addressFont, XBrushes.Black, addressX, 230);
                gfx.DrawString(string.Format("{0:dd/MM/yyyy}", invoiceModel.CreatedDate), addressFont, XBrushes.Black, addressX, 244.5);
                gfx.DrawString(invoiceModel.ClientName, addressFont, XBrushes.Black, addressX, 275);
                gfx.DrawString(invoiceModel.ClientAddressFirstLine, addressFont, XBrushes.Black, addressX, 304);

                double projectX = 315;
                var projectFont = new XFont(fontName, 14, XFontStyle.Bold);
                gfx.DrawString(invoiceModel.Project, projectFont, XBrushes.Black, projectX, 384.5);
                var descriptionFont = new XFont(fontName, 14, XFontStyle.Regular);
                gfx.DrawString(invoiceModel.Description, descriptionFont, XBrushes.Black, projectX, 414);

                double itemSpacing = 22;
                double newlineSpacing = 13;
                double lineWidth = 63;
                double itemY = 490;
                double itemDescriptionX = 38;
                double itemNetAmountX = 408;
                var itemFont = new XFont(fontName, 11, XFontStyle.Regular);
                foreach (var itemModel in invoiceModel.ChildrenList)
                {
                    var description = itemModel.Description;
                    while (description.Length > lineWidth)
                    {
                        var stuff = description.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
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
                    gfx.DrawString(itemModel.NetAmount.ToString("C", CultureInfo.CurrentCulture), itemFont, XBrushes.Black, itemNetAmountX, itemY);
                    itemY += itemSpacing;
                }

                double grossAmountX = itemNetAmountX;
                double grossAmountY = 688;
                var grossAmountFont = new XFont(fontName, 14, XFontStyle.Bold);
                gfx.DrawString(invoiceModel.GrossAmount.ToString("C", CultureInfo.CurrentCulture), grossAmountFont, XBrushes.Black, grossAmountX, grossAmountY);
                outputDocument.Save(outputFile);
            }
        }

        private static Stream GetImagine3DInvoiceStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.i3D.pdf";
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
