using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deadfile.Infrastructure.Behaviors
{
    public static class ButtonHelper
    {
        public static readonly DependencyProperty ActiveImageSourceProperty = DependencyProperty.RegisterAttached(
            "ActiveImageSource",
            typeof(ImageSource),
            typeof(ButtonHelper));

        public static void SetActiveImageSource(UIElement element, ImageSource value)
        {
            element.SetValue(ActiveImageSourceProperty, value);
        }

        public static ImageSource GetActiveImageSource(UIElement element)
        {
            return (ImageSource)element.GetValue(ActiveImageSourceProperty);
        }

        public static readonly DependencyProperty HoverImageSourceProperty = DependencyProperty.RegisterAttached(
            "HoverImageSource",
            typeof(ImageSource),
            typeof(ButtonHelper));

        public static void SetHoverImageSource(UIElement element, ImageSource value)
        {
            element.SetValue(HoverImageSourceProperty, value);
        }

        public static ImageSource GetHoverImageSource(UIElement element)
        {
            return (ImageSource)element.GetValue(HoverImageSourceProperty);
        }
    }
}
