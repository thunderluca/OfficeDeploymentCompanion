﻿using GalaSoft.MvvmLight;
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
        //public ConfigurationModel(
        //    List<Language> availableLanguages,
        //    List<Product> availableProducts,
        //    IEnumerable<Language> addedLanguages,
        //    IEnumerable<Product> excludedProducts)
        //{
        //    if (availableLanguages == null)
        //        throw new ArgumentNullException(nameof(availableLanguages));

        //    if (availableProducts == null)
        //        throw new ArgumentNullException(nameof(availableProducts));

        //    this.AvailableLanguages = availableLanguages;

        //    this.AvailableProducts = availableProducts;

        //    this.AddedLanguages = addedLanguages != null 
        //        ? addedLanguages.ToObservableCollection() 
        //        : new ObservableCollection<Language>();

        //    this.ExcludedProducts = excludedProducts != null
        //        ? excludedProducts.ToObservableCollection()
        //        : new ObservableCollection<Product>();
        //}

        private Language _selectedAvailableLanguage, _selectedAddedLanguage;
        private List<Language> _availableLanguages;
        private ObservableCollection<Language> _addedLanguages;
        private Product _selectedAvailableProduct, _selectedExcludedProduct;
        private List<Product> _availableProducts;
        private ObservableCollection<Product> _excludedProducts;
        private bool _enableUpdates, _autoActivate, _forceAppShutdown, _pinIconsToTaskBar;
        private bool _sharedComputerLicensing, _silentMode, _acceptEula;
        private Channel _selectedChannel;
        private OfficeClientEdition _selectedArchitecture;
        private DisplayLevel _displayLevel;

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

        public OfficeClientEdition SelectedArchitecture
        {
            get { return _selectedArchitecture; }
            set { Set(nameof(SelectedArchitecture), ref _selectedArchitecture, value); }
        }

        public Channel SelectedChannel
        {
            get { return _selectedChannel; }
            set { Set(nameof(SelectedChannel), ref _selectedChannel, value); }
        }

        public DisplayLevel DisplayLevel
        {
            get { return _displayLevel; }
            set { Set(nameof(DisplayLevel), ref _displayLevel, value); }
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
