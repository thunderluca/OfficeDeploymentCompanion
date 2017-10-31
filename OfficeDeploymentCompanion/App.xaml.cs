using MahApps.Metro;
using System;
using System.Windows;

namespace OfficeDeploymentCompanion
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.AddAccent("OfficeAccent", new Uri($"pack://application:,,,/{nameof(OfficeDeploymentCompanion)};component/Resources/OfficeAccent.xaml"));

            var theme = ThemeManager.DetectAppStyle(Current);

            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("OfficeAccent"), theme.Item1);
        }
    }
}
