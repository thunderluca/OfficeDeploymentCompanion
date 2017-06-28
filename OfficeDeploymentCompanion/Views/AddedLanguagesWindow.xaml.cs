using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using OfficeDeploymentCompanion.Messages;
using OfficeDeploymentCompanion.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OfficeDeploymentCompanion.Views
{
    public partial class AddedLanguagesWindow : MetroWindow
    {
        private AddedLanguagesViewModel ViewModel
        {
            get { return DataContext as AddedLanguagesViewModel; }
        }

        public AddedLanguagesWindow(AddedLanguagesViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            this.LanguagesComboBox.SelectedIndex = 0;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => this.Close();

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            var languagesIds = ViewModel.AddedLanguages != null
                ? ViewModel.AddedLanguages.Select(l => l.Id).ToArray()
                : new string[0];

            var message = new AddedLanguagesMessage(languagesIds);

            Messenger.Default.Send(message);

            this.Close();
        }

        private void OnAddedLanguageButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;

            var id = (sender as Button).Tag.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            var languageToRemove = ViewModel.AddedLanguages.SingleOrDefault(l => l.Id == id);
            if (languageToRemove != null)
                ViewModel.AddedLanguages.Remove(languageToRemove);
        }

        private void OnCommonComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox)) return;

            var comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex != 0)
                comboBox.SelectedIndex = 0;
        }
    }
}
