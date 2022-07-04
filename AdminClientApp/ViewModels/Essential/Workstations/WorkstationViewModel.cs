using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Workstations.Performance;
using AdminClientApp.ViewModels.Essential.Workstations.Processes;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using DataTransfer.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Workstations
{
    public class WorkstationViewModel : BindableBase
    {
        public string Id { get; private set; }

        private string name;
        public string Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            private set => SetProperty(ref isConnected, value);
        }

        public ProcessesViewModel ProcessesViewModel { get; }
        public PerformanceViewModel PerformanceViewModel { get; }
        public SystemInfoDTO SystemInfo { get; private set; }

        public WorkstationViewModel(IClientConnection connection, IUpserter<ProcessInfoDTO, ProcessInfoViewModel> processInfoViewModelUpserter)
        {
            connection.Server.OnRead += Server_OnRead;
            ProcessesViewModel = new ProcessesViewModel(processInfoViewModelUpserter);
            PerformanceViewModel = new PerformanceViewModel();
        }

        private void Server_OnRead(object sender, IData data)
        {
            if (data is ProcessesInfoPacket processesInfoPacket && processesInfoPacket.WorkstationId.Equals(Id))
            {
                ProcessesViewModel.Update(processesInfoPacket);
                PerformanceViewModel.Update(processesInfoPacket);
            }
        }

        public override string ToString() => Name;

    }
}
