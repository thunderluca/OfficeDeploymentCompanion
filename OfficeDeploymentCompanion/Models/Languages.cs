﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace OfficeDeploymentCompanion.Models
{
    public static class Languages
    {
        private static Language[] _available;
        public static Language[] Available
        {
            get
            {
                if (_available == null)
                {
                    var resourceStream = Application.GetResourceStream(new Uri("Resources/languages.json", UriKind.Relative));
                    if (resourceStream == null)
                        throw new ArgumentNullException(nameof(resourceStream));

                    using (var reader = new StreamReader(resourceStream.Stream))
                    {
                        var content = reader.ReadToEnd();

                        _available = JsonConvert.DeserializeObject<Language[]>(content);
                    }
                }

                return _available;
            }
        }

        public class Language
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
