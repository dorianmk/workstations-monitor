using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataTransfer.Interfaces
{
    public interface IServerConnections
    {
        IReadOnlyList<IConnection> Clients { get; }
        event EventHandler<IConnection> OnNewClient;
        Task Start();
    }
}
