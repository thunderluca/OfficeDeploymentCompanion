using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace OfficeDeploymentCompanion.Models
{
    public static class Products
    {
        private static Product[] _available;
        public static Product[] Available
        {
            get
            {
                if (_available == null)
                {
                    var resourceStream = Application.GetResourceStream(new Uri("Resources/products.json", UriKind.Relative));
                    if (resourceStream == null)
                        throw new ArgumentNullException(nameof(resourceStream));

                    using (var reader = new StreamReader(resourceStream.Stream))
                    {
                        var content = reader.ReadToEnd();

                        _available = JsonConvert.DeserializeObject<Product[]>(content);
                    }
                }

                return _available;
            }
        }
        
        public class Product
        {
            public string Name { get; set; }
            
            public string Id { get; set; }
        }
    }
}
