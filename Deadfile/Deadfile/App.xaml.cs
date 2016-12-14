using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using MahApps.Metro;

namespace Deadfile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DeadfileAccent = "DeadfileAccent";
        private const string DeadfileTheme = "DeadfileTheme";

        protected override void OnStartup(StartupEventArgs e)
        {
            // Add Deadfile Accent and Theme Resource Dictionaries to the ThemeManager.
            ThemeManager.AddAccent(DeadfileAccent, new Uri("pack://application:,,,/Deadfile.Infrastructure;component/Styles/DeadfileAccent.xaml"));
            ThemeManager.AddAppTheme(DeadfileTheme, new Uri("pack://application:,,,/Deadfile.Infrastructure;component/Styles/DeadfileTheme.xaml"));

            // Change app style to the Deadfile theme.
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(DeadfileAccent),
                                        ThemeManager.GetAppTheme(DeadfileTheme));


            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }
}
