using Diagnostics.Processes.ETW.EventsArgs;
using System.Threading;

namespace Diagnostics.Processes.ETW
{
    internal class ETWCounters
    {
        public int TcpReceived => Interlocked.Exchange(ref tcpReceived, 0);
        public int TcpSent => Interlocked.Exchange(ref tcpSent, 0);
        public int DiskIORead => Interlocked.Exchange(ref diskIORead, 0);
        public int DiskIOWrite => Interlocked.Exchange(ref diskIOWrite, 0);

        private int ProcessId { get; }
        private int tcpReceived;
        private int tcpSent;
        private int diskIORead;
        private int diskIOWrite;

        internal ETWCounters(int processId, IETWEvents eTWEvents)
        {
            ProcessId = processId;
            eTWEvents.TcpReceived += ETWEvents_TcpRecived;
            eTWEvents.TcpSent += ETWEvents_TcpSent;
            eTWEvents.DiskIORead += ETWEvents_DiskIORead;
            eTWEvents.DiskIOWrite += ETWEvents_DiskIOWrite;
        }

        private void ETWEvents_TcpRecived(object sender, TcpEventArgs e)
        {
            if (e.ProcessId == ProcessId)
                Interlocked.Add(ref tcpReceived, e.Size);
        }

        private void ETWEvents_TcpSent(object sender, TcpEventArgs e)
        {
            if (e.ProcessId == ProcessId)
                Interlocked.Add(ref tcpSent, e.Size);
        }

        private void ETWEvents_DiskIORead(object sender, DiskIOEventArgs e)
        {
            if (e.ProcessId == ProcessId)
                Interlocked.Add(ref diskIORead, e.TransferSize);
        }

        private void ETWEvents_DiskIOWrite(object sender, DiskIOEventArgs e)
        {
            if (e.ProcessId == ProcessId)
                Interlocked.Add(ref diskIOWrite, e.TransferSize);
        }
    }
}
