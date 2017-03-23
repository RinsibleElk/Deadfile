using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

    internal static class ThemeUtils
    {
        public static void SetThemeFromProperties()
        {
            var theme = PropertiesAccessor.GetTheme();
            var accent = PropertiesAccessor.GetAccent();
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(accent),
                                        ThemeManager.GetAppTheme(theme));
            var resources = Application.Current.Resources;
            var themables = resources.MergedDictionaries.FirstOrDefault((r) => r.Source?.ToString().ToLower().Contains("themables") ?? false);
            if (themables == null) return;
            resources.MergedDictionaries.Remove(themables);
            resources.MergedDictionaries.Add(themables);
        }
    }
}
