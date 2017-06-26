using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OfficeDeploymentCompanion.WorkerServices;
using System;
using System.Collections.Generic;
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

        public bool ShowUnsavedChangesAlert { get; set; }

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
                    _saveCommand = new RelayCommand(() =>
                    {
                        var filePath = this.WorkerServices.SaveConfigurationFilePath();
                        if (string.IsNullOrWhiteSpace(filePath)) return;

                        this.SelectedFilePath = filePath;

                        try
                        {
                            this.WorkerServices.CreateConfiguration(filePath, this.CurrentConfiguration);
                            MessageBox.Show("Configuration file successfully saved!");
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show($"Configuration file not saved! Error: {exception.Message}");
                        }
                    });
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

                        await this.WorkerServices.DownloadAsync(this.SelectedFilePath);
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
                    _installCommand = new RelayCommand(() =>
                    {
                        if (string.IsNullOrWhiteSpace(this.SelectedFilePath)) return;

                        this.WorkerServices.Install(this.SelectedFilePath);
                    });
                }

                return _installCommand;
            }
        }
    }
}