using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OfficeDeploymentCompanion.WorkerServices
{
    public class AddedLanguagesViewModelWorkerServices
    {
        public AddedLanguagesViewModel GetAddedLanguagesViewModel(IEnumerable<string> alreadyAddedLanguagesIds)
        {
            var availableLanguages = this.GetAvailableLanguages();

            if (alreadyAddedLanguagesIds == null || alreadyAddedLanguagesIds.Count() == 0)
                return new AddedLanguagesViewModel(availableLanguages, new AddedLanguagesViewModel.Language[0]);

            var alreadyAddedLanguages = availableLanguages.Where(p => alreadyAddedLanguagesIds.Any(epId => epId == p.Id));

            return new AddedLanguagesViewModel(availableLanguages, alreadyAddedLanguages);
        }

        public List<AddedLanguagesViewModel.Language> GetAvailableLanguages()
        {
            var languages = Languages.AvailableDictionary
                .OrderBy(l => l.Name)
                .Select(l => l.ToAddedLanguagesViewModelLanguage())
                .ToList();

            languages.Insert(index: 0, item: new AddedLanguagesViewModel.Language
            {
                Name = "Select a language",
                Id = string.Empty
            });

            return languages;
        }
    }
}
