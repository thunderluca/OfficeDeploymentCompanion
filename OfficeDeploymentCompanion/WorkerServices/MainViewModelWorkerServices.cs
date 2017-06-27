using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using OfficeDeploymentCompanion.Helpers;
using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.Resources;
using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OfficeDeploymentCompanion.WorkerServices
{
    public class MainViewModelWorkerServices
    {
        private readonly IDialogCoordinator DialogCoordinator;

        public MainViewModelWorkerServices(IDialogCoordinator dialogCoordinator)
        {
            if (dialogCoordinator == null)
                throw new ArgumentNullException(nameof(dialogCoordinator));

            this.DialogCoordinator = dialogCoordinator;
        }

        private static Process DownloadProcess { get; set; }

        public string GetDefaultFilePath() => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Constants.DefaultConfigurationFileName);

        public async Task CreateConfigurationAsync(string filePath, ConfigurationModel configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            
            var xmlWriterSettings = XmlWriterHelper.GetDefaultXmlWriterSettings();

            var OSBitArchitecture = configuration.SelectedEdition.GetOSArchitecture();

            await Task.Run(() =>
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var xmlWriter = XmlWriter.Create(fileStream, xmlWriterSettings))
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteStartElement("Add");
                        xmlWriter.WriteAttributeString("OfficeClientEdition", OSBitArchitecture.ToString());
                        xmlWriter.WriteAttributeString("Channel", configuration.SelectedChannel.ToString("G"));
                        xmlWriter.WriteOffice365ProPlusRetailProductElement(
                            languages: configuration.AddedLanguages.Select(l => l.Id),
                            excludedAppIds: configuration.ExcludedProducts.Select(p => p.Id));
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteUpdatesElement(configuration.EnableUpdates, configuration.SelectedChannel);
                        xmlWriter.WriteDisplayElement(configuration.SilentMode, configuration.AcceptEula);
                        xmlWriter.WritePropertyElement("AUTOACTIVATE", (configuration.AutoActivate.GetBitStringFromBoolean()));
                        xmlWriter.WritePropertyElement("FORCEAPPSHUTDOWN", (configuration.ForceAppShutdown.GetBooleanStringFromBoolean()));
                        xmlWriter.WritePropertyElement("PinIconsToTaskBar", (configuration.PinIconsToTaskBar.GetBooleanStringFromBoolean()));
                        xmlWriter.WritePropertyElement("SharedComputerLicensing", (configuration.SharedComputerLicensing.GetBitStringFromBoolean()));
                        xmlWriter.WriteEndElement();
                        xmlWriter.Flush();
                    }
                }
            });
        }

        public ConfigurationModel InitializeConfiguration()
        {
            var languages = GetAvailableLanguages();
            var products = GetAvailableProducts();

            var channels = EnumHelper.GetEnumValuesArray<Channel>().ToList();
            var editions = EnumHelper.GetEnumValuesArray<OfficeClientEdition>().ToList();

            return new ConfigurationModel(languages, products, channels, editions);
        }

        public ConfigurationModel LoadConfiguration(string filePath)
        {
            var configurationModel = InitializeConfiguration();

            try
            {
                using (var xmlReader = XmlReader.Create(filePath))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType != XmlNodeType.Element) continue;

                        switch (xmlReader.Name)
                        {
                            case "Configuration":
                            case "Product":
                                break;
                            case "Add":
                                {
                                    var officeClientEdition = xmlReader.GetAttribute("OfficeClientEdition");
                                    if (!string.IsNullOrWhiteSpace(officeClientEdition))
                                    {
                                        switch (Convert.ToInt32(officeClientEdition))
                                        {
                                            case 32:
                                                configurationModel.SelectedEdition = OfficeClientEdition.X86;
                                                break;
                                            case 64:
                                                configurationModel.SelectedEdition = OfficeClientEdition.X64;
                                                break;
                                            default:
                                                throw new NotSupportedException($"Invalid OfficeClientEdition: {officeClientEdition}");
                                        }
                                    }

                                    var channel = xmlReader.GetAttribute("Channel");
                                    if (!string.IsNullOrWhiteSpace(channel))
                                        configurationModel.SelectedChannel = channel.ToEnum<Channel>();
                                    break;
                                }
                            case "Language":
                                {
                                    var id = xmlReader.GetAttribute("ID");
                                    configurationModel.TryAddLanguageIfValid(id);
                                    break;
                                }
                            case "ExcludeApp":
                                {
                                    var id = xmlReader.GetAttribute("ID");
                                    configurationModel.TryAddExcludedProductIfValid(id);
                                    break;
                                }
                            case "Updates":
                                {
                                    var enableUpdates = xmlReader.GetAttribute("Enabled");
                                    configurationModel.EnableUpdates = enableUpdates.GetValueFromBoolean();
                                    break;
                                }
                            case "Display":
                                {
                                    var displayLevel = xmlReader.GetAttribute("Level");
                                    if (!string.IsNullOrWhiteSpace(displayLevel))
                                        configurationModel.SilentMode = displayLevel.ToEnum<DisplayLevel>() == DisplayLevel.None;

                                    var acceptEula = xmlReader.GetAttribute("AcceptEULA");
                                    configurationModel.AcceptEula = acceptEula.GetValueFromBoolean();
                                    break;
                                }
                            case "Property":
                                {
                                    var name = xmlReader.GetAttribute("Name");
                                    switch (name.ToUpper())
                                    {
                                        case "AUTOACTIVATE":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.AutoActivate = value.GetValueFromBit();
                                                break;
                                            }
                                        case "FORCEAPPSHUTDOWN":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.ForceAppShutdown = value.GetValueFromBoolean();
                                                break;
                                            }
                                        case "PinIconsToTaskBar":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.PinIconsToTaskBar = value.GetValueFromBoolean();
                                                break;
                                            }
                                        case "SharedComputerLicensing":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.SharedComputerLicensing = value.GetValueFromBit();
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                    }

                    return configurationModel;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetConfigurationFilePath()
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = Constants.DefaultFileDialogExtension,
                Filter = "Office XML configuration file (.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result.GetValueOrDefault() != true) return null;

            return openFileDialog.FileName;
        }

        public string SaveConfigurationFilePath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = Constants.DefaultConfigurationFileName,
                DefaultExt = Constants.DefaultFileDialogExtension,
                Filter = "Office XML configuration file (.xml)|*.xml"
            };
            var result = saveFileDialog.ShowDialog();

            if (result.GetValueOrDefault() != true) return null;

            return saveFileDialog.FileName;
        }

        public List<ConfigurationModel.Language> GetAvailableLanguages()
        {
            var languages = Languages.AvailableDictionary
                .OrderBy(l => l.Name)
                .Select(l => new ConfigurationModel.Language
                {
                    Name = $"{l.Name} ({l.Id})",
                    Id = l.Id
                })
                .ToList();

            languages.Insert(index: 0, item: new ConfigurationModel.Language
            {
                Name = "Select a language to include in Office installation",
                Id = string.Empty
            });

            return languages;
        }

        public List<ConfigurationModel.Product> GetAvailableProducts()
        {
            var products = Products.AvailableDictionary
                .OrderBy(p => p.Name)
                .Select(p => new ConfigurationModel.Product
                {
                    Name = p.Name,
                    Id = p.Id
                })
                .ToList();

            products.Insert(index: 0, item: new ConfigurationModel.Product
            {
                Name = "Select a product to exclude from Office installation",
                Id = string.Empty
            });

            return products;
        }

        public async Task<bool> CheckRequirementsAsync(string filePath, ConfigurationModel configuration)
        {
            try
            {
                var continueOperation = await CheckConfigurationFileAsync(filePath, configuration);
                if (!continueOperation) return false;

                var folder = Path.GetDirectoryName(filePath);

                var result = CheckSetupFile(folder);
                return result;
            }
            catch (Exception exception)
            {
                await DialogCoordinator.ShowMessageAsync(RetrieveDialogCoordinatorContext(), title: "Error", message: $"Error: {exception.Message}");
                return false;
            }
        }

        public async Task DownloadAsync(string filePath, ConfigurationModel configuration)
        {
            try
            {
                await Task.Run(() =>
                {
                    var folder = Path.GetDirectoryName(filePath);

                    var processStartInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        WorkingDirectory = folder,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = Constants.DefaultSetupFileName,
                        Arguments = $"/download \"{filePath}\""
                    };

                    DownloadProcess = new Process
                    {
                        StartInfo = processStartInfo
                    };

                    DownloadProcess.Start();
                    DownloadProcess.WaitForExit();

                    DownloadProcess = null;
                });
            }
            catch (Exception exception)
            {
                await DialogCoordinator.ShowMessageAsync(RetrieveDialogCoordinatorContext(), title: "Error", message: $"Error: {exception.Message}");
            }
        }

        public void CancelDownload()
        {
            try
            {
                if (DownloadProcess == null) return;

                var processes = Process.GetProcessesByName(DownloadProcess.ProcessName);
                if (processes == null) return;

                foreach (var p in processes)
                    p.Kill();

                DownloadProcess = null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DidUserDownloadPackages(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);

            var officeFolderPath = Path.Combine(folder, "Office");

            if (!Directory.Exists(officeFolderPath)) return false;

            var folders = Directory.EnumerateDirectories(officeFolderPath);
            return folders != null && folders.Count() > 0;
        }

        public void Install(string filePath, ConfigurationModel configuration)
        {
            try
            {
                var folder = Path.GetDirectoryName(filePath);

                var processStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = folder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "setup.exe",
                    Arguments = $"/configure \"{filePath}\""
                };

                var process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.Start();
            }
            catch (Exception exception)
            {
                DialogCoordinator.ShowMessageAsync(RetrieveDialogCoordinatorContext(), title: "Error", message: $"Error: {exception.Message}");
            }
        }

        private async Task<bool> CheckConfigurationFileAsync(string filePath, ConfigurationModel configuration)
        {
            if (File.Exists(filePath)) return true;

            var messageDialogResult = await DialogCoordinator.ShowMessageAsync(
                RetrieveDialogCoordinatorContext(),
                title: "Configuration not found",
                message: "No configuration saved found, do you want to create it with selected unsaved options?",
                style: MessageDialogStyle.AffirmativeAndNegative);

            if (messageDialogResult != MessageDialogResult.Affirmative) return false;

            await this.CreateConfigurationAsync(filePath, configuration);

            return true;
        }

        private bool CheckSetupFile(string folder)
        {
            var setupFilePath = Path.Combine(folder, Constants.DefaultSetupFileName);
            if (File.Exists(setupFilePath)) return true;

            DialogCoordinator.ShowMessageAsync(
                RetrieveDialogCoordinatorContext(),
                title: "Setup not found",
                message: "Office Deployment Tool setup.exe doesn't exists in current configuration file path, do you want to download it?",
                style: MessageDialogStyle.AffirmativeAndNegative).ContinueWith(task => 
                {
                    if (task.Result == MessageDialogResult.Affirmative)
                        Process.Start(new ProcessStartInfo(Constants.OfficeDeploymentToolDownloadUrl));
                });

            return false;
        }

        public bool HasCurrentConfigurationUnsavedChanges(ConfigurationModel configuration)
        {
            var defaultConfiguration = InitializeConfiguration();

            return !defaultConfiguration.Equals(configuration);
        }

        private static object RetrieveDialogCoordinatorContext() => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
