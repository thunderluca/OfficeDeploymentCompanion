using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Models
{
    public static class Products
    {
        public static Dictionary<string, string> AvailableDictionary = new Dictionary<string, string>
        {
            { "Access", "Access" }, { "Excel", "Excel" },
            { "OneDrive for Business", "Groove" }, { "Skype for Business", "Lync" },
            { "OneDrive", "OneDrive" }, { "OneNote", "OneNote" },
            { "Outlook", "Outlook" }, { "PowerPoint", "PowerPoint" },
            { "Publisher", "Publisher" }, { "Word", "Work" }
        };
    }
}
