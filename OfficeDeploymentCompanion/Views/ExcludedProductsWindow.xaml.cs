using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using OfficeDeploymentCompanion.Messages;
using OfficeDeploymentCompanion.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OfficeDeploymentCompanion.Views
{
    public partial class ExcludedProductsWindow : MetroWindow
    {
        private ExcludedProductsViewModel ViewModel
        {
            get { return DataContext as ExcludedProductsViewModel; }
        }

        public ExcludedProductsWindow(ExcludedProductsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            this.ProductsComboBox.SelectedIndex = 0;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => this.Close();

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            var productsIds = ViewModel.ExcludedProducts?.Select(p => p.Id).ToArray() ?? new string[0];

            var message = new ExcludedProductsMessage(productsIds);

            Messenger.Default.Send(message);

            this.Close();
        }

        private void OnExcludedProductButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;

            var id = (sender as Button).Tag.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            var productToReInclude = ViewModel.ExcludedProducts.SingleOrDefault(p => p.Id == id);
            if (productToReInclude != null)
                ViewModel.ExcludedProducts.Remove(productToReInclude);
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
