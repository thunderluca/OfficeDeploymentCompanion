using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class ExcludedProductsViewModel : ViewModelBase
    {
        public ExcludedProductsViewModel(
            List<Product> availableProducts,
            IEnumerable<Product> alreadyExcludedProducts)
        {
            if (availableProducts == null)
                throw new ArgumentNullException(nameof(availableProducts));
            
            this.AvailableProducts = availableProducts;
            this.ExcludedProducts = new ObservableCollection<Product>();

            if (alreadyExcludedProducts == null || alreadyExcludedProducts.Count() == 0) return;

            foreach (var ep in alreadyExcludedProducts)
                this.ExcludedProducts.Add(ep);
        }

        private Product _selectedProduct;
        private List<Product> _availableProducts;
        private ObservableCollection<Product> _excludedProducts;

        public List<Product> AvailableProducts
        {
            get { return _availableProducts; }
            set { Set(nameof(AvailableProducts), ref _availableProducts, value); }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (!Set(nameof(SelectedProduct), ref _selectedProduct, value)) return;

                if (_selectedProduct == null) return;

                if (string.IsNullOrWhiteSpace(_selectedProduct.Id) ||
                    this.ExcludedProducts.Any(l => l.Id == _selectedProduct.Id)) return;

                this.ExcludedProducts.Add(_selectedProduct);

                this.SelectedProduct = null;
            }
        }

        public ObservableCollection<Product> ExcludedProducts
        {
            get { return _excludedProducts; }
            set { Set(nameof(ExcludedProducts), ref _excludedProducts, value); }
        }

        public class Product
        {
            public string Name { get; set; }

            public string Id { get; set; }

            public string IconKey { get; set; }
        }
    }
}
