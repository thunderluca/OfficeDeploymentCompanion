using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using OfficeDeploymentCompanion.Messages;
using OfficeDeploymentCompanion.Views;
using OfficeDeploymentCompanion.WorkerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainViewModelWorkerServices MainWorkerServices;
        private readonly AddedLanguagesViewModelWorkerServices AddedLanguagesWorkerServices;
        private readonly ExcludedProductsViewModelWorkerServices ExcludedProductsWorkerServices;
        private readonly IDialogCoordinator DialogCoordinator;

        public MainViewModel(
            MainViewModelWorkerServices mainWorkerServices,
            AddedLanguagesViewModelWorkerServices addedLanguagesWorkerServices,
            ExcludedProductsViewModelWorkerServices excludedProductsWorkerServices,
            IDialogCoordinator dialogCoordinator)
        {
            if (mainWorkerServices == null)
                throw new ArgumentNullException(nameof(mainWorkerServices));

            if (addedLanguagesWorkerServices == null)
                throw new ArgumentNullException(nameof(addedLanguagesWorkerServices));

            if (excludedProductsWorkerServices == null)
                throw new ArgumentNullException(nameof(excludedProductsWorkerServices));

            if (dialogCoordinator == null)
                throw new ArgumentNullException(nameof(dialogCoordinator));

            this.MainWorkerServices = mainWorkerServices;
            this.AddedLanguagesWorkerServices = addedLanguagesWorkerServices;
            this.ExcludedProductsWorkerServices = excludedProductsWorkerServices;
            this.DialogCoordinator = dialogCoordinator;

            if (this.CurrentConfiguration == null)
                this.CurrentConfiguration = this.MainWorkerServices.InitializeConfiguration();

            this.SelectedFilePath = this.MainWorkerServices.GetDefaultFilePath();

            Messenger.Default.Register<ExcludedProductsMessage>(this, ManageExcludedProductsMessage);
            Messenger.Default.Register<AddedLanguagesMessage>(this, ManageAddedLanguagesMessage);
        }

        private bool _isBusy;
        private string _selectedFilePath;
        private ConfigurationModel _currentConfiguration;
        private RelayCommand _loadCommand, _saveCommand, _downloadCommand, _installCommand;
        private RelayCommand _manageAddedLanguagesCommand, _manageExcludedProductsCommand;
        private RelayCommand<CancelEventArgs> _windowClosingCommand;

        public string Title
        {
            get { return "Office 2016 Deployment Tool Companion"; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(nameof(IsBusy), ref _isBusy, value); }
        }

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set { Set(nameof(SelectedFilePath), ref _selectedFilePath, value); }
        }

        public ConfigurationModel CurrentConfiguration
        {
            get { return _currentConfiguration; }
            set { Set(nameof(CurrentConfiguration), ref _currentConfiguration, value); }
        }

        public RelayCommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new RelayCommand(() =>
                    {
                        var filePath = this.MainWorkerServices.GetConfigurationFilePath();
                        if (string.IsNullOrWhiteSpace(filePath)) return;

                        this.SelectedFilePath = filePath;

                        var configuration = this.MainWorkerServices.LoadConfiguration(filePath);
                        if (configuration != null)
                        {
                            this.CurrentConfiguration = configuration;
                            return;
                        }

                        throw new ArgumentNullException(nameof(configuration));
                    });
                }

                return _loadCommand;
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(async () => await SaveConfigurationAsync());
                }

                return _saveCommand;
            }
        }

        public RelayCommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(async () => await DownloadFilesAsync());
                }

                return _downloadCommand;
            }
        }

        public RelayCommand InstallCommand
        {
            get
            {
                if (_installCommand == null)
                {
                    _installCommand = new RelayCommand(async () =>
                    {
                        if (string.IsNullOrWhiteSpace(this.SelectedFilePath)) return;
                        
                        var officeFilesExist = this.MainWorkerServices.DidUserDownloadPackages(this.SelectedFilePath);
                        if (!officeFilesExist)
                        {
                            var downloadResult = await DownloadFilesAsync();
                            if (!downloadResult) return;
                        }
                        else
                        {
                            var checkResult = await this.MainWorkerServices.CheckRequirementsAsync(this.SelectedFilePath, this.CurrentConfiguration);
                            if (!checkResult) return;
                        }

                        this.MainWorkerServices.Install(this.SelectedFilePath, this.CurrentConfiguration);
                    });
                }

                return _installCommand;
            }
        }

        public RelayCommand ManageAddedLanguagesCommand
        {
            get
            {
                if (_manageAddedLanguagesCommand == null)
                {
                    _manageAddedLanguagesCommand = new RelayCommand(() =>
                    {
                        var addedLanguagesIds = this.CurrentConfiguration.AddedLanguages.Select(al => al.Id).ToArray();

                        var model = this.AddedLanguagesWorkerServices.GetAddedLanguagesViewModel(addedLanguagesIds);

                        var excludedProductsWindow = new AddedLanguagesWindow(model);
                        excludedProductsWindow.Show();
                    });
                }

                return _manageAddedLanguagesCommand;
            }
        }

        public RelayCommand ManageExcludedProductsCommand
        {
            get
            {
                if (_manageExcludedProductsCommand == null)
                {
                    _manageExcludedProductsCommand = new RelayCommand(() =>
                    {
                        var excludedProductsIds = this.CurrentConfiguration.ExcludedProducts.Select(ep => ep.Id).ToArray();

                        var model = this.ExcludedProductsWorkerServices.GetExcludedProductsViewModel(excludedProductsIds);

                        var excludedProductsWindow = new ExcludedProductsWindow(model);
                        excludedProductsWindow.Show();
                    });
                }

                return _manageExcludedProductsCommand;
            }
        }

        public RelayCommand<CancelEventArgs> WindowClosingCommand
        {
            get
            {
                if (_windowClosingCommand == null)
                {
                    _windowClosingCommand = new RelayCommand<CancelEventArgs>(async args =>
                    {
                        var hasUnsavedChanges = this.MainWorkerServices.HasCurrentConfigurationUnsavedChanges(this.CurrentConfiguration);
                        if (hasUnsavedChanges)
                        {
                            args.Cancel = true;
                            var result = await DialogCoordinator.ShowMessageAsync(
                                context: this,
                                title: "Unsaved changes detected",
                                message: "The current configuration is not saved, do you want to save it before exit? (If you press Cancel, you will lost all unsaved changes.)",
                                style: MessageDialogStyle.AffirmativeAndNegative);

                            if (result == MessageDialogResult.Affirmative)
                                await SaveConfigurationAsync();
                            
                            Application.Current.Shutdown();
                        }
                    });
                }

                return _windowClosingCommand;
            }
        }

        private async Task<bool> DownloadFilesAsync()
        {
            if (string.IsNullOrWhiteSpace(this.SelectedFilePath)) return false;

            var checkResult = await this.MainWorkerServices.CheckRequirementsAsync(this.SelectedFilePath, this.CurrentConfiguration);
            if (!checkResult) return false;

            var progressDialogController = await DialogCoordinator.ShowProgressAsync(
                                context: this,
                                title: "Downloading Office packages",
                                message: "Please wait, this process will take minutes or hours depending on your Internet connection download speed",
                                isCancelable: true).ConfigureAwait(false);
            progressDialogController.SetIndeterminate();
            progressDialogController.Canceled += OnProgressDialogControllerCanceled;

            await this.MainWorkerServices.DownloadAsync(this.SelectedFilePath, this.CurrentConfiguration);

            progressDialogController.Canceled -= OnProgressDialogControllerCanceled;
            await progressDialogController.CloseAsync();

            return true;
        }

        private void OnProgressDialogControllerCanceled(object sender, EventArgs e) => this.MainWorkerServices.CancelDownload();

        private async Task SaveConfigurationAsync()
        {
            var filePath = this.MainWorkerServices.SaveConfigurationFilePath();
            if (string.IsNullOrWhiteSpace(filePath)) return;

            this.SelectedFilePath = filePath;

            try
            {
                await this.MainWorkerServices.CreateConfigurationAsync(filePath, this.CurrentConfiguration);
                await DialogCoordinator.ShowMessageAsync(context: this, title: "Operation completed", message: "Configuration file successfully saved!");
            }
            catch (Exception exception)
            {
                await DialogCoordinator.ShowMessageAsync(context: this, title: "Operation failed", message: $"Configuration file not saved! Error: {exception.Message}");
            }
        }

        private void ManageExcludedProductsMessage(ExcludedProductsMessage message)
        {
            this.CurrentConfiguration.ExcludedProducts.Clear();

            if (message.ProductsIds.Length == 0) return;

            var excludedProducts = this.CurrentConfiguration.AvailableProducts
                .Where(l => message.ProductsIds.Any(id => id == l.Id))
                .OrderBy(l => l.Name)
                .ToArray();

            foreach (var product in excludedProducts)
                this.CurrentConfiguration.ExcludedProducts.Add(product);
        }

        private void ManageAddedLanguagesMessage(AddedLanguagesMessage message)
        {
            this.CurrentConfiguration.AddedLanguages.Clear();

            if (message.LanguagesIds.Length == 0) return;

            var addedLanguages = this.CurrentConfiguration.AvailableLanguages
                .Where(l => message.LanguagesIds.Any(id => id == l.Id))
                .OrderBy(l => l.Name)
                .ToArray();

            foreach (var language in addedLanguages)
                this.CurrentConfiguration.AddedLanguages.Add(language);
        }
    }
}