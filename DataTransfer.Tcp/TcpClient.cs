using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using PHS.Networking.Enums;
using System;
using System.Threading.Tasks;
using Tcp.NET.Client;
using Tcp.NET.Client.Events.Args;
using Tcp.NET.Client.Models;

namespace DataTransfer.Tcp
{
    public class TcpClient : IClientConnection
    {
        public event EventHandler Connected;
        public event EventHandler Stopped;

        private readonly ITcpNETClient _tcpNETClient;
        private readonly ISerializer _serializer;

        public TcpClient(IEndpoint endpoint, ISerializer serializer)
        {
            var paramsTcpClient = new ParamsTcpClient(
                endpoint.Hostname,
                endpoint.Port,
                endOfLineCharacters: TcpServer.EOL,
                isSSL: false,
                onlyEmitBytes: true);
            _tcpNETClient = new TcpNETClient(paramsTcpClient);
            _serializer = serializer;
            _tcpNETClient.ConnectionEvent += OnConnectionEvent;
        }

        public bool IsConnected { get; private set; }

        public IDataTwoWay Server { get; private set; }

        public async Task<bool> Start()
        {
            return await _tcpNETClient.ConnectAsync();
        }

        private void OnConnectionEvent(object sender, TcpConnectionClientEventArgs args)
        {
            if (args.ConnectionEventType == ConnectionEventType.Connected)
            {
                Server = new ServerConnection(_tcpNETClient, _serializer);
                IsConnected = true;
                Connected?.Invoke(this, EventArgs.Empty);
            }
            else if (args.ConnectionEventType == ConnectionEventType.Disconnect)
            {
                IsConnected = false;
                Stopped?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
