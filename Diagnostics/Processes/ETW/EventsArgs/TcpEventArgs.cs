
namespace Diagnostics.Processes.ETW.EventsArgs
{
    internal class TcpEventArgs
    {
        internal int ProcessId { get; private set; }
        internal int Size { get; private set; }

        internal TcpEventArgs(int processID, int size)
        {
            ProcessId = processID;
            Size = size;
        }
    }
}
