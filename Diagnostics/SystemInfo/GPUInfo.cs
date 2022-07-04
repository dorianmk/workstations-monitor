using Diagnostics.Interfaces;
using Diagnostics.WMI;

namespace Diagnostics.SystemInfo
{
    internal class GPUInfo : IGPUInfo
    {
        public string Name { get; }

        public uint MemoryMB { get; }

        public bool IsActive { get; }

        internal GPUInfo(VideoControllerWMI videoControllerWMI)
        {
            Name = videoControllerWMI.Name;
            MemoryMB = videoControllerWMI.MemoryMB;
            IsActive = videoControllerWMI.IsActive;
        }
    }
}
