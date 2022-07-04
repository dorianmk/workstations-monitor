using Common.DataTransfer.DataPackets.Workstation;
using Database.Interfaces.Workstation;
using DataTransfer.Interfaces;

namespace ServerService.Essential.Workstation
{
    internal interface IWorkstationCache
    {
        IWorkstation Workstation { get; }
        bool IsConnected { get; }
        SystemInfoDTO SystemInfo { get; set; }
        void SetConnect(IConnection connection);
    }

    internal class WorkstationCache : IWorkstationCache
    {
        public IWorkstation Workstation { get; }
        public bool IsConnected { get; private set; }
        public SystemInfoDTO SystemInfo { get; set; }

        private IConnection Connection { get; set; }

        public WorkstationCache(IWorkstation workstation)
        {
            Workstation = workstation;
        }

        public void SetConnect(IConnection connection)
        {
            IsConnected = true;
            Connection = connection;
            Connection.Stopped += (s, e) => IsConnected = false;
            Connection.Write(new ConnectedPacket(Workstation.GetId(), Workstation.Name));
        }

    }
}
