using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Messages
{
    public class AddedLanguagesMessage
    {
        public AddedLanguagesMessage(IEnumerable<string> languagesIds)
        {
            this.LanguagesIds = languagesIds != null
                ? languagesIds.ToArray()
                : new string[0];
        }

        public string[] LanguagesIds { get; }
    }
}
