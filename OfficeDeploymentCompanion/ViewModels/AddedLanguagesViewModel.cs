using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class AddedLanguagesViewModel : ViewModelBase
    {
        public AddedLanguagesViewModel(
               List<Language> availableLanguages,
               IEnumerable<Language> alreadyAddedLanguages)
        {
            if (availableLanguages == null)
                throw new ArgumentNullException(nameof(availableLanguages));

            this.AvailableLanguages = availableLanguages;
            this.AddedLanguages = new ObservableCollection<Language>();

            if (alreadyAddedLanguages == null || alreadyAddedLanguages.Count() == 0) return;

            foreach (var ep in alreadyAddedLanguages)
                this.AddedLanguages.Add(ep);
        }

        private Language _selectedLanguage;
        private List<Language> _availableLanguages;
        private ObservableCollection<Language> _addedLanguages;

        public List<Language> AvailableLanguages
        {
            get { return _availableLanguages; }
            set { Set(nameof(AvailableLanguages), ref _availableLanguages, value); }
        }

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (!Set(nameof(SelectedLanguage), ref _selectedLanguage, value)) return;

                if (_selectedLanguage == null) return;

                if (string.IsNullOrWhiteSpace(_selectedLanguage.Id) ||
                    this.AddedLanguages.Any(l => l.Id == _selectedLanguage.Id)) return;

                this.AddedLanguages.Add(_selectedLanguage);

                this.SelectedLanguage = null;
            }
        }

        public ObservableCollection<Language> AddedLanguages
        {
            get { return _addedLanguages; }
            set { Set(nameof(AddedLanguages), ref _addedLanguages, value); }
        }

        public class Language
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
