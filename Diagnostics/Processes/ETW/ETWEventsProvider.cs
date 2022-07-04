using Diagnostics.Processes.ETW.EventsArgs;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Threading.Tasks;

namespace Diagnostics.Processes.ETW
{
    internal class ETWEventsProvider : IETWEvents
    {
        private TraceEventSession etwSession;

        internal ETWEventsProvider()
        {
            Task.Run(() => StartEtwSession());
        }

        private void StartEtwSession()
        {
            using (etwSession = new TraceEventSession("MyKernelAndClrEventsSession"))
            {
                etwSession.EnableKernelProvider(KernelTraceEventParser.Keywords.NetworkTCPIP, KernelTraceEventParser.Keywords.DiskIO);
                etwSession.Source.Kernel.TcpIpRecv += data =>
                {
                    TcpReceived?.Invoke(this, new TcpEventArgs(data.ProcessID, data.size));
                };
                etwSession.Source.Kernel.TcpIpSend += data =>
                {
                    TcpSent?.Invoke(this, new TcpEventArgs(data.ProcessID, data.size));
                };
                etwSession.Source.Kernel.DiskIORead += data =>
                {
                    DiskIORead?.Invoke(this, new DiskIOEventArgs(data.ProcessID, data.TransferSize));
                };
                etwSession.Source.Kernel.DiskIOWrite += data =>
                {
                    DiskIOWrite?.Invoke(this, new DiskIOEventArgs(data.ProcessID, data.TransferSize));
                };
                etwSession.Source.Process();
            }
        }

        public event EventHandler<TcpEventArgs> TcpReceived;
        public event EventHandler<TcpEventArgs> TcpSent;
        public event EventHandler<DiskIOEventArgs> DiskIORead;
        public event EventHandler<DiskIOEventArgs> DiskIOWrite;
    }
}
