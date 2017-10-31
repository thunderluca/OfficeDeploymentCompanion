using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace OfficeDeploymentCompanion.Helpers
{
    public static class XmlWriterHelper
    {
        public static XmlWriterSettings GetDefaultXmlWriterSettings() => new XmlWriterSettings
        {
            CloseOutput = true,
            ConformanceLevel = ConformanceLevel.Fragment,
            Indent = true,
            OmitXmlDeclaration = true
        };

        public static void WriteOffice365ProPlusRetailProductElement(
            this XmlWriter xmlWriter, 
            IEnumerable<string> languages, 
            IEnumerable<string> excludedAppIds)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            xmlWriter.WriteStartElement("Product");
            xmlWriter.WriteAttributeString("ID", "O365ProPlusRetail");

            if (languages == null || languages.Count() == 0)
            {
                xmlWriter.WriteDefaultLanguageElement();
            }
            else
            {
                foreach (var language in languages)
                    xmlWriter.WriteLanguageElement(language);
            }

            if (excludedAppIds?.Count() > 0)
            {
                foreach (var excludedApp in excludedAppIds)
                    xmlWriter.WriteExcludeAppElement(excludedApp);
            }

            xmlWriter.WriteEndElement();
        }

        private static void WriteDefaultLanguageElement(this XmlWriter xmlWriter) => WriteLanguageElement(xmlWriter, Constants.DefaultLanguageMatchOSId);

        private static void WriteLanguageElement(this XmlWriter xmlWriter, string languageCultureName)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (string.IsNullOrWhiteSpace(languageCultureName))
                throw new ArgumentNullException(nameof(languageCultureName));

            if (languageCultureName != Constants.DefaultLanguageMatchOSId 
                && Languages.AvailableDictionary.All(l => l.Id != languageCultureName))
                throw new NotSupportedException($"Unsupported language: {languageCultureName}");

            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("ID", languageCultureName);
            xmlWriter.WriteEndElement();
        }

        private static void WriteExcludeAppElement(this XmlWriter xmlWriter, string excludedAppId)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (string.IsNullOrWhiteSpace(excludedAppId))
                throw new ArgumentNullException(nameof(excludedAppId));

            if (Products.AvailableDictionary.All(p => p.Id != excludedAppId))
                throw new NotSupportedException($"Unsupported product: {excludedAppId}");

            xmlWriter.WriteStartElement("ExcludeApp");
            xmlWriter.WriteAttributeString("ID", excludedAppId);
            xmlWriter.WriteEndElement();
        }

        public static void WriteUpdatesElement(this XmlWriter xmlWriter, bool enableUpdates, Channel channel)
        {
            xmlWriter.WriteStartElement("Updates");
            xmlWriter.WriteAttributeString("Enabled", enableUpdates.GetBooleanStringFromBoolean());
            xmlWriter.WriteAttributeString("Channel", channel.ToString("G"));
            xmlWriter.WriteEndElement();
        }

        public static void WriteDisplayElement(this XmlWriter xmlWriter, bool silentMode, bool acceptEula)
        {
            xmlWriter.WriteStartElement("Display");
            xmlWriter.WriteAttributeString("Level", (silentMode ? DisplayLevel.None : DisplayLevel.Full).ToString());
            xmlWriter.WriteAttributeString("AcceptEULA", acceptEula.GetBooleanStringFromBoolean());
            xmlWriter.WriteEndElement();
        }

        public static void WritePropertyElement(this XmlWriter xmlWriter, string name, string value)
        {
            xmlWriter.WriteStartElement("Property");
            xmlWriter.WriteAttributeString("Name", name);
            xmlWriter.WriteAttributeString("Value", value);
            xmlWriter.WriteEndElement();
        }

        public static void WriteRemoveElement(this XmlWriter xmlWriter, bool removePreviousOfficeInstallations)
        {
            if (!removePreviousOfficeInstallations) return;

            xmlWriter.WriteStartElement("Remove");
            xmlWriter.WriteAttributeString("All", true.GetBooleanStringFromBoolean());
            xmlWriter.WriteFullEndElement();
        }
    }
}
