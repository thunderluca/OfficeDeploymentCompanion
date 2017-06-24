using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
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
            o365ProPlusSettings: new O365ProPlusSettings());

        public string CreateConfiguration(
            string fileName, 
            string folder,
            O365ProPlusSettings o365ProPlusSettings,
            CpuArchitecture cpuArchitecture = CpuArchitecture.X86,
            Channel channel = Channel.Current)
        {
            if (o365ProPlusSettings == null)
            {
                throw new InvalidOperationException("You need to include at least an Office product/suite");
            }

            if (cpuArchitecture == CpuArchitecture.ARM || cpuArchitecture == CpuArchitecture.ARM64)
            {
                throw new NotSupportedException("ARM architecture is not supported by Office Deployment Tool");
            }

            var filePath = Path.Combine(folder, fileName);
            var xmlWriterSettings = GetDefaultXmlWriterSettings();

            var OSBitArchitecture = cpuArchitecture.GetOSArchitecture();

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var xmlWriter = XmlWriter.Create(fileStream, xmlWriterSettings))
                {
                    xmlWriter.WriteStartElement("Configuration");
                    xmlWriter.WriteStartElement("Add");
                    xmlWriter.WriteAttributeString("OfficeClientEdition", OSBitArchitecture.ToString());
                    xmlWriter.WriteAttributeString("Channel", channel.ToString("G"));

                    if (o365ProPlusSettings != null)
                        xmlWriter.WriteOffice365ProPlusRetailProductElement(
                            languages: o365ProPlusSettings.Languages,
                            excludedApps: o365ProPlusSettings.ExcludedApps);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                }
            }

            return filePath;
        }

        private static XmlWriterSettings GetDefaultXmlWriterSettings() => new XmlWriterSettings
        {
            CloseOutput = true,
            ConformanceLevel = ConformanceLevel.Fragment,
            Indent = true,
            OmitXmlDeclaration = true
        };

        public List<MainViewModel.Language> GetAvailableLanguages()
        {
            return Languages.AvailableDictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new MainViewModel.Language
                {
                    Name = kvp.Key,
                    CultureName = kvp.Value
                })
                .ToList();
        }

        public List<MainViewModel.Product> GetAvailableProducts()
        {
            return Products.AvailableDictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new MainViewModel.Product
                {
                    Name = kvp.Key,
                    Id = kvp.Value
                })
                .ToList();
        }
    }
}
