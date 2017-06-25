using MahApps.Metro.Controls;
using OfficeDeploymentCompanion.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OfficeDeploymentCompanion
{
    public partial class Main : MetroWindow
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public Main()
        {
            InitializeComponent();
        }

        private void OnAddedLanguageButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;

            var id = (sender as Button).Tag.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            var languageToRemove = ViewModel.CurrentConfiguration.AddedLanguages.SingleOrDefault(l => l.Id == id);
            if (languageToRemove != null)
                ViewModel.CurrentConfiguration.AddedLanguages.Remove(languageToRemove);
        }
    }
}
