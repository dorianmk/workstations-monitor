using AsyncNet.Tcp.Remote;
using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using System;
using System.Collections.Generic;

namespace DataTransfer.Tcp
{
    internal class TcpTransferBase : IConnection
    {
        private IRemoteTcpPeer Peer { get; }
        private ISerializer Serializer { get; }

        public event EventHandler<Exception> Stopped;

        internal TcpTransferBase(IRemoteTcpPeer peer, ISerializer serializer)
        { 
            Serializer = serializer;            
            Peer = peer;
            Peer.ConnectionClosed += (s, e) => Stopped?.Invoke(this, e.ConnectionCloseException);
            Peer.FrameArrived += (s, e) =>
            {
                lastFrameBuffer.AddRange(e.FrameData);
                if (e.FrameData.Length != 4096)
                {
                    var data = lastFrameBuffer.ToArray();
                    lastFrameBuffer.Clear();
                    var readItems = Serializer.GetDatas(data);
                    foreach (var readItem in readItems)
                        OnRead?.Invoke(this, readItem);
                }
            };
        }

        private List<byte> lastFrameBuffer = new List<byte>();

        public event EventHandler<IData> OnRead;

        public bool Write(IData data)
        {
            var bytes = Serializer.GetBytes(data);
            return Peer.Post(bytes);
        }
    }
}
