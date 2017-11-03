using MahApps.Metro;
using System;
using System.Windows;
using System.Windows.Media;

namespace OfficeDeploymentCompanion
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.AddAccent("OfficeAccent", new Uri($"pack://application:,,,/{nameof(OfficeDeploymentCompanion)};component/Resources/OfficeAccent.xaml"));

            var theme = ThemeManager.DetectAppStyle(Current);

            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("OfficeAccent"), theme.Item1);

            try
            {
                if (SystemParameters.WindowGlassBrush is SolidColorBrush accentColorBrush)
                    UpdateAccentColors(accentColorBrush.Color);
            }
            catch { }
        }

        private void UpdateAccentColors(Color accentColor)
        {
            Resources["AccentColor"] = accentColor;
            var accentColor2 = Color.FromArgb((255 / 5) * 3, accentColor.R, accentColor.G, accentColor.B);
            Resources["AccentColor2"] = accentColor2;
            var accentColor3 = Color.FromArgb((255 / 5) * 2, accentColor.R, accentColor.G, accentColor.B);
            Resources["AccentColor3"] = accentColor3;
            var accentColor4 = Color.FromArgb((255 / 5) * 1, accentColor.R, accentColor.G, accentColor.B);
            Resources["AccentColor4"] = accentColor4;
            var hslColor = accentColor.ToHslColor();
            var highlightHslColor = new HslColor(hslColor.H, (hslColor.S / 3) * 2, hslColor.L);
            Resources["HighlightColor"] = highlightHslColor.ToRgbColor();
        }
    }
}
