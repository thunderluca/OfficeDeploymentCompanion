using GalaSoft.MvvmLight;
using OfficeDeploymentCompanion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class ConfigurationModel : ViewModelBase
    {
        public ConfigurationModel(
            List<Language> availableLanguages,
            List<Product> availableProducts,
            List<Channel> availableChannels,
            List<OfficeClientEdition> availableEditions)
        {
            AddedLanguages = new ObservableCollection<Language>();
            ExcludedProducts = new ObservableCollection<Product>();
            EnableUpdates = true;
            AvailableLanguages = availableLanguages;
            AvailableProducts = availableProducts;
            AvailableChannels = availableChannels;
            AvailableEditions = availableEditions;

            SelectedLanguage = AvailableLanguages.FirstOrDefault(l => string.IsNullOrWhiteSpace(l.Id));
            SelectedProduct = AvailableProducts.FirstOrDefault(p => string.IsNullOrWhiteSpace(p.Id));
            SelectedChannel = AvailableChannels.FirstOrDefault();
            SelectedEdition = AvailableEditions.FirstOrDefault();
        }

        private Language _selectedLanguage;
        private List<Language> _availableLanguages;
        private ObservableCollection<Language> _addedLanguages;
        private Product _selectedProduct;
        private List<Product> _availableProducts;
        private ObservableCollection<Product> _excludedProducts;
        private bool _enableUpdates, _autoActivate, _forceAppShutdown, _pinIconsToTaskBar;
        private bool _sharedComputerLicensing, _silentMode, _acceptEula;
        private Channel _selectedChannel;
        private List<Channel> _availableChannels;
        private OfficeClientEdition _selectedEdition;
        private List<OfficeClientEdition> _availableEditions;

        public List<Language> AvailableLanguages
        {
            get { return _availableLanguages; }
            set { Set(nameof(AvailableLanguages), ref _availableLanguages, value); }
        }

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (!Set(nameof(SelectedLanguage), ref _selectedLanguage, value)) return;

                if (_selectedLanguage == null) return;

                if (string.IsNullOrWhiteSpace(_selectedLanguage.Id) || 
                    this.AddedLanguages.Any(l => l.Id == _selectedLanguage.Id)) return;

                this.AddedLanguages.Add(_selectedLanguage);

                this.SelectedLanguage = null;
            }
        }

        public ObservableCollection<Language> AddedLanguages
        {
            get { return _addedLanguages; }
            set { Set(nameof(AddedLanguages), ref _addedLanguages, value); }
        }

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

        public bool EnableUpdates
        {
            get { return _enableUpdates; }
            set { Set(nameof(EnableUpdates), ref _enableUpdates, value); }
        }

        public bool AutoActivate
        {
            get { return _autoActivate; }
            set { Set(nameof(AutoActivate), ref _autoActivate, value); }
        }

        public bool ForceAppShutdown
        {
            get { return _forceAppShutdown; }
            set { Set(nameof(ForceAppShutdown), ref _forceAppShutdown, value); }
        }

        public bool PinIconsToTaskBar
        {
            get { return _pinIconsToTaskBar; }
            set { Set(nameof(PinIconsToTaskBar), ref _pinIconsToTaskBar, value); }
        }

        public bool SharedComputerLicensing
        {
            get { return _sharedComputerLicensing; }
            set { Set(nameof(SharedComputerLicensing), ref _sharedComputerLicensing, value); }
        }

        public bool SilentMode
        {
            get { return _silentMode; }
            set { Set(nameof(SilentMode), ref _silentMode, value); }
        }

        public bool AcceptEula
        {
            get { return _acceptEula; }
            set { Set(nameof(AcceptEula), ref _acceptEula, value); }
        }

        public Channel SelectedChannel
        {
            get { return _selectedChannel; }
            set { Set(nameof(SelectedChannel), ref _selectedChannel, value); }
        }

        public List<Channel> AvailableChannels
        {
            get { return _availableChannels; }
            set { Set(nameof(AvailableChannels), ref _availableChannels, value); }
        }

        public OfficeClientEdition SelectedEdition
        {
            get { return _selectedEdition; }
            set { Set(nameof(SelectedEdition), ref _selectedEdition, value); }
        }

        public List<OfficeClientEdition> AvailableEditions
        {
            get { return _availableEditions; }
            set { Set(nameof(AvailableEditions), ref _availableEditions, value); }
        }

        public class Language
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
