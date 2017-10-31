using System.Collections.Generic;

namespace OfficeDeploymentCompanion.Models
{
    public class O365ProPlusSettings
    {
        public O365ProPlusSettings()
        {
            Languages = new List<string>();
            ExcludedApps = new List<string>();
        }

        public List<string> Languages { get; set; }

        public List<string> ExcludedApps { get; set; }
    }
}
