using GalaSoft.MvvmLight;
using OfficeDeploymentCompanion.WorkerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainViewModelWorkerServices WorkerServices;

        public MainViewModel(MainViewModelWorkerServices workerServices)
        {
            if (workerServices == null)
                throw new ArgumentNullException(nameof(workerServices));

            this.WorkerServices = workerServices;

            this.AvailableLanguages = this.WorkerServices.GetAvailableLanguages();
            this.AvailableProducts = this.WorkerServices.GetAvailableProducts();
        }

        private Language _selectedAvailableLanguage, _selectedAddedLanguage;
        private List<Language> _availableLanguages;
        private ObservableCollection<Language> _addedLanguages;

        private Product _selectedAvailableProduct, _selectedExcludedProduct;
        private List<Product> _availableProducts;
        private ObservableCollection<Product> _excludedProducts;

        public string Title
        {
            get { return "Office Deployment Companion"; }
        }

        public List<Language> AvailableLanguages
        {
            get { return _availableLanguages; }
            set { Set(nameof(AvailableLanguages), ref _availableLanguages, value); }
        }

        public Language SelectedAvailableLanguage
        {
            get { return _selectedAvailableLanguage; }
            set { Set(nameof(SelectedAvailableLanguage), ref _selectedAvailableLanguage, value); }
        }

        public ObservableCollection<Language> AddedLanguages
        {
            get { return _addedLanguages; }
            set { Set(nameof(AddedLanguages), ref _addedLanguages, value); }
        }

        public Language SelectedAddedLanguage
        {
            get { return _selectedAddedLanguage; }
            set { Set(nameof(SelectedAddedLanguage), ref _selectedAddedLanguage, value); }
        }

        public List<Product> AvailableProducts
        {
            get { return _availableProducts; }
            set { Set(nameof(AvailableProducts), ref _availableProducts, value); }
        }

        public Product SelectedAvailableProduct
        {
            get { return _selectedAvailableProduct; }
            set { Set(nameof(SelectedAvailableProduct), ref _selectedAvailableProduct, value); }
        }

        public ObservableCollection<Product> ExcludedProducts
        {
            get { return _excludedProducts; }
            set { Set(nameof(ExcludedProducts), ref _excludedProducts, value); }
        }

        public Product SelectedExcludedProduct
        {
            get { return _selectedExcludedProduct; }
            set { Set(nameof(SelectedExcludedProduct), ref _selectedExcludedProduct, value); }
        }

        public class Language
        {
            public string Name { get; set; }

            public string CultureName { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}