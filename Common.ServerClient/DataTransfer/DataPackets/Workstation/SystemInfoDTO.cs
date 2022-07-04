using DataTransfer.Interfaces;
using System.Collections.Generic;

namespace Common.DataTransfer.DataPackets.Workstation
{
    public class SystemInfoDTO : IData
    {
        public string ComputerName { get; set; }
        public IEnumerable<string> Applications { get; set; }
        public string HardwareProcessor { get; set; }
        public string HardwareMotherboard { get; set; }
        public string HardwareRAM { get; set; }
        public string HardwareGPU1Name { get; set; }
        public uint HardwareGPU1MemoryMB { get; set; }
        public bool HardwareGPU1IsActive { get; set; }
        public string HardwareGPU2Name { get; set; }
        public uint HardwareGPU2MemoryMB { get; set; }
        public bool HardwareGPU2IsActive { get; set; }
        public List<string> NetworkIpAdresses { get; set; }
    }
}
