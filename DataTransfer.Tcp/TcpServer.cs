using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using PHS.Networking.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tcp.NET.Server;
using Tcp.NET.Server.Events.Args;
using Tcp.NET.Server.Models;

namespace DataTransfer.Tcp
{
    public class TcpServer : IServerConnections
    {
        internal static byte[] TempBytes { get; } = new byte[] { 0, 1, 2, 3 };//todo temporary workaround for https://github.com/LiveOrDevTrying/Tcp.NET/issues/3
        internal static string EOL { get; } = "\r\n";

        public event EventHandler<IConnection> OnNewClient;

        private readonly ITcpNETServer _tcpNETServer;
        private readonly List<IConnection> _clients;
        private readonly ISerializer _serializer;

        public TcpServer(IServerSettings settings, ISerializer serializer)
        {
            var paramsTcpServer = new ParamsTcpServer(
               settings.Port,
               endOfLineCharacters: EOL,
               onlyEmitBytes: true);
            _tcpNETServer = new TcpNETServer(paramsTcpServer);
            _clients = new();
            _tcpNETServer.ConnectionEvent += OnConnectionEvent;
            _serializer = serializer;
        }

        public IReadOnlyList<IConnection> Clients => _clients;

        public Task Start()
        {
            _tcpNETServer.Start();
            return Task.CompletedTask;
        }

        private void OnConnectionEvent(object sender, TcpConnectionServerEventArgs args)
        {
            if (args.ConnectionEventType == ConnectionEventType.Connected)
            {
                IConnection clientConnection = new ClientConnection(_tcpNETServer, args.Connection, _serializer);
                _clients.Add(clientConnection);
                OnNewClient?.Invoke(this, clientConnection);
            }
        }

    }
}
