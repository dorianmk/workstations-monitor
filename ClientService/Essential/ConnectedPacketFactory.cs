using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Diagnostics.Interfaces;
using WorkstationService.Essential.Id;

namespace WorkstationService.Essential
{
    internal class ConnectedPacketFactory : IFactory<ConnectedPacket>
    {
        private IWorkstationId WorkstationId { get; }
        private ISystemInfoProvider SystemInfoProvider { get; }

        public ConnectedPacketFactory(IWorkstationId workstationId, ISystemInfoProvider systemInfoProvider)
        {
            WorkstationId = workstationId;
            SystemInfoProvider = systemInfoProvider;
        }

        public ConnectedPacket Create()
        {
            return new ConnectedPacket(WorkstationId.Get(), SystemInfoProvider.ComputerName);
        }
    }
}
