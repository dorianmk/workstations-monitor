using AsyncNet.Tcp.Client;
using DataTransfer.Interfaces;
using DataTransfer.Tcp.Serializers;
using System;
using System.Threading.Tasks;

namespace DataTransfer.Tcp
{
    public class TcpClient : IClientConnection
    {
        private IAsyncNetTcpClient Client { get; }

        public IDataTwoWay Server { get; private set; }
        public event EventHandler Connected;
        public event EventHandler<Exception> Stopped;
        public bool IsConnected { get; private set; }

        public TcpClient(IEndpoint endpoint, ISerializer serializer)
        {
            Client = new AsyncNetTcpClient(endpoint.Hostname, endpoint.Port);
            Client.ClientStopped += (s, e) =>
            {
                IsConnected = false;
                Stopped?.Invoke(this, e.Exception);
            };
            Client.ConnectionEstablished += (s, e) =>
            {
                Server = new TcpTransferBase(e.RemoteTcpPeer, serializer);               
                IsConnected = true;
                Connected?.Invoke(this, EventArgs.Empty);
            };            
        }        

        public Task Start() => Client.StartAsync();
    }
}
