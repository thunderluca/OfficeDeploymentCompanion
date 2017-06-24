using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public static class ConfigurationModelExtensions
    {
        public static void TryAddLanguageIfValid(this ConfigurationModel model, string id)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(id) || id == "MatchOS") return;

            var language = model.AvailableLanguages.SingleOrDefault(al => string.Equals(al.Id, id, StringComparison.OrdinalIgnoreCase));
            if (language != null)
                model.AddedLanguages.Add(language);
        }

        public static void TryAddExcludedProductIfValid(this ConfigurationModel model, string id)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(id)) return;

            var product = model.AvailableProducts.SingleOrDefault(ap => string.Equals(ap.Id, id, StringComparison.OrdinalIgnoreCase));
            if (product != null)
                model.ExcludedProducts.Add(product);
        }
    }
}
