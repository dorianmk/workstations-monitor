using Diagnostics.Processes.ETW.EventsArgs;
using System;

namespace Diagnostics.Processes.ETW
{
    internal interface IETWEvents
    {
        event EventHandler<TcpEventArgs> TcpReceived;
        event EventHandler<TcpEventArgs> TcpSent;
        event EventHandler<DiskIOEventArgs> DiskIORead;
        event EventHandler<DiskIOEventArgs> DiskIOWrite;
    }
}
