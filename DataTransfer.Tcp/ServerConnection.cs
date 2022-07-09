using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using PHS.Networking.Enums;
using System;
using System.Linq;
using Tcp.NET.Client;
using Tcp.NET.Client.Events.Args;

namespace DataTransfer.Tcp
{
    internal class ServerConnection : IConnection
    {
        public event EventHandler Stopped;
        public event EventHandler<IData> OnRead;

        private readonly ITcpNETClient _tcpNETClient;
        private readonly ISerializer _serializer;

        internal ServerConnection(ITcpNETClient tcpNETClient, ISerializer serializer)
        {
            _tcpNETClient = tcpNETClient;
            _serializer = serializer;
            _tcpNETClient.ConnectionEvent += OnConnectionEvent;
            _tcpNETClient.MessageEvent += OnMessageEvent;
        }

        private void OnMessageEvent(object sender, TcpMessageClientEventArgs args)
        {
            if (args.MessageEventType == MessageEventType.Receive)
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
            var result = _tcpNETClient.SendAsync(bytes).Result;
            _tcpNETClient.SendAsync(TcpServer.TempBytes);
            return result;
        }

        private void OnConnectionEvent(object sender, TcpConnectionClientEventArgs args)
        {
            if (args.ConnectionEventType == ConnectionEventType.Disconnect)
                Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
