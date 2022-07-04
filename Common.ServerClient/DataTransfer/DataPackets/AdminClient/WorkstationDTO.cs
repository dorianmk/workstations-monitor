using Common.DataTransfer.DataPackets.Workstation;
using DataTransfer.Interfaces;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class WorkstationDTO : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsConnected { get; set; }
        public SystemInfoDTO SystemInfo { get; set; }
    }
}
