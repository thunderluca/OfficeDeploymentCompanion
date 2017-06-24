using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Xml
{
    public static class XmlWriterExtensions
    {
        public static void WriteOffice365ProPlusRetailProductElement(
            this XmlWriter xmlWriter, 
            IList<string> languages = null, 
            IList<string> excludedApps = null)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            xmlWriter.WriteStartElement("Product");
            xmlWriter.WriteAttributeString("ID", "O365ProPlusRetail");

            if (languages == null || languages.Count == 0)
            {
                xmlWriter.WriteDefaultLanguageElement();
            }
            else
            {
                foreach (var language in languages)
                    xmlWriter.WriteLanguageElement(language);
            }

            if (excludedApps != null && excludedApps.Count > 0)
            {
                foreach (var excludedApp in excludedApps)
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

            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("ID", languageCultureName);
            xmlWriter.WriteFullEndElement();
            xmlWriter.WriteEndElement();
        }

        private static void WriteExcludeAppElement(this XmlWriter xmlWriter, string excludedAppName)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (string.IsNullOrWhiteSpace(excludedAppName))
                throw new ArgumentNullException(nameof(excludedAppName));

            xmlWriter.WriteStartElement("ExcludeApp");
            xmlWriter.WriteAttributeString("ID", excludedAppName);
            xmlWriter.WriteFullEndElement();
            xmlWriter.WriteEndElement();
        }
    }
}
