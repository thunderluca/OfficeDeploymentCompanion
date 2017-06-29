using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OfficeDeploymentCompanion.Models.Products;

namespace OfficeDeploymentCompanion.Models
{
    public static class ProductsExtensions
    {
        public static ConfigurationModel.Product ToConfigurationModelProduct(this Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return new ConfigurationModel.Product
            {
                Name = product.Name,
                Id = product.Id
            };
        }

        public static ExcludedProductsViewModel.Product ToExcludedProductsViewModelProduct(this Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return new ExcludedProductsViewModel.Product
            {
                Name = product.Name,
                Id = product.Id,
                IconKey = $"{product.Id}IconKey"
            };
        }
    }
}
