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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private static readonly Color PrimaryColor = Color.FromRgb(31, 73, 125);
        private static readonly Color SecondaryColor = Color.FromRgb(220, 230, 241);
        private static readonly Brush PrimaryColorBrush = new SolidColorBrush(PrimaryColor);
        private static readonly Brush SecondaryColorBrush = new SolidColorBrush(SecondaryColor);
        private static readonly FontFamily Calibri = new FontFamily("Calibri");

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
            var sideMargin = 30;
            var verticalMargin = 50;
            page.Margin = new Thickness(sideMargin, verticalMargin, sideMargin, verticalMargin);
            var pageStackPanel = new StackPanel();

            // At the top, there's a stack panel.
            var headerStackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

            // A vertical stack panel for the address.
            var header = new StackPanel() {Orientation = Orientation.Vertical};
            headerStackPanel.Children.Add(header);

            // AddressFirstLine at the top left.
            var logoWidth = 200.0;
            var addressWidth = pageWidth - sideMargin - sideMargin - logoWidth;
            var glasgowStudAddress = new StackPanel
            {
                Width = addressWidth
            };
            glasgowStudAddress.Children.Add(new PaulSamsonAddressTextBlock { Text = "THE GLASGOW STUD," });
            glasgowStudAddress.Children.Add(new PaulSamsonAddressTextBlock { Text = "BURNT FARM RIDE," });
            glasgowStudAddress.Children.Add(new PaulSamsonAddressTextBlock { Text = "CREWS HILL, EN2 9DY" });
            headerStackPanel.Children.Add(glasgowStudAddress);

            // Logo image at the top right.
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = GetPaulSamsonLogoStream();
            bi.EndInit();
            var i = new Image();
            i.Source = bi;
            i.Width = logoWidth;
            i.Stretch = Stretch.Uniform;
            headerStackPanel.Children.Add(i);

            var invoiceHeader = new Border
            {
                BorderThickness = new Thickness(0),
                Margin = new Thickness(20, 20, 20, 50),
                Background = SecondaryColorBrush,
                Child = new PaulSamsonTextBlock
                {
                    Text = "INVOICE",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 15,
                    Foreground = PrimaryColorBrush,
                    FontWeight = FontWeights.Bold
                }
            };

            // The top level properties of the invoice.
            var invoicePropertiesHeight = 160.0;
            var invoicePropertiesStackPanel = new StackPanel
            {
                Width = pageWidth - sideMargin - sideMargin - 4,
                Height = invoicePropertiesHeight
            };
            var invoicePropertiesFirstRowStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMargin - sideMargin - 4,
                Margin = new Thickness(20)
            };
            var invoicePropertiesColumnWidth = (pageWidth - sideMargin - sideMargin - 4 - 20)/4;
            invoicePropertiesFirstRowStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Invoice Number:")
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesFirstRowStackPanel.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceModel.InvoiceReference.ToString())
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesFirstRowStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Invoiced To:")
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesFirstRowStackPanel.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceModel.ClientName)
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesStackPanel.Children.Add(invoicePropertiesFirstRowStackPanel);
            var invoicePropertiesSecondAndThirdRowsStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMargin - sideMargin - 4,
                Margin = new Thickness(20, 0, 20, 0)
            };
            var invoiceDateAndPropertyTitles = new StackPanel();
            invoiceDateAndPropertyTitles.Children.Add(new PaulSamsonFieldTitleTextBlock("Invoice Date:")
            {
                Margin = new Thickness(0, 0, 0, 20),
                Width = invoicePropertiesColumnWidth
            });
            invoiceDateAndPropertyTitles.Children.Add(new PaulSamsonFieldTitleTextBlock("Property:")
            {
                Width = invoicePropertiesColumnWidth,
                Margin = new Thickness(0, 0, 0, 20)
            });
            var invoiceDateAndPropertyValues = new StackPanel();
            invoiceDateAndPropertyValues.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceModel.CreatedDate.ToString("dd/MM/yyyy"))
            {
                Margin = new Thickness(0, 0, 0, 20),
                Width = invoicePropertiesColumnWidth
            });
            invoiceDateAndPropertyValues.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceModel.Project, true)
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesSecondAndThirdRowsStackPanel.Children.Add(invoiceDateAndPropertyTitles);
            invoicePropertiesSecondAndThirdRowsStackPanel.Children.Add(invoiceDateAndPropertyValues);
            invoicePropertiesSecondAndThirdRowsStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Invoice AddressFirstLine:")
            {
                Width = invoicePropertiesColumnWidth
            });
            var invoiceAddressStackPanel = new StackPanel
            {
                Width = invoicePropertiesColumnWidth
            };
            foreach (var addressLine in new string[] {invoiceModel.ClientAddressFirstLine, invoiceModel.ClientAddressSecondLine, invoiceModel.ClientAddressThirdLine, invoiceModel.ClientAddressPostCode}.Where((s) => !String.IsNullOrWhiteSpace(s)))
            {
                invoiceAddressStackPanel.Children.Add(new PaulSamsonFieldValueTextBlock(addressLine));
            }
            invoicePropertiesSecondAndThirdRowsStackPanel.Children.Add(invoiceAddressStackPanel);
            invoicePropertiesStackPanel.Children.Add(invoicePropertiesSecondAndThirdRowsStackPanel);
            var invoicePropertiesFourthRowStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMargin - sideMargin - 4,
                Margin = new Thickness(20, 0, 20, 0)
            };
            invoicePropertiesFourthRowStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Job Description:")
            {
                Width = invoicePropertiesColumnWidth
            });
            invoicePropertiesFourthRowStackPanel.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceModel.Description)
            {
                Width = invoicePropertiesColumnWidth * 3,
                TextWrapping = TextWrapping.Wrap
            });
            invoicePropertiesStackPanel.Children.Add(invoicePropertiesFourthRowStackPanel);
            var invoiceProperties =
                new Border
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = PrimaryColorBrush,
                    Child = invoicePropertiesStackPanel
                };

            // Itemised bit.
            var sideMarginForItemised = 20;
            var itemisedHeight = 180;
            var marginAboveAndBelowItemised = 20;
            var itemizedPropertiesStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMarginForItemised - sideMarginForItemised - sideMargin - sideMargin,
                Height = itemisedHeight,
                Margin = new Thickness(sideMarginForItemised, marginAboveAndBelowItemised, sideMarginForItemised, marginAboveAndBelowItemised)
            };
            var itemizedTitleWidth = 30;
            itemizedPropertiesStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("For:")
            {
                Width = itemizedTitleWidth
            });
            var itemizedObjectsStackPanel = new StackPanel
            {
                Width = pageWidth - sideMarginForItemised - sideMarginForItemised - sideMargin - sideMargin - itemizedTitleWidth
            };
            foreach (var invoiceItemModel in invoiceModel.ChildrenList)
            {
                itemizedObjectsStackPanel.Children.Add(new PaulSamsonFieldValueTextBlock(invoiceItemModel.Description)
                {
                    Margin = new Thickness(0, 0, 0, 20)
                });
            }
            itemizedPropertiesStackPanel.Children.Add(itemizedObjectsStackPanel);

            var gapToLeftOfPrices = 400.0;
            var pricesTitlesWidth = 150.0;
            var pricesValuesWidth = pageWidth - sideMargin - sideMargin - gapToLeftOfPrices - pricesTitlesWidth;
            var pricesRowStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = pageWidth - sideMargin - sideMargin
            };
            pricesRowStackPanel.Children.Add(new StackPanel
            {
                Width = gapToLeftOfPrices
            });
            var pricesStackPanel = new StackPanel
            {
                Width = pageWidth - sideMargin - sideMargin - gapToLeftOfPrices
            };
            pricesRowStackPanel.Children.Add(pricesStackPanel);
            var netAmountStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 20)
            };
            netAmountStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Fee For Above:")
            {
                Width = pricesTitlesWidth
            });
            netAmountStackPanel.Children.Add(
                new PaulSamsonFieldValueTextBlock(invoiceModel.NetAmount.ToString("C", CultureInfo.CurrentCulture))
                {
                    Width = pricesValuesWidth
                });
            var vatValueStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 20)
            };
            vatValueStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock($"VAT @ {invoiceModel.VatRate}%")
            {
                Width = pricesTitlesWidth
            });
            vatValueStackPanel.Children.Add(
                new PaulSamsonFieldValueTextBlock(invoiceModel.VatValue.ToString("C", CultureInfo.CurrentCulture))
                {
                    Width = pricesValuesWidth
                });
            var grossAmountStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 50)
            };
            grossAmountStackPanel.Children.Add(new PaulSamsonFieldTitleTextBlock("Total amount due:")
            {
                Width = pricesTitlesWidth,
                FontSize = 14
            });
            grossAmountStackPanel.Children.Add(
                new Border
                {
                    Background = SecondaryColorBrush,
                    BorderBrush = PrimaryColorBrush,
                    BorderThickness = new Thickness(0, 2, 0, 2),
                    Child =
                        new PaulSamsonFieldValueTextBlock(invoiceModel.GrossAmount.ToString("C", CultureInfo.CurrentCulture), true)
                        {
                            Width = pricesValuesWidth,
                            FontSize = 15
                        }
                });
            pricesStackPanel.Children.Add(netAmountStackPanel);
            pricesStackPanel.Children.Add(vatValueStackPanel);
            pricesStackPanel.Children.Add(grossAmountStackPanel);

            // T's and C's.
            var termsAndConditions = new StackPanel
            {
                Width = pageWidth - sideMargin - sideMargin
            };
            termsAndConditions.Children.Add(new PaulSamsonTsAndCsTextBlock("Payment terms are 14 days from the date of invoice. Late payments will be charged at a rate of 2% per"));
            termsAndConditions.Children.Add(new PaulSamsonTsAndCsTextBlock("month. Payment can be made via cheque, made payable to PAUL SAMSON or via bank transfer to Account"));
            termsAndConditions.Children.Add(new PaulSamsonTsAndCsTextBlock("Number: 80780111 Sort Code 20‐52‐74"));
            termsAndConditions.Children.Add(new PaulSamsonTsAndCsTextBlock("VAT Registration No: 713 9749 10"));

            var footerStackPanel = new StackPanel
            {
                Width = pageWidth - sideMargin - sideMargin
            };
            // Logo image at the top right.
            var footerBi = new BitmapImage();
            footerBi.BeginInit();
            footerBi.StreamSource = GetPaulSamsonFooterStream();
            footerBi.EndInit();
            var footerImage = new Image();
            footerImage.Source = footerBi;
            footerImage.Width = pageWidth - sideMargin - sideMargin;
            footerImage.Stretch = Stretch.Uniform;
            footerStackPanel.Children.Add(footerImage);

            // Add all to page.
            pageStackPanel.Children.Add(headerStackPanel);
            pageStackPanel.Children.Add(invoiceHeader);
            pageStackPanel.Children.Add(invoiceProperties);
            pageStackPanel.Children.Add(itemizedPropertiesStackPanel);
            pageStackPanel.Children.Add(pricesRowStackPanel);
            pageStackPanel.Children.Add(termsAndConditions);
            pageStackPanel.Children.Add(footerStackPanel);
            page.Children.Add(pageStackPanel);
            var pageContent = new PageContent();
            ((IAddChild) pageContent).AddChild(page);
            doc.Pages.Add(pageContent);
            return doc;
        }

        private static Stream GetPaulSamsonLogoStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.pksLogo.png";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private static Stream GetPaulSamsonFooterStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Deadfile.Pdf.Resources.pksFooter.png";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private class PaulSamsonTextBlock : TextBlock
        {
            public PaulSamsonTextBlock()
            {
                FontFamily = Calibri;
            }
        }

        private class PaulSamsonFieldTitleTextBlock : PaulSamsonTextBlock
        {
            public PaulSamsonFieldTitleTextBlock(string text) : base()
            {
                FontSize = 11;
                TextAlignment = TextAlignment.Left;
                VerticalAlignment = VerticalAlignment.Top;
                HorizontalAlignment = HorizontalAlignment.Left;
                Foreground = PrimaryColorBrush;
                FontWeight = FontWeights.Bold;
                Text = text;
            }
        }

        private class PaulSamsonFieldValueTextBlock : PaulSamsonTextBlock
        {
            public PaulSamsonFieldValueTextBlock(string text) : this(text, false)
            {
            }

            public PaulSamsonFieldValueTextBlock(string text, bool isBold) : base()
            {
                FontSize = 11;
                FontSize = 11;
                TextAlignment = TextAlignment.Left;
                VerticalAlignment = VerticalAlignment.Top;
                HorizontalAlignment = HorizontalAlignment.Left;
                Foreground = Brushes.Black;
                FontWeight = (isBold) ? FontWeights.Bold : FontWeights.Regular;
                Text = text;
                TextWrapping = TextWrapping.Wrap;
            }
        }

        private class PaulSamsonTsAndCsTextBlock : PaulSamsonTextBlock
        {
            public PaulSamsonTsAndCsTextBlock(string text) : base()
            {
                FontSize = 10;
                TextAlignment = TextAlignment.Center;
                HorizontalAlignment = HorizontalAlignment.Center;
                Foreground = PrimaryColorBrush;
                Text = text;
            }
        }

        private class PaulSamsonAddressTextBlock : PaulSamsonTextBlock
        {
            public PaulSamsonAddressTextBlock() : base()
            {
                FontSize = 11;
                Foreground = PrimaryColorBrush;
                FontWeight = FontWeights.Bold;
            }
        }
    }
}
