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
                gfx.DrawString($"{invoiceModel.InvoiceReference:0000}", addressFont, XBrushes.Black, addressX, 230);
                gfx.DrawString($"{invoiceModel.CreatedDate:dd/MM/yyyy}", addressFont, XBrushes.Black, addressX, 244.5);
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
        private static readonly Brush PrimaryColorBrush = new SolidColorBrush(PrimaryColor);
        private static readonly Brush SecondaryColorBrush = new SolidColorBrush(SecondaryColor);
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
            var pageStackPanel = new StackPanel();

            // At the top, there's a stack panel.
            var headerStackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

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
            headerStackPanel.Children.Add(i);

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
            headerStackPanel.Children.Add(header);

            var addressesSectionHeight = 200.0;
            var addressesStackPanel = new StackPanel
            {
                Height = addressesSectionHeight,
                Width = pageWidth - sideMargin - sideMargin,
                Orientation = Orientation.Horizontal
            };
            var addressesColumn1Width = 300.0;
            var glasgowStudStackPanel = new StackPanel
            {
                Width = addressesColumn1Width,
                Orientation = Orientation.Vertical
            };
            glasgowStudStackPanel.Children.Add(new TextBlock
            {
                Text = "The Glasgow Stud,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new TextBlock
            {
                Text = "Burnt Farm Ride,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new TextBlock
            {
                Text = "Crews Hill,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new TextBlock
            {
                Text = "Enfield, EN2 9DY",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            addressesStackPanel.Children.Add(glasgowStudStackPanel);

            var detailsPaddingHeight = 20.0;
            var detailsPaddingWidth = 5.0;
            var detailsTitlesWidth = 240.0;
            var detailsTitlesStackPanel = new StackPanel
            {
                Width = detailsTitlesWidth,
                Margin = new Thickness(0, 0, detailsPaddingWidth, 0)
            };
            detailsTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "Invoice Number:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "Date:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new TextBlock { Height = detailsPaddingHeight });
            detailsTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "To:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new TextBlock { Height = detailsPaddingHeight });
            detailsTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "Of:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                Margin = new Thickness(0, 1, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            });
            addressesStackPanel.Children.Add(detailsTitlesStackPanel);

            var detailsWidth = pageWidth - sideMargin - sideMargin - addressesColumn1Width - detailsTitlesWidth;
            var detailsStackPanel = new StackPanel
            {
                Width = detailsWidth
            };
            detailsStackPanel.Children.Add(new TextBlock
            {
                Text = $"{invoiceModel.InvoiceReference:0000}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new TextBlock
            {
                Text = $"{invoiceModel.CreatedDate:dd/MM/yyyy}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new TextBlock { Height = detailsPaddingHeight });
            detailsStackPanel.Children.Add(new TextBlock
            {
                Text = $"{invoiceModel.ClientName}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new TextBlock { Height = detailsPaddingHeight });
            foreach (
                var addressLine in
                new string[]
                {
                    invoiceModel.ClientAddressFirstLine, invoiceModel.ClientAddressSecondLine,
                    invoiceModel.ClientAddressThirdLine, invoiceModel.ClientAddressPostCode
                }.Where(
                    (s) => !String.IsNullOrWhiteSpace(s)))
            {
                detailsStackPanel.Children.Add(new TextBlock
                {
                    Text = $"{addressLine}",
                    FontSize = 12
                });
            }
            addressesStackPanel.Children.Add(detailsStackPanel);

            var projectHeight = 75.0;
            var projectStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMargin - sideMargin,
                Height = projectHeight
            };
            var projectTitlesWidth = (pageWidth - sideMargin - sideMargin)/2;
            var projectTitlesStackPanel = new StackPanel
            {
                Width = projectTitlesWidth
            };
            var projectPaddingWidth = 5.0;
            projectTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "Project:",
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = PrimaryColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, projectPaddingWidth, 0)
            });
            projectTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "Description:",
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = PrimaryColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, projectPaddingWidth, 0)
            });
            projectStackPanel.Children.Add(projectTitlesStackPanel);
            var projectDetailsStackPanel = new StackPanel
            {
                Width = pageWidth - sideMargin - sideMargin - projectTitlesWidth - projectPaddingWidth
            };
            projectDetailsStackPanel.Children.Add(new TextBlock
            {
                Text = $"{invoiceModel.Project}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 4, 0, 0),
                FontSize = 12
            });
            projectDetailsStackPanel.Children.Add(new TextBlock
            {
                Text = $"{invoiceModel.Description}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 4, 0, 0),
                FontSize = 12
            });
            projectStackPanel.Children.Add(projectDetailsStackPanel);

            var itemListWidth = pageWidth - sideMargin - sideMargin;
            var itemListTitlesStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = PrimaryColorBrush,
                Width = itemListWidth
            };
            var itemListTitlesWidth = pageWidth - sideMargin - sideMargin - detailsWidth;
            var descriptionLeftPadding = 5.0;
            itemListTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "DESCRIPTION",
                Foreground = Brushes.White,
                FontSize = 15,
                Width = itemListTitlesWidth,
                Margin = new Thickness(descriptionLeftPadding, 0, 0, 0)
            });
            itemListTitlesStackPanel.Children.Add(new TextBlock
            {
                Text = "TOTAL",
                Foreground = Brushes.White,
                FontSize = 15,
                Width = detailsWidth
            });

            var itemListHeight = 250.0;
            var itemListStackPanel = new StackPanel
            {
                Width = itemListWidth,
                Height = itemListHeight
            };
            var invoiceItemPadding = 10.0;
            foreach (var invoiceItemModel in invoiceModel.ChildrenList)
            {
                var itemListItemStackPanel = new StackPanel
                {
                    Width = itemListWidth,
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, invoiceItemPadding, 0, invoiceItemPadding)
                };
                itemListItemStackPanel.Children.Add(new TextBlock
                {
                    Text = invoiceItemModel.Description,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 11,
                    Margin = new Thickness(descriptionLeftPadding, 0, 0, 0),
                    Width = itemListTitlesWidth
                });
                itemListItemStackPanel.Children.Add(new TextBlock
                {
                    Text = invoiceItemModel.NetAmount.ToString("C", CultureInfo.CurrentCulture),
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Bottom
                });
                itemListStackPanel.Children.Add(itemListItemStackPanel);
            }

            pageStackPanel.Children.Add(headerStackPanel);
            pageStackPanel.Children.Add(addressesStackPanel);
            pageStackPanel.Children.Add(projectStackPanel);
            pageStackPanel.Children.Add(itemListTitlesStackPanel);
            pageStackPanel.Children.Add(itemListStackPanel);
            page.Children.Add(pageStackPanel);
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
