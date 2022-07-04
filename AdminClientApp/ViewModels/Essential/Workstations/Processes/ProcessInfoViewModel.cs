using AdminClientApp.ViewModels.Common;

namespace AdminClientApp.ViewModels.Essential.Workstations.Processes
{
    public class ProcessInfoViewModel : BindableBase
    {
        public string Name { get; set; }
        public double CPUPercent { get; set; }
        public int MemoryMB { get; set; }
        public int TcpReceivedKB { get; set; }
        public int TcpSentKB { get; set; }
        public int DiskIOReadKB { get; set; }
        public int DiskIOWriteKB { get; set; }
        public string ExecPath { get; set; }

    }
}
