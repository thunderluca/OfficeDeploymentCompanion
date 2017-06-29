using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.WorkerServices
{
    public class ExcludedProductsViewModelWorkerServices
    {
        public ExcludedProductsViewModel GetExcludedProductsViewModel(IEnumerable<string> alreadyExcludedProductsIds)
        {
            var availableProducts = this.GetAvailableProducts();

            var alreadyExcludedProducts = alreadyExcludedProductsIds != null && alreadyExcludedProductsIds.Count() > 0
                ? availableProducts.Where(p => alreadyExcludedProductsIds.Any(epId => epId == p.Id))
                : new ExcludedProductsViewModel.Product[0];

            return new ExcludedProductsViewModel(availableProducts, alreadyExcludedProducts);
        }

        public List<ExcludedProductsViewModel.Product> GetAvailableProducts()
        {
            var products = Products.AvailableDictionary
                .OrderBy(p => p.Name)
                .Select(p => p.ToExcludedProductsViewModelProduct())
                .ToList();

            products.Insert(index: 0, item: new ExcludedProductsViewModel.Product
            {
                Name = "Select a program",
                Id = string.Empty
            });

            return products;
        }
    }
}
