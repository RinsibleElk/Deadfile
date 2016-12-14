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
        private const double SideMargin = 45.0;
        private const double VerticalMargin = 100.0;

        public FixedDocument GenerateDocument(InvoiceModel invoiceModel)
        {
            // Create a FixedDocument.
            var doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(PageWidth, PageHeight);

            // Single page doc.
            var page = new FixedPage();
            page.Width = doc.DocumentPaginator.PageSize.Width;
            page.Height = doc.DocumentPaginator.PageSize.Height;
            page.Margin = new Thickness(SideMargin, VerticalMargin, SideMargin, VerticalMargin);
            var pageStackPanel = new StackPanel();

            // At the top, there's a stack panel.
            var headerStackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

            // Start with image at the top left.
            var logoBitmap = new BitmapImage();
            logoBitmap.BeginInit();
            logoBitmap.StreamSource = GetImagine3DLogoStream();
            logoBitmap.EndInit();
            var logoImage = new Image();
            logoImage.Source = logoBitmap;
            logoImage.Width = LogoImageWidth;
            logoImage.Stretch = Stretch.Uniform;
            headerStackPanel.Children.Add(logoImage);

            // Then there's a vertical stack panel.
            var header = new StackPanel() {Orientation = Orientation.Vertical};
            var title = new Imagine3DTextBlock()
            {
                Background = new SolidColorBrush(SecondaryColor),
                FontSize = 36,
                Text = "INVOICE",
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right,
                Padding = new Thickness(10),
                Width = PageWidth - SideMargin - SideMargin - LogoImageWidth
            };
            var paddingAboveHeader = 45.0;
            header.Children.Add(new Imagine3DTextBlock() {Height = paddingAboveHeader});
            header.Children.Add(title);
            // Use an image to show the handwriting text.
            var danielBitmap = new BitmapImage();
            danielBitmap.BeginInit();
            danielBitmap.StreamSource = GetImagine3DTextStream();
            danielBitmap.EndInit();
            var danielImage = new Image();
            danielImage.Source = danielBitmap;
            var danielImageWidth = 145.0;
            danielImage.Width = danielImageWidth;
            danielImage.Stretch = Stretch.Uniform;
            var danielImageStackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            danielImageStackPanel.Children.Add(danielImage);
            header.Children.Add(danielImageStackPanel);

            headerStackPanel.Children.Add(header);

            var addressesSectionHeight = 200.0;
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
            glasgowStudStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "The Glasgow Stud,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Burnt Farm Ride,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Crews Hill,",
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });
            glasgowStudStackPanel.Children.Add(new Imagine3DTextBlock
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
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Invoice Number:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Date:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock { Height = detailsPaddingHeight });
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "To:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock { Height = detailsPaddingHeight });
            detailsTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Of:",
                Foreground = PrimaryColorBrush,
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                Margin = new Thickness(0, 1, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            });
            addressesStackPanel.Children.Add(detailsTitlesStackPanel);

            var detailsWidth = PageWidth - SideMargin - SideMargin - addressesColumn1Width - detailsTitlesWidth;
            var detailsStackPanel = new StackPanel
            {
                Width = detailsWidth
            };
            detailsStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = $"{invoiceModel.InvoiceReference:0000}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = $"{invoiceModel.CreatedDate:dd/MM/yyyy}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new Imagine3DTextBlock { Height = detailsPaddingHeight });
            detailsStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = $"{invoiceModel.ClientName}",
                FontSize = 11
            });
            detailsStackPanel.Children.Add(new Imagine3DTextBlock { Height = detailsPaddingHeight });
            foreach (
                var addressLine in
                new string[]
                {
                    invoiceModel.ClientAddressFirstLine, invoiceModel.ClientAddressSecondLine,
                    invoiceModel.ClientAddressThirdLine, invoiceModel.ClientAddressPostCode
                }.Where(
                    (s) => !String.IsNullOrWhiteSpace(s)))
            {
                detailsStackPanel.Children.Add(new Imagine3DTextBlock
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
                Width = PageWidth - SideMargin - SideMargin,
                Height = projectHeight
            };
            var projectTitlesWidth = (PageWidth - SideMargin - SideMargin)/2;
            var projectTitlesStackPanel = new StackPanel
            {
                Width = projectTitlesWidth
            };
            var projectPaddingWidth = 5.0;
            projectTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Project:",
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = PrimaryColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, projectPaddingWidth, 0)
            });
            projectTitlesStackPanel.Children.Add(new Imagine3DTextBlock
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
                Width = PageWidth - SideMargin - SideMargin - projectTitlesWidth - projectPaddingWidth
            };
            projectDetailsStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = $"{invoiceModel.Project}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 4, 0, 0),
                FontSize = 12
            });
            projectDetailsStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = $"{invoiceModel.Description}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 4, 0, 0),
                FontSize = 12
            });
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
            itemListTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "DESCRIPTION",
                Foreground = Brushes.White,
                FontSize = 15,
                Width = itemListTitlesWidth,
                Margin = new Thickness(descriptionLeftPadding, 2, 0, 2)
            });
            itemListTitlesStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "TOTAL",
                Foreground = Brushes.White,
                FontSize = 15,
                Margin = new Thickness(0, 2, 0, 2),
                Width = detailsWidth
            });

            var itemListHeight = 250.0;
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
                itemListItemStackPanel.Children.Add(new Imagine3DTextBlock
                {
                    Text = invoiceItemModel.Description,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 11,
                    Margin = new Thickness(descriptionLeftPadding, 0, 0, 0),
                    Width = itemListTitlesWidth
                });
                itemListItemStackPanel.Children.Add(new Imagine3DTextBlock
                {
                    Text = invoiceItemModel.NetAmount.ToString("C", CultureInfo.CurrentCulture),
                    FontSize = 11,
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
            totalDueStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "TOTAL DUE",
                Margin = new Thickness(0, 2, 2, 2),
                FontWeight = FontWeights.Bold,
                Foreground = PrimaryColorBrush,
                Width = itemListTitlesWidth,
                TextAlignment = TextAlignment.Right,
                FontSize = 17
            });
            var totalBorder = new Border
            {
                BorderThickness = new Thickness(0, 2, 0, 2),
                BorderBrush = PrimaryColorBrush,
                Background = SecondaryColorBrush,
                Width = itemListWidth - itemListTitlesWidth,
                Child = new Imagine3DTextBlock
                {
                    Text = invoiceModel.GrossAmount.ToString("C", CultureInfo.CurrentCulture),
                    Margin = new Thickness(2, 0, 0, 0),
                    FontSize = 17,
                    FontWeight = FontWeights.Bold
                }
            };
            totalDueStackPanel.Children.Add(totalBorder);

            var footerStackPanel = new StackPanel
            {
                Margin = new Thickness(0, 20, 0, 0)
            };
            footerStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Payment terms are 14 days from the date of invoice. Late payments will be charged at a rate of 2% per month.",
                TextAlignment = TextAlignment.Center,
                FontSize = 11
            });
            footerStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Payment can be made via cheque, made payable to Imagine3D Ltd. or via bank transfer to",
                TextAlignment = TextAlignment.Center,
                FontSize = 11
            });
            footerStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Acccount Number: 92563142, Sort Code 40-01-06",
                TextAlignment = TextAlignment.Center,
                FontSize = 11
            });
            footerStackPanel.Children.Add(new Imagine3DTextBlock
            {
                Text = "Company Registration No: 7595218",
                TextAlignment = TextAlignment.Center,
                FontSize = 11
            });

            pageStackPanel.Children.Add(headerStackPanel);
            pageStackPanel.Children.Add(addressesStackPanel);
            pageStackPanel.Children.Add(projectStackPanel);
            pageStackPanel.Children.Add(itemListTitlesStackPanel);
            pageStackPanel.Children.Add(itemListStackPanel);
            pageStackPanel.Children.Add(totalDueStackPanel);
            pageStackPanel.Children.Add(footerStackPanel);
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
    }
}
