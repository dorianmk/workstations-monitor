using DataTransfer.Interfaces;

namespace Common.DataTransfer.DataPackets.Workstation
{
    public class ConnectedPacket : IData
    {
        public string WorkstationId { get; private set; }
        public string ComputerName { get; private set; }

        public ConnectedPacket(string workstationId, string computerName)
        {
            WorkstationId = workstationId;
            ComputerName = computerName;
        }

    }
}
