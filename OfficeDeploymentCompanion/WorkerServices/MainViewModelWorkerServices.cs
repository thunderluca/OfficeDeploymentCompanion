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
                        languages: configuration.AddedLanguages.Select(l => l.CultureName),
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
                SelectedArchitecture = CpuArchitecture.X86
            };
        }

        public ConfigurationModel LoadConfiguration(string filePath)
        {
            using (var xmlReader = XmlReader.Create(filePath))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                break;
                            }
                        case XmlNodeType.Attribute:
                            {
                                break;
                            }
                    }
                }
            }
        }

        public List<ConfigurationModel.Language> GetAvailableLanguages()
        {
            return Languages.AvailableDictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new ConfigurationModel.Language
                {
                    Name = kvp.Key,
                    CultureName = kvp.Value
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
