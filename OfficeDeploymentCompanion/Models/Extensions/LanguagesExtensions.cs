using OfficeDeploymentCompanion.ViewModels;
using System;
using static OfficeDeploymentCompanion.Models.Languages;

namespace OfficeDeploymentCompanion.Models
{
    public static class LanguagesExtensions
    {
        public static ConfigurationModel.Language ToConfigurationModelLanguage(this Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            return new ConfigurationModel.Language
            {
                Name = $"{language.Name} ({language.Id})",
                Id = language.Id
            };
        }

        public static AddedLanguagesViewModel.Language ToAddedLanguagesViewModelLanguage(this Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            return new AddedLanguagesViewModel.Language
            {
                Name = $"{language.Name} ({language.Id})",
                Id = language.Id
            };
        }
    }
}
