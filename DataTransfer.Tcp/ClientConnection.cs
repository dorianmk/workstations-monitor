using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using PHS.Networking.Enums;
using System;
using System.Linq;
using Tcp.NET.Server;
using Tcp.NET.Server.Events.Args;
using Tcp.NET.Server.Models;

namespace DataTransfer.Tcp
{
    internal class ClientConnection : IConnection
    {
        public event EventHandler Stopped;
        public event EventHandler<IData> OnRead;

        private readonly ITcpNETServer _tcpNETServer;
        private readonly ConnectionTcpServer _connectionTcpServer;
        private readonly ISerializer _serializer;

        internal ClientConnection(
            ITcpNETServer tcpNETServer,
            ConnectionTcpServer connectionTcpServer,
            ISerializer serializer)
        {
            _tcpNETServer = tcpNETServer;
            _connectionTcpServer = connectionTcpServer;
            _serializer = serializer;
            _tcpNETServer.MessageEvent += OnMessageEvent;
        }

        private void OnMessageEvent(object sender, TcpMessageServerEventArgs args)
        {
            if (args.MessageEventType == MessageEventType.Receive
                && args.Connection.ConnectionId == _connectionTcpServer.ConnectionId)
            {
                if (args.Bytes.SequenceEqual(TcpServer.TempBytes))
                    return;

                var readItem = _serializer.GetData(args.Bytes);
                OnRead?.Invoke(this, readItem);
            }
        }

        public bool Write(IData data)
        {
            var bytes = _serializer.GetBytes(data);
            var result = _tcpNETServer.SendToConnectionAsync(bytes, _connectionTcpServer).Result;
            _tcpNETServer.SendToConnectionAsync(TcpServer.TempBytes, _connectionTcpServer);
            return result;
        }
    }
}
