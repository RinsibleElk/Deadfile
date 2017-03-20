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
        private static readonly Color PrimaryColor = Color.FromRgb(226, 107, 10);
        private static readonly Color SecondaryColor = Color.FromRgb(253, 233, 217);
        private static readonly Brush PrimaryColorBrush = new SolidColorBrush(PrimaryColor);
        private static readonly Brush SecondaryColorBrush = new SolidColorBrush(SecondaryColor);
        private static readonly FontFamily Calibri = new FontFamily("Calibri");
        private const double LogoImageWidth = 145.0;
        private const double PageWidth = 793.92;
        private const double PageHeight = 1122.24;
        private const double SideMargin = 30.0;
        private const double VerticalMargin = 30.0;
        private const double ProjectPaddingWidth = 5.0;

        private sealed class Imagine3DInvoiceDocumentPresenter : IDocumentPresenter
        {
            public Imagine3DInvoiceDocumentPresenter(FixedDocument document)
            {
                Document = document;
            }

            public FixedDocument Document { get; }
        }

        public IDocumentPresenter GenerateDocument(InvoiceModel invoiceModel)
        {
            // Create a FixedDocument.
            var doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(PageWidth, PageHeight);

            // Single page doc.
            var page = new FixedPage
            {
                Width = doc.DocumentPaginator.PageSize.Width,
                Height = doc.DocumentPaginator.PageSize.Height,
                Margin = new Thickness(SideMargin, VerticalMargin, SideMargin, VerticalMargin)
            };
            var pageStackPanel = new StackPanel();

            // At the top, there's a stack panel.
            var headerStackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

            // Start with image at the top left.
            var logoBitmap = new BitmapImage();
            logoBitmap.BeginInit();
            logoBitmap.StreamSource = GetImagine3DLogoStream();
            logoBitmap.EndInit();
            var logoImage = new Image
            {
                Source = logoBitmap,
                Width = LogoImageWidth,
                Stretch = Stretch.Uniform
            };
            headerStackPanel.Children.Add(logoImage);

            // Then there's a vertical stack panel.
            var header = new StackPanel() {Orientation = Orientation.Vertical};
            var title = new Imagine3DHeaderTextBlock("INVOICE");
            var paddingAboveHeader = 45.0;
            header.Children.Add(new Imagine3DTextBlock() {Height = paddingAboveHeader});
            header.Children.Add(title);
            // Use an image to show the handwriting text.
            var danielBitmap = new BitmapImage();
            danielBitmap.BeginInit();
            danielBitmap.StreamSource = GetImagine3DTextStream();
            danielBitmap.EndInit();
            var danielImageWidth = 145.0;
            var danielImage = new Image
            {
                Source = danielBitmap,
                Width = danielImageWidth,
                Stretch = Stretch.Uniform
            };
            var danielImageStackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            danielImageStackPanel.Children.Add(danielImage);
            header.Children.Add(danielImageStackPanel);

            headerStackPanel.Children.Add(header);

            var addressesSectionHeight = 210.0;
            var addressesStackPanel = new StackPanel
            {
                Height = addressesSectionHeight,
                Width = PageWidth - SideMargin - SideMargin,
                Orientation = Orientation.Horizontal
            };
            var addressesColumn1Width = 300.0;
            var glasgowStudStackPanel = new StackPanel
            {
                Width = addressesColumn1Width,
                Orientation = Orientation.Vertical
            };
            glasgowStudStackPanel.Children.Add(new Imagine3DAddressTextBlock("The Glasgow Stud,"));
            glasgowStudStackPanel.Children.Add(new Imagine3DAddressTextBlock("Burnt Farm Ride,"));
            glasgowStudStackPanel.Children.Add(new Imagine3DAddressTextBlock("Crews Hill,"));
            glasgowStudStackPanel.Children.Add(new Imagine3DAddressTextBlock("Enfield, EN2 9DY"));
            addressesStackPanel.Children.Add(glasgowStudStackPanel);

            var detailsPaddingHeight = 20.0;
            var detailsPaddingWidth = 5.0;
            var detailsTitlesWidth = 240.0;
            var detailsTitlesStackPanel = new StackPanel
            {
                Width = detailsTitlesWidth,
                Margin = new Thickness(0, 0, detailsPaddingWidth, 0)
            };
            detailsTitlesStackPanel.Children.Add(new Imagine3DDetailsTitlesTextBlock("Invoice Number:"));
            detailsTitlesStackPanel.Children.Add(new Imagine3DDetailsTitlesTextBlock("Date:"));
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock {Height = detailsPaddingHeight});
            detailsTitlesStackPanel.Children.Add(new Imagine3DDetailsTitlesTextBlock("To:"));
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock {Height = detailsPaddingHeight});
            detailsTitlesStackPanel.Children.Add(new Imagine3DDetailsTitlesTextBlock("Of:")
            {
                Margin = new Thickness(0, 1, 0, 0),
            });
            addressesStackPanel.Children.Add(detailsTitlesStackPanel);

            var detailsWidth = PageWidth - SideMargin - SideMargin - addressesColumn1Width - detailsTitlesWidth;
            var detailsStackPanel = new StackPanel
            {
                Width = detailsWidth
            };
            detailsStackPanel.Children.Add(new Imagine3DDetailsValuesTextBlock($"{invoiceModel.InvoiceReference:0000}"));
            detailsStackPanel.Children.Add(new Imagine3DDetailsValuesTextBlock($"{invoiceModel.CreatedDate:dd/MM/yyyy}"));
            detailsStackPanel.Children.Add(new Imagine3DTextBlock {Height = detailsPaddingHeight});
            detailsStackPanel.Children.Add(new Imagine3DDetailsValuesTextBlock($"{invoiceModel.ClientName}"));
            detailsStackPanel.Children.Add(new Imagine3DTextBlock {Height = detailsPaddingHeight});
            foreach (
                var addressLine in
                new string[]
                {
                    invoiceModel.ClientAddressFirstLine, invoiceModel.ClientAddressSecondLine,
                    invoiceModel.ClientAddressThirdLine, invoiceModel.ClientAddressPostCode
                }.Where(
                    (s) => !String.IsNullOrWhiteSpace(s)))
            {
                detailsStackPanel.Children.Add(new Imagine3DAddressLineTextBlock($"{addressLine}"));
            }
            addressesStackPanel.Children.Add(detailsStackPanel);

            var projectHeight = 85.0;
            var projectStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = PageWidth - SideMargin - SideMargin,
                Height = projectHeight
            };
            var projectTitlesWidth = (PageWidth - SideMargin - SideMargin)/2;
            var projectTitlesStackPanel = new StackPanel
            {
                Width = projectTitlesWidth
            };
            projectTitlesStackPanel.Children.Add(new Imagine3DProjectTitlesTextBlock("Project:"));
            projectTitlesStackPanel.Children.Add(new Imagine3DProjectTitlesTextBlock("Description:"));
            projectStackPanel.Children.Add(projectTitlesStackPanel);
            var projectDetailsStackPanel = new StackPanel
            {
                Width = PageWidth - SideMargin - SideMargin - projectTitlesWidth - ProjectPaddingWidth
            };
            projectDetailsStackPanel.Children.Add(new Imagine3DProjectDetailsTextBlock($"{invoiceModel.Project}"));
            projectDetailsStackPanel.Children.Add(new Imagine3DProjectDetailsTextBlock($"{invoiceModel.Description}"));
            projectStackPanel.Children.Add(projectDetailsStackPanel);

            var itemListWidth = PageWidth - SideMargin - SideMargin;
            var itemListTitlesStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = PrimaryColorBrush,
                Width = itemListWidth
            };
            var itemListTitlesWidth = PageWidth - SideMargin - SideMargin - detailsWidth;
            var descriptionLeftPadding = 5.0;
            itemListTitlesStackPanel.Children.Add(new Imagine3DItemListTitlesTextBlock("DESCRIPTION")
            { 
                Width = itemListTitlesWidth,
                Margin = new Thickness(descriptionLeftPadding, 2, 0, 2)
            });
            itemListTitlesStackPanel.Children.Add(new Imagine3DItemListTitlesTextBlock("TOTAL")
            {
                Margin = new Thickness(0, 2, 0, 2),
                Width = detailsWidth
            });

            var itemListHeight = 370.0;
            var itemListStackPanel = new StackPanel
            {
                Width = itemListWidth,
                Height = itemListHeight
            };
            var invoiceItemPadding = 20.0;
            foreach (var invoiceItemModel in invoiceModel.ChildrenList)
            {
                var itemListItemStackPanel = new StackPanel
                {
                    Width = itemListWidth,
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, invoiceItemPadding, 0, 0)
                };
                itemListItemStackPanel.Children.Add(new Imagine3DItemListItemsTextBlock(invoiceItemModel.Description)
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(descriptionLeftPadding, 0, 0, 0),
                    Width = itemListTitlesWidth
                });
                itemListItemStackPanel.Children.Add(new Imagine3DItemListItemsTextBlock(invoiceItemModel.NetAmount.ToString("C", CultureInfo.CurrentCulture))
                {
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Bottom
                });
                itemListStackPanel.Children.Add(itemListItemStackPanel);
            }

            var totalDueStackPanel = new StackPanel
            {
                Width = PageWidth - SideMargin - SideMargin,
                Orientation = Orientation.Horizontal
            };
            totalDueStackPanel.Children.Add(new Imagine3DTotalTextBlock("TOTAL DUE")
            {
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 2, 2, 2),
                Foreground = PrimaryColorBrush,
                Width = itemListTitlesWidth
            });
            var totalBorder = new Border
            {
                BorderThickness = new Thickness(0, 2, 0, 2),
                BorderBrush = PrimaryColorBrush,
                Background = SecondaryColorBrush,
                Width = itemListWidth - itemListTitlesWidth,
                Child = new Imagine3DTotalTextBlock(invoiceModel.GrossAmount.ToString("C", CultureInfo.CurrentCulture))
                {
                    Margin = new Thickness(2, 0, 0, 0)
                }
            };
            totalDueStackPanel.Children.Add(totalBorder);

            var footerStackPanel = new StackPanel
            {
                Margin = new Thickness(0, 30, 0, 0)
            };
            footerStackPanel.Children.Add(
                new Imagine3DTsAndCsTextBlock(
                    "Payment terms are 14 days from the date of invoice. Late payments will be charged at a rate of 2% per month."));
            footerStackPanel.Children.Add(
                new Imagine3DTsAndCsTextBlock(
                    "Payment can be made via cheque, made payable to Imagine3D Ltd. or via bank transfer to"));
            footerStackPanel.Children.Add(new Imagine3DTsAndCsTextBlock("Acccount Number: 92563142, Sort Code 40-01-06"));
            footerStackPanel.Children.Add(new Imagine3DTsAndCsTextBlock("Company Registration No: 7595218"));

            pageStackPanel.Children.Add(headerStackPanel);
            pageStackPanel.Children.Add(addressesStackPanel);
            pageStackPanel.Children.Add(projectStackPanel);
            pageStackPanel.Children.Add(itemListTitlesStackPanel);
            pageStackPanel.Children.Add(itemListStackPanel);
            pageStackPanel.Children.Add(totalDueStackPanel);
            pageStackPanel.Children.Add(footerStackPanel);
            page.Children.Add(pageStackPanel);
            var pageContent = new PageContent();
            ((IAddChild) pageContent).AddChild(page);
            doc.Pages.Add(pageContent);
            return new Imagine3DInvoiceDocumentPresenter(doc);
        }

        private static Stream GetImagine3DLogoStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.i3dLogo.png";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private static Stream GetImagine3DTextStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.Imagine3DText.png";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private class Imagine3DTextBlock : TextBlock
        {
            public Imagine3DTextBlock() : base()
            {
                FontFamily = Calibri;
            }
        }

        private class Imagine3DHeaderTextBlock : Imagine3DTextBlock
        {
            public Imagine3DHeaderTextBlock(string text) : base()
            {
                Background = new SolidColorBrush(SecondaryColor);
                FontSize = 38;
                Text = text;
                FontWeight = FontWeights.Bold;
                TextAlignment = TextAlignment.Right;
                Padding = new Thickness(10);
                Width = PageWidth - SideMargin - SideMargin - LogoImageWidth;
            }
        }

        private class Imagine3DAddressTextBlock : Imagine3DTextBlock
        {
            public Imagine3DAddressTextBlock(string text) : base()
            {
                Text = text;
                FontWeight = FontWeights.Bold;
                FontSize = 14;
            }
        }

        private class Imagine3DDetailsTitlesTextBlock : Imagine3DTextBlock
        {
            public Imagine3DDetailsTitlesTextBlock(string text) : base()
            {
                Text = text;
                Foreground = PrimaryColorBrush;
                FontWeight = FontWeights.Bold;
                FontSize = 13;
                HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        private class Imagine3DDetailsValuesTextBlock : Imagine3DTextBlock
        {
            public Imagine3DDetailsValuesTextBlock(string text) : base()
            {
                Text = text;
                FontSize = 13;
            }
        }

        private class Imagine3DAddressLineTextBlock : Imagine3DTextBlock
        {
            public Imagine3DAddressLineTextBlock(string text) : base()
            {
                Text = text;
                FontSize = 14;
            }
        }

        private class Imagine3DProjectTitlesTextBlock : Imagine3DTextBlock
        {
            public Imagine3DProjectTitlesTextBlock(string text) : base()
            {
                Text = text;
                FontSize = 17;
                FontWeight = FontWeights.Bold;
                Foreground = PrimaryColorBrush;
                HorizontalAlignment = HorizontalAlignment.Right;
                Margin = new Thickness(0, 1, ProjectPaddingWidth, 0);
            }
        }

        private class Imagine3DProjectDetailsTextBlock : Imagine3DTextBlock
        {
            public Imagine3DProjectDetailsTextBlock(string text) : base()
            {
                Text = text;
                FontWeight = FontWeights.Bold;
                Margin = new Thickness(0, 4, 0, 0);
                FontSize = 14;
            }
        }

        private class Imagine3DItemListTitlesTextBlock : Imagine3DTextBlock
        {
            public Imagine3DItemListTitlesTextBlock(string text) : base()
            {
                Text = text;
                Foreground = Brushes.White;
                FontSize = 17;
            }
        }

        private class Imagine3DItemListItemsTextBlock : Imagine3DTextBlock
        {
            public Imagine3DItemListItemsTextBlock(string text) : base()
            {
                Text = text;
                FontSize = 14;
            }
        }

        private class Imagine3DTsAndCsTextBlock : Imagine3DTextBlock
        {
            public Imagine3DTsAndCsTextBlock(string text) : base()
            {
                Text = text;
                TextAlignment = TextAlignment.Center;
                FontSize = 12;
            }
        }

        private class Imagine3DTotalTextBlock : Imagine3DTextBlock
        {
            public Imagine3DTotalTextBlock(string text) : base()
            {
                Text = text;
                FontWeight = FontWeights.Bold;
                FontSize = 19;
            }
        }
    }
}
