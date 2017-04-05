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
        public static readonly DependencyProperty PrimaryGeometryPathProperty = DependencyProperty.RegisterAttached(
            "PrimaryGeometryPath",
            typeof(Geometry),
            typeof(ButtonHelper));

        public static void SetPrimaryGeometryPath(UIElement element, Geometry value)
        {
            element.SetValue(PrimaryGeometryPathProperty, value);
        }

        public static Geometry GetPrimaryGeometryPath(UIElement element)
        {
            return (Geometry)element.GetValue(PrimaryGeometryPathProperty);
        }

        public static readonly DependencyProperty SecondaryGeometryPathProperty = DependencyProperty.RegisterAttached(
            "SecondaryGeometryPath",
            typeof(Geometry),
            typeof(ButtonHelper));

        public static void SetSecondaryGeometryPath(UIElement element, Geometry value)
        {
            element.SetValue(SecondaryGeometryPathProperty, value);
        }

        public static Geometry GetSecondaryGeometryPath(UIElement element)
        {
            return (Geometry)element.GetValue(SecondaryGeometryPathProperty);
        }

        public static readonly DependencyProperty TertiaryGeometryPathProperty = DependencyProperty.RegisterAttached(
            "TertiaryGeometryPath",
            typeof(Geometry),
            typeof(ButtonHelper));

        public static void SetTertiaryGeometryPath(UIElement element, Geometry value)
        {
            element.SetValue(TertiaryGeometryPathProperty, value);
        }

        public static Geometry GetTertiaryGeometryPath(UIElement element)
        {
            return (Geometry)element.GetValue(TertiaryGeometryPathProperty);
        }

        public static readonly DependencyProperty OriginalSizeProperty = DependencyProperty.RegisterAttached(
            "OriginalSize",
            typeof(Point),
            typeof(ButtonHelper));

        public static void SetOriginalSize(UIElement element, Point value)
        {
            element.SetValue(OriginalSizeProperty, value);
        }

        public static Point GetOriginalSize(UIElement element)
        {
            return (Point)element.GetValue(OriginalSizeProperty);
        }
    }
}
