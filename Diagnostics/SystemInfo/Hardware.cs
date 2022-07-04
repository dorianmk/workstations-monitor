using Diagnostics.Interfaces;
using Diagnostics.WMI;
using System.Linq;

namespace Diagnostics.SystemInfo
{
    internal class Hardware : IHardware
    {
        public string Processor { get; }
        public string Motherboard { get; }
        public string RAM { get; }
        public IGPUInfo GPU1 { get; }
        public IGPUInfo GPU2 { get; }

        internal Hardware()
        {
            var wMIObjectsProvider = new WMIObjectsProvider();
            Processor = wMIObjectsProvider.GetProcessorObjects().First().Name;
            var gpus = wMIObjectsProvider.GetVideoControllerObjects().Select(x => new GPUInfo(x)).ToList();
            GPU1 = gpus.First();
            if (gpus.Count > 1)
                GPU2 = gpus[1];
            var baseBoard = wMIObjectsProvider.GetBaseBoardObjects().First();
            Motherboard = $"{baseBoard.Manufacturer} {baseBoard.Name}";
            var physicalMemory = wMIObjectsProvider.GetPhysicalMemoryObjects().First();
            RAM = $"{physicalMemory.Capacity/ 1073741824}GB {physicalMemory.MHz}MHz";
        }

    }
}
