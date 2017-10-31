namespace OfficeDeploymentCompanion.Models
{
    public static class Languages
    {
        public static Language[] AvailableDictionary = new[]
        {
            new Language { Name = "Arabic", Id = "ar-sa" },
            new Language { Name = "Bulgarian", Id = "bg-bg" },
            new Language { Name = "Chinese (Simplified)", Id = "zh-cn" },
            new Language { Name = "Chinese", Id = "zh-tw" },
            new Language { Name = "Croatian", Id = "hr-hr" },
            new Language { Name = "Czech", Id = "cs-cz" },
            new Language { Name = "Danish", Id = "da-dk" },
            new Language { Name = "Dutch", Id = "nl-nl" },
            new Language { Name = "English", Id = "en-us" },
            new Language { Name = "Estonian", Id = "et-ee" },
            new Language { Name = "Finnish", Id = "fi-fi" },
            new Language { Name = "French", Id = "fr-fr" },
            new Language { Name = "German", Id = "de-de" },
            new Language { Name = "Greek", Id = "el-gr" },
            new Language { Name = "Hebrew", Id = "he-il" },
            new Language { Name = "Hindi", Id = "hi-in" },
            new Language { Name = "Hungarian", Id = "hu-hu" },
            new Language { Name = "Indonesian", Id = "id-id" },
            new Language { Name = "Italian", Id = "it-it" },
            new Language { Name = "Japanese", Id = "ja-jp" },
            new Language { Name = "Kazakh", Id = "kk-kz" },
            new Language { Name = "Korean", Id = "ko-kr" },
            new Language { Name = "Latvian", Id = "lv-lv" },
            new Language { Name = "Lithuanian", Id = "lt-lt" },
            new Language { Name = "Malay", Id = "ms-my" },
            new Language { Name = "Norwegian (Bokmål)", Id = "nb-no" },
            new Language { Name = "Polish", Id = "pl-pl" },
            new Language { Name = "Portuguese", Id = "pt-br" },
            new Language { Name = "Portuguese", Id = "pt-pt" },
            new Language { Name = "Romanian", Id = "ro-ro" },
            new Language { Name = "Russian", Id = "ru-ru" },
            new Language { Name = "Serbian (Latin)", Id = "sr-latn-rs" },
            new Language { Name = "Slovak", Id = "sk-sk" },
            new Language { Name = "Slovenian", Id = "sl-si" },
            new Language { Name = "Spanish", Id = "es-es" },
            new Language { Name = "Swedish", Id = "sv-se" },
            new Language { Name = "Thai", Id = "th-th" },
            new Language { Name = "Turkish", Id = "tr-tr" },
            new Language { Name = "Ukrainian", Id = "uk-ua" },
            new Language { Name = "Vietnamese", Id = "vi-vn" }
        };

        public class Language
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
