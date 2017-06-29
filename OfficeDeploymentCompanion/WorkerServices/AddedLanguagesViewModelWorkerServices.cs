using OfficeDeploymentCompanion.Models;
using OfficeDeploymentCompanion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.WorkerServices
{
    public class AddedLanguagesViewModelWorkerServices
    {
        public AddedLanguagesViewModel GetAddedLanguagesViewModel(IEnumerable<string> alreadyAddedLanguagesIds)
        {
            var availableLanguages = this.GetAvailableLanguages();

            var alreadyAddedLanguages = alreadyAddedLanguagesIds != null && alreadyAddedLanguagesIds.Count() > 0
                ? availableLanguages.Where(p => alreadyAddedLanguagesIds.Any(epId => epId == p.Id))
                : new AddedLanguagesViewModel.Language[0];

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
