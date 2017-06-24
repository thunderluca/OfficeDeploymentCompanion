using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OfficeDeploymentCompanion.WorkerServices
{
    public class MainViewModelWorkerServices
    {
        public string CreateDefaultConfiguration() => CreateConfiguration(
            fileName: "configuration.xml", 
            folder: Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            configuration: new ConfigurationModel());

        public string CreateConfiguration(
            string fileName, 
            string folder,
            ConfigurationModel configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //if (configuration.SelectedArchitecture == CpuArchitecture.ARM 
            //    || configuration.SelectedArchitecture == CpuArchitecture.ARM64)
            //{
            //    throw new NotSupportedException("ARM architecture is not supported by Office Deployment Tool");
            //}

            var filePath = Path.Combine(folder, fileName);
            var xmlWriterSettings = GetDefaultXmlWriterSettings();

            var OSBitArchitecture = configuration.SelectedArchitecture.GetOSArchitecture();

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
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                }
            }

            return filePath;
        }

        public ConfigurationModel InitializeConfiguration()
        {
            var languages = GetAvailableLanguages();
            var products = GetAvailableProducts();

            return new ConfigurationModel
            {
                AvailableLanguages = languages,
                AvailableProducts = products,
                AddedLanguages = new ObservableCollection<ConfigurationModel.Language>(),
                ExcludedProducts = new ObservableCollection<ConfigurationModel.Product>(),
                EnableUpdates = true,
                SelectedChannel = Channel.Current,
                SelectedArchitecture = OfficeClientEdition.X86
            };
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
                                                configurationModel.SelectedArchitecture = OfficeClientEdition.X86;
                                                break;
                                            case 64:
                                                configurationModel.SelectedArchitecture = OfficeClientEdition.X64;
                                                break;
                                            default:
                                                throw new NotSupportedException($"Invalid OfficeClientEdition: {officeClientEdition}");
                                        }
                                    }

                                    var channel = xmlReader.GetAttribute("Channel");
                                    if (!string.IsNullOrWhiteSpace(channel))
                                        configurationModel.SelectedChannel = (Channel)Enum.Parse(typeof(Channel), channel);
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
                                    configurationModel.EnableUpdates = GetValueFromBoolean(enableUpdates);
                                    break;
                                }
                            case "Display":
                                {
                                    var displayLevel = xmlReader.GetAttribute("Level");
                                    if (!string.IsNullOrWhiteSpace(displayLevel))
                                        configurationModel.DisplayLevel = (DisplayLevel)Enum.Parse(typeof(DisplayLevel), displayLevel);

                                    var acceptEula = xmlReader.GetAttribute("AcceptEULA");
                                    configurationModel.AcceptEula = GetValueFromBoolean(acceptEula);
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
                                                configurationModel.AutoActivate = GetValueFromBit(value);
                                                break;
                                            }
                                        case "FORCEAPPSHUTDOWN":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.ForceAppShutdown = GetValueFromBoolean(value);
                                                break;
                                            }
                                        case "PinIconsToTaskBar":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.PinIconsToTaskBar = GetValueFromBoolean(value);
                                                break;
                                            }
                                        case "SharedComputerLicensing":
                                            {
                                                var value = xmlReader.GetAttribute("Value");
                                                configurationModel.SharedComputerLicensing = GetValueFromBit(value);
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

        private static bool GetValueFromBit(string value) => !string.IsNullOrWhiteSpace(value) ? value == "1" : false;

        private static bool GetValueFromBoolean(string value) => !string.IsNullOrWhiteSpace(value) ? Boolean.Parse(value) : false;

        public List<ConfigurationModel.Language> GetAvailableLanguages()
        {
            return Languages.AvailableDictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new ConfigurationModel.Language
                {
                    Name = kvp.Key,
                    Id = kvp.Value
                })
                .ToList();
        }

        public List<ConfigurationModel.Product> GetAvailableProducts()
        {
            return Products.AvailableDictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new ConfigurationModel.Product
                {
                    Name = kvp.Key,
                    Id = kvp.Value
                })
                .ToList();
        }

        private static XmlWriterSettings GetDefaultXmlWriterSettings() => new XmlWriterSettings
        {
            CloseOutput = true,
            ConformanceLevel = ConformanceLevel.Fragment,
            Indent = true,
            OmitXmlDeclaration = true
        };
    }
}
