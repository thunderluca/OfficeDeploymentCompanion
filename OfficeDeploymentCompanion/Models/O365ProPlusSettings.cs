using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
