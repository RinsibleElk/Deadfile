using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Deadfile.Entity;
using Deadfile.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Windows.Controls.Image;
using Size = System.Windows.Size;

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

        private static readonly Color PrimaryColor = Color.FromRgb(226, 107, 10);
        private static readonly Color SecondaryColor = Color.FromRgb(253, 233, 217);
        private static readonly FontFamily Calibri = new FontFamily("Calibri");

        private Stream GetImageStreamFromResource(string imageName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream("Deadfile.Pdf.Resources." + imageName);
        }

        public FixedDocument GenerateDocument(InvoiceModel invoiceModel)
        {
            // Create a FixedDocument.
            var doc = new FixedDocument();
            var pageWidth = 793.92;
            doc.DocumentPaginator.PageSize = new Size(pageWidth, 1122.24);

            // Single page doc.
            var page = new FixedPage();
            page.Width = doc.DocumentPaginator.PageSize.Width;
            page.Height = doc.DocumentPaginator.PageSize.Height;
            var sideMargin = 45.0;
            page.Margin = new Thickness(sideMargin, 100, sideMargin, 100);

            // At the top, there's a stack panel.
            var stackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

            // Start with image at the top left.
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = GetImagine3DLogoStream();
            bi.EndInit();
            var i = new Image();
            i.Source = bi;
            var imageWidth = 145.0;
            i.Width = imageWidth;
            i.Stretch = Stretch.Uniform;
            stackPanel.Children.Add(i);

            // Then there's a vertical stack panel.
            var header = new StackPanel() {Orientation = Orientation.Vertical};
            var title = new TextBlock()
            {
                Background = new SolidColorBrush(SecondaryColor),
                FontSize = 36,
                Text = "INVOICE",
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right,
                Padding = new Thickness(10),
                Width = pageWidth - sideMargin - sideMargin - imageWidth
            };
            var paddingAboveHeader = 45.0;
            header.Children.Add(new TextBlock() {Height = paddingAboveHeader});
            header.Children.Add(title);
            header.Children.Add(new TextBlock()
            {
                Foreground = new SolidColorBrush(PrimaryColor),
                Text = "Imagine3D Ltd",
                FontSize = 16
            });
            stackPanel.Children.Add(header);

            page.Children.Add(stackPanel);

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            doc.Pages.Add(pageContent);
            return doc;
        }

        private static Stream GetImagine3DLogoStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.i3dLogo.png";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private static Stream GetImagine3DInvoiceStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.i3D.pdf";
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
