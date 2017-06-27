using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
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
        private readonly MainViewModelWorkerServices WorkerServices;
        private readonly IDialogCoordinator DialogCoordinator;

        public MainViewModel(MainViewModelWorkerServices workerServices, IDialogCoordinator dialogCoordinator)
        {
            if (workerServices == null)
                throw new ArgumentNullException(nameof(workerServices));

            if (dialogCoordinator == null)
                throw new ArgumentNullException(nameof(dialogCoordinator));

            this.WorkerServices = workerServices;
            this.DialogCoordinator = dialogCoordinator;

            if (this.CurrentConfiguration == null)
                this.CurrentConfiguration = this.WorkerServices.InitializeConfiguration();

            this.SelectedFilePath = this.WorkerServices.GetDefaultFilePath();
        }

        private bool _isBusy, _skipClosingPopup;
        private string _selectedFilePath;
        private ConfigurationModel _currentConfiguration;
        private RelayCommand _loadCommand, _saveCommand, _downloadCommand, _installCommand;
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
                        var filePath = this.WorkerServices.GetConfigurationFilePath();
                        if (string.IsNullOrWhiteSpace(filePath)) return;

                        this.SelectedFilePath = filePath;

                        var configuration = this.WorkerServices.LoadConfiguration(filePath);
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
                        
                        var officeFilesExist = this.WorkerServices.DidUserDownloadPackages(this.SelectedFilePath);
                        if (!officeFilesExist)
                        {
                            var downloadResult = await DownloadFilesAsync();
                            if (!downloadResult) return;
                        }
                        else
                        {
                            var checkResult = await this.WorkerServices.CheckRequirementsAsync(this.SelectedFilePath, this.CurrentConfiguration);
                            if (!checkResult) return;
                        }

                        this.WorkerServices.Install(this.SelectedFilePath, this.CurrentConfiguration);
                    });
                }

                return _installCommand;
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
                        var hasUnsavedChanges = this.WorkerServices.HasCurrentConfigurationUnsavedChanges(this.CurrentConfiguration);
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

                            _skipClosingPopup = true;
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

            var checkResult = await this.WorkerServices.CheckRequirementsAsync(this.SelectedFilePath, this.CurrentConfiguration);
            if (!checkResult) return false;

            var progressDialogController = await DialogCoordinator.ShowProgressAsync(
                                context: this,
                                title: "Downloading Office packages",
                                message: "Please wait, this process will take minutes or hours depending on your Internet connection download speed",
                                isCancelable: true).ConfigureAwait(false);
            progressDialogController.SetIndeterminate();
            progressDialogController.Canceled += OnProgressDialogControllerCanceled;

            await this.WorkerServices.DownloadAsync(this.SelectedFilePath, this.CurrentConfiguration);

            progressDialogController.Canceled -= OnProgressDialogControllerCanceled;
            await progressDialogController.CloseAsync();

            return true;
        }

        private void OnProgressDialogControllerCanceled(object sender, EventArgs e) => this.WorkerServices.CancelDownload();

        private async Task SaveConfigurationAsync()
        {
            var filePath = this.WorkerServices.SaveConfigurationFilePath();
            if (string.IsNullOrWhiteSpace(filePath)) return;

            this.SelectedFilePath = filePath;

            try
            {
                await this.WorkerServices.CreateConfigurationAsync(filePath, this.CurrentConfiguration);
                await DialogCoordinator.ShowMessageAsync(context: this, title: "Operation completed", message: "Configuration file successfully saved!");
            }
            catch (Exception exception)
            {
                await DialogCoordinator.ShowMessageAsync(context: this, title: "Operation failed", message: $"Configuration file not saved! Error: {exception.Message}");
            }
        }
    }
}