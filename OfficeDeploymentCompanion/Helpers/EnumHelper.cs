using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Helpers
{
    public static class EnumHelper
    {
        public static T ToEnum<T>(this string enumString)
        {
            if (string.IsNullOrWhiteSpace(enumString))
                throw new ArgumentNullException(nameof(enumString));
            try
            {
                return (T)Enum.Parse(typeof(T), enumString);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static T[] GetEnumValuesArray<T>()
        {
            try
            {
                return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
