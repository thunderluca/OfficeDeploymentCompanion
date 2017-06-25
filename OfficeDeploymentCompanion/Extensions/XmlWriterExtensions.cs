using OfficeDeploymentCompanion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml
{
    public static class XmlWriterExtensions
    {
        public static void WriteOffice365ProPlusRetailProductElement(
            this XmlWriter xmlWriter, 
            IEnumerable<string> languages = null, 
            IEnumerable<string> excludedAppIds = null)
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

            if (excludedAppIds != null && excludedAppIds.Count() > 0)
            {
                foreach (var excludedApp in excludedAppIds)
                    xmlWriter.WriteExcludeAppElement(excludedApp);
            }

            xmlWriter.WriteEndElement();
        }

        private static void WriteDefaultLanguageElement(this XmlWriter xmlWriter) => WriteLanguageElement(xmlWriter, "MatchOS");

        private static void WriteLanguageElement(this XmlWriter xmlWriter, string languageCultureName)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (string.IsNullOrWhiteSpace(languageCultureName))
                throw new ArgumentNullException(nameof(languageCultureName));

            if (Languages.AvailableDictionary.All(l => l.Id != languageCultureName))
                throw new NotSupportedException($"Unsupported language: {languageCultureName}");

            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("ID", languageCultureName);
            xmlWriter.WriteFullEndElement();
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
            xmlWriter.WriteFullEndElement();
            xmlWriter.WriteEndElement();
        }
    }
}
