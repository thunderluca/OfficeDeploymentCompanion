using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        public MainViewModel(MainViewModelWorkerServices workerServices)
        {
            if (workerServices == null)
                throw new ArgumentNullException(nameof(workerServices));

            this.WorkerServices = workerServices;

            if (this.CurrentConfiguration == null)
                this.CurrentConfiguration = this.WorkerServices.InitializeConfiguration();

            this.SelectedFilePath = this.WorkerServices.GetDefaultFilePath();
        }

        private string _selectedFilePath;
        private ConfigurationModel _currentConfiguration;
        private RelayCommand _loadCommand, _saveCommand, _downloadCommand, _installCommand;
        private RelayCommand<CancelEventArgs> _windowClosingCommand;

        public string Title
        {
            get { return "Office Deployment Tool Companion"; }
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
                    _loadCommand = new RelayCommand(async () =>
                    {
                        var filePath = this.WorkerServices.GetConfigurationFilePath();
                        if (string.IsNullOrWhiteSpace(filePath)) return;

                        this.SelectedFilePath = filePath;

                        var configuration = await this.WorkerServices.LoadConfigurationAsync(filePath);
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
                    _downloadCommand = new RelayCommand(async () =>
                    {
                        if (string.IsNullOrWhiteSpace(this.SelectedFilePath)) return;

                        await this.WorkerServices.DownloadAsync(this.SelectedFilePath, this.CurrentConfiguration);
                    });
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

                        await this.WorkerServices.InstallAsync(this.SelectedFilePath, this.CurrentConfiguration);
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
                            var messageBoxResult = MessageBox.Show(
                                messageBoxText: "The current configuration is not saved, do you want to save it before exit?",
                                caption: "Unsaved changes detected",
                                button: MessageBoxButton.YesNo);

                            if (messageBoxResult == MessageBoxResult.Yes || messageBoxResult == MessageBoxResult.OK)
                            {
                                await SaveConfigurationAsync();

                                Application.Current.Shutdown();
                            }
                        }
                    });
                }

                return _windowClosingCommand;
            }
        }

        private async Task SaveConfigurationAsync()
        {
            var filePath = this.WorkerServices.SaveConfigurationFilePath();
            if (string.IsNullOrWhiteSpace(filePath)) return;

            this.SelectedFilePath = filePath;

            try
            {
                await this.WorkerServices.CreateConfigurationAsync(filePath, this.CurrentConfiguration);
                MessageBox.Show("Configuration file successfully saved!");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Configuration file not saved! Error: {exception.Message}");
            }
        }
    }
}