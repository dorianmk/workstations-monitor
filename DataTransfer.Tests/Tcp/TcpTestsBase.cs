using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataTransfer.Tests.Tcp
{
    abstract class TcpTestsBase
    {
        protected IClientConnection Client { get; private set; }
        protected IServerConnections Server { get; private set; }

        protected void SetUp(IEndpoint endpoint, IServerSettings serverSettings, ISerializer serializer)
        {
            Client = new TcpClient(endpoint, serializer);
            Server = new TcpServer(serverSettings, serializer);
            var clientConnected = new ManualResetEvent(false);
            var serverOnNewClient = new ManualResetEvent(false);
            Client.Connected += (s, e) => { clientConnected.Set(); };
            Server.OnNewClient += (s, e) => { serverOnNewClient.Set(); };
            Client.Start();
            Server.Start();
            var connected = WaitHandle.WaitAll(new WaitHandle[] { clientConnected, serverOnNewClient }, TimeSpan.FromSeconds(5));
            Assert.IsTrue(connected);
        }

        protected void GotPacketsTest<T>(IDataWriter source, IDataReader target, List<T> datas)
            where T : IData
        {
            List<T> packets = new List<T>();
            var waitHandles = new ManualResetEvent[datas.Count];
            for (int i = 0; i < datas.Count; i++)
                waitHandles[i] = new ManualResetEvent(false);

            target.OnRead += (s, e) =>
            {
                packets.Add((T)e);
                waitHandles[packets.Count - 1].Set();
            };

            foreach (var data in datas)
                Assert.IsTrue(source.Write(data));

            var didReceiveAll = WaitHandle.WaitAll(waitHandles, TimeSpan.FromSeconds(1));
            Assert.IsTrue(didReceiveAll, $"Received {packets.Count}/{datas.Count} packets");          
            Assert.IsTrue(packets.SequenceEqual(datas));
        }

    }
}
