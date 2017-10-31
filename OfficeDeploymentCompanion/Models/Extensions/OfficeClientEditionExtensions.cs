using System;

namespace OfficeDeploymentCompanion.Models
{
    public static class OfficeClientEditionExtensions
    {
        public static int GetOSArchitecture(this OfficeClientEdition edition)
        {
            switch (edition)
            {
                //case CpuArchitecture.ARM:
                case OfficeClientEdition.X86:
                    return 32;
                //case CpuArchitecture.ARM64:
                case OfficeClientEdition.X64:
                    return 64;
                default:
                    throw new NotSupportedException($"Not supported OfficeClientEdition: {edition}");
            }
        }
    }
}
