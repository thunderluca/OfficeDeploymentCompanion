using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.Models
{
    public static class CpuArchitectureExtensions
    {
        public static int GetOSArchitecture(this CpuArchitecture cpuArchitecture)
        {
            switch (cpuArchitecture)
            {
                //case CpuArchitecture.ARM:
                case CpuArchitecture.X86:
                    return 32;
                //case CpuArchitecture.ARM64:
                case CpuArchitecture.X64:
                    return 64;
                default:
                    throw new NotSupportedException($"Not supported CPU Architecture: {cpuArchitecture}");
            }
        }
    }
}
