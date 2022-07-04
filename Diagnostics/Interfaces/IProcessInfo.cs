namespace Diagnostics.Interfaces
{
    public interface IProcessInfo
    {
        string Name { get; }
        double CPUPercent { get; }
        int MemoryMB { get; }
        int TcpReceivedKB { get; }
        int TcpSentKB { get; }
        int DiskIOReadKB { get; }
        int DiskIOWriteKB { get; }
        string ExecPath { get; }
    }
}