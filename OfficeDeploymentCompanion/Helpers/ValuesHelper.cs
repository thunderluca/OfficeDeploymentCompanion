using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Helpers
{
    public static class ValuesHelper
    {
        public static bool GetValueFromBit(this string value) => !string.IsNullOrWhiteSpace(value) ? value == "1" : false;

        public static bool GetValueFromBoolean(this string value) => !string.IsNullOrWhiteSpace(value) ? Boolean.Parse(value) : false;

        public static string GetBitStringFromBoolean(this bool value) => (value ? 1 : 0).ToString();

        public static string GetBooleanStringFromBoolean(this bool value) => value.ToString().ToUpper();
    }
}
