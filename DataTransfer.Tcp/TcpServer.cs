using AsyncNet.Tcp.Server;
using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataTransfer.Tcp
{
    public class TcpServer : IServerConnections
    {
        private IAsyncNetTcpServer Server { get; }
        private List<IConnection> ClientsList { get; }

        public IReadOnlyList<IConnection> Clients => ClientsList;
        public event EventHandler<IConnection> OnNewClient;

        public TcpServer(IServerSettings settings, ISerializer serializer)
        {
            Server = new AsyncNetTcpServer(settings.Port);
            ClientsList = new List<IConnection>();
            Server.ConnectionEstablished += (s, e) =>
            {
                var client = new TcpTransferBase(e.RemoteTcpPeer, serializer);
                ClientsList.Add(client);
                OnNewClient?.Invoke(this, client);
            };

        }

        public Task Start() => Server.StartAsync();

    }
}
