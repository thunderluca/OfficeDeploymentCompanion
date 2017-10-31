using System.Collections.Generic;
using System.Linq;

namespace OfficeDeploymentCompanion.Messages
{
    public class AddedLanguagesMessage
    {
        public AddedLanguagesMessage(IEnumerable<string> languagesIds)
        {
            this.LanguagesIds = languagesIds?.ToArray() ?? new string[0];
        }

        public string[] LanguagesIds { get; }
    }
}
