
namespace Diagnostics.Processes.ETW.EventsArgs
{
    internal class DiskIOEventArgs
    {
        internal int ProcessId { get; private set; }
        internal int TransferSize { get; private set; }

        internal DiskIOEventArgs(int processID, int transferSize)
        {
            ProcessId = processID;
            TransferSize = transferSize;
        }
    }
}
