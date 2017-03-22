using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Deadfile.Infrastructure.Styles;
using MahApps.Metro;
using Accent = Deadfile.Infrastructure.Styles.Accent;

namespace Deadfile
{
    internal static class PropertiesAccessor
    {
        public static string GetTheme()
        {
            Theme theme;
            if (!Enum.TryParse(Properties.Settings.Default.Theme, out theme))
            {
                theme = Theme.BaseDark;
                Properties.Settings.Default.Theme = theme.ToString();
                Properties.Settings.Default.Save();
            }
            return theme.ToString();
        }
        public static string GetAccent()
        {
            Accent accent;
            if (!Enum.TryParse(Properties.Settings.Default.Accent, out accent))
            {
                accent = Accent.Red;
                Properties.Settings.Default.Accent = accent.ToString();
                Properties.Settings.Default.Save();
            }
            return accent.ToString();
        }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Change app style to the Deadfile theme.
            var theme = PropertiesAccessor.GetTheme();
            var accent = PropertiesAccessor.GetAccent();
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(accent),
                                        ThemeManager.GetAppTheme(theme));

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }
}
