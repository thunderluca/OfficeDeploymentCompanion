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
            List<Channel> availableChannels,
            List<OfficeClientEdition> availableEditions)
        {
            this.AddedLanguages = new ObservableCollection<Language>();
            this.ExcludedProducts = new ObservableCollection<Product>();
            this.EnableUpdates = true;
            this.AvailableChannels = availableChannels;
            this.AvailableEditions = availableEditions;
            this.SelectedChannel = AvailableChannels.FirstOrDefault();
            this.SelectedEdition = AvailableEditions.FirstOrDefault();
        }
        
        private ObservableCollection<Language> _addedLanguages, _previousLanguagesToRemove;
        private ObservableCollection<Product> _excludedProducts;
        private string _sourcePath, _downloadPath, _version;
        private bool _enableUpdates, _autoActivate, _forceAppShutdown, _pinIconsToTaskBar;
        private bool _sharedComputerLicensing, _silentMode, _acceptEula, _removePreviousOfficeInstallations;
        private bool _forceUpgradeFromOffice2013, _useCustomDownloadPath, _useCustomSourcePath;
        private Channel _selectedChannel;
        private List<Channel> _availableChannels;
        private OfficeClientEdition _selectedEdition;
        private List<OfficeClientEdition> _availableEditions;

        public ObservableCollection<Language> AddedLanguages
        {
            get { return _addedLanguages; }
            set { Set(nameof(AddedLanguages), ref _addedLanguages, value); }
        }

        public ObservableCollection<Product> ExcludedProducts
        {
            get { return _excludedProducts; }
            set { Set(nameof(ExcludedProducts), ref _excludedProducts, value); }
        }

        public bool RemovePreviousOfficeInstallations
        {
            get { return _removePreviousOfficeInstallations; }
            set { Set(nameof(RemovePreviousOfficeInstallations), ref _removePreviousOfficeInstallations, value); }
        }

        public ObservableCollection<Language> PreviousLanguagesToRemove
        {
            get { return _previousLanguagesToRemove; }
            set { Set(nameof(PreviousLanguagesToRemove), ref _previousLanguagesToRemove, value); }
        }

        public bool ForceUpgradeFromOffice2013
        {
            get { return _forceUpgradeFromOffice2013; }
            set { Set(nameof(ForceUpgradeFromOffice2013), ref _forceUpgradeFromOffice2013, value); }
        }

        public bool UseCustomDownloadPath
        {
            get { return _useCustomDownloadPath; }
            set { Set(nameof(UseCustomDownloadPath), ref _useCustomDownloadPath, value); }
        }

        public string DownloadPath
        {
            get { return _downloadPath; }
            set { Set(nameof(DownloadPath), ref _downloadPath, value); }
        }

        public bool UseCustomSourcePath
        {
            get { return _useCustomSourcePath; }
            set { Set(nameof(UseCustomSourcePath), ref _useCustomSourcePath, value); }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
            set { Set(nameof(SourcePath), ref _sourcePath, value); }
        }

        public string Version
        {
            get { return _version; }
            set { Set(nameof(Version), ref _version, value); }
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var otherConfiguration = (ConfigurationModel)obj;
            if (otherConfiguration == null)
                throw new ArgumentNullException(nameof(otherConfiguration));

            return this.AcceptEula.Equals(otherConfiguration.AcceptEula)
                && this.AddedLanguages.SequenceEqual(otherConfiguration.AddedLanguages)
                && this.AutoActivate.Equals(otherConfiguration.AutoActivate)
                && this.EnableUpdates.Equals(otherConfiguration.EnableUpdates)
                && this.ExcludedProducts.SequenceEqual(otherConfiguration.ExcludedProducts)
                && this.ForceAppShutdown.Equals(otherConfiguration.ForceAppShutdown)
                && this.PinIconsToTaskBar.Equals(otherConfiguration.PinIconsToTaskBar)
                && Equals(this.SelectedChannel, otherConfiguration.SelectedChannel)
                && Equals(this.SelectedEdition, otherConfiguration.SelectedEdition)
                && this.SharedComputerLicensing.Equals(otherConfiguration.SharedComputerLicensing)
                && this.SilentMode.Equals(otherConfiguration.SilentMode);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
