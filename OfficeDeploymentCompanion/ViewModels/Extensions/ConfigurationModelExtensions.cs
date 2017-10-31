using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.Resources;
using System;
using System.Linq;

namespace OfficeDeploymentCompanion.ViewModels
{
    public static class ConfigurationModelExtensions
    {
        public static void TryAddLanguageIfValid(this ConfigurationModel model, string id)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(id) || id == Constants.DefaultLanguageMatchOSId) return;

            var language = Languages.AvailableDictionary.SingleOrDefault(al => string.Equals(al.Id, id, StringComparison.OrdinalIgnoreCase));
            if (language == null) return;

            model.AddedLanguages.Add(language.ToConfigurationModelLanguage());
        }

        public static void TryAddExcludedProductIfValid(this ConfigurationModel model, string id)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(id)) return;

            var product = Products.AvailableDictionary.SingleOrDefault(ap => string.Equals(ap.Id, id, StringComparison.OrdinalIgnoreCase));
            if (product == null) return;

            model.ExcludedProducts.Add(product.ToConfigurationModelProduct());
        }
    }
}
