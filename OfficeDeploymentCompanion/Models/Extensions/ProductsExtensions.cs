using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Windows.Media;
using static OfficeDeploymentCompanion.Models.Products;

namespace OfficeDeploymentCompanion.Models
{
    public static class ProductsExtensions
    {
        public static ConfigurationModel.Product ToConfigurationModelProduct(this Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var pathColor = (Color)ColorConverter.ConvertFromString(product.Color);

            return new ConfigurationModel.Product
            {
                Name = product.Name,
                Id = product.Id,
                PathData = product.Path,
                PathBrush = new SolidColorBrush(pathColor)
            };
        }

        public static ExcludedProductsViewModel.Product ToExcludedProductsViewModelProduct(this Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var pathColor = (Color)ColorConverter.ConvertFromString(product.Color);

            return new ExcludedProductsViewModel.Product
            {
                Name = product.Name,
                Id = product.Id,
                PathData = product.Path,
                PathBrush = new SolidColorBrush(pathColor)
            };
        }
    }
}
