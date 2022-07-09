using Common.DataTransfer.RequestResponse;
using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using NUnit.Framework;
using System;
using System.Threading;

namespace Common.Tests
{
    [TestFixture]
    class RequestResponseManagerTests
    {
        private IClientConnection Client { get; set; }
        private IServerConnections Server { get; set; }
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
        private RequestResponseManager RequestResponseManager { get; set; }
        private bool sendBack = true;

        [OneTimeSetUp]
        public void SetUp()
        {
            SetUp(new Endpoint(), new ServerSettings(), new JsonSerializer());
        }

        private void SetUp(IEndpoint endpoint, IServerSettings serverSettings, ISerializer serializer)
        {
            Client = new TcpClient(endpoint, serializer);
            Server = new TcpServer(serverSettings, serializer);
            var clientConnected = new ManualResetEvent(false);
            var serverOnNewClient = new ManualResetEvent(false);
            Client.Connected += (s, e) => { clientConnected.Set(); };
            Server.OnNewClient += (s, e) => { serverOnNewClient.Set(); };
            Client.Start();
            Server.Start();
            var connected = WaitHandle.WaitAll(new WaitHandle[] { clientConnected, serverOnNewClient }, timeout);
            Assert.IsTrue(connected);
            RequestResponseManager = new RequestResponseManager(Client.Server);

            Server.Clients[0].OnRead += (s, e) =>
            {
                if (e is IDataWithId dataWithId)
                {
                    if (sendBack)
                        Server.Clients[0].Write(dataWithId);
                }
                else
                    Assert.Fail("Read unknown packet");
            };
        }

        [Test]
        public void ThrowsExceptionsForEmptyGuid()
        {
            var testPacket = new TestPacket();
            var ae = Assert.Throws<ArgumentException>(() => RequestResponseManager.DoCallbackOnResponse(testPacket, x => x.ToString()));
            Assert.AreEqual("GUID not set", ae.Message);
        }

        [Test]
        public void ThrowsExceptionsForDuplicatedGuid()
        {
            sendBack = false;
            var testPacket = new TestPacket() { Id = Guid.NewGuid() };
            Assert.IsTrue(RequestResponseManager.DoCallbackOnResponse(testPacket, x => x.ToString()));
            var ae = Assert.Throws<ArgumentException>(() => RequestResponseManager.DoCallbackOnResponse(testPacket, x => x.ToString()));
            Assert.AreEqual("GUID already exists", ae.Message);
            sendBack = true;
        }

        [Test]
        public void DoCallbackOnResponse()
        {
            var id = Guid.NewGuid();
            var testPacket = new TestPacket() { Id = id };
            var callbackEvent = new ManualResetEvent(false);

            Assert.IsTrue(RequestResponseManager.DoCallbackOnResponse(testPacket, x =>
            {
                Assert.AreEqual(id, x.Id);
                callbackEvent.Set();
            }));
            var succeed = callbackEvent.WaitOne(timeout);
            Assert.IsTrue(succeed);
        }

        [Test]
        public void TryGetReponse()
        {
            var id = Guid.NewGuid();
            var testPacket = new TestPacket() { Id = id };
            if (RequestResponseManager.TryGetResponse<TestPacket>(testPacket, out var response, timeout))
                Assert.IsNotNull(response);
            else
                Assert.Fail();
        }

        [Test]
        public void TryGetReponseTimespanZero()
        {
            var id = Guid.NewGuid();
            var testPacket = new TestPacket() { Id = id };
            TestPacket response;
            var result = RequestResponseManager.TryGetResponse<TestPacket>(testPacket, out response, TimeSpan.Zero);
            Assert.IsFalse(result);
            Assert.IsNull(response);
        }

        [Test]
        public void TryGetReponseTimeout()
        {
            var id = Guid.NewGuid();
            var testPacket = new TestPacket() { Id = id };
            TestPacket response;
            sendBack = false;
            var result = RequestResponseManager.TryGetResponse<TestPacket>(testPacket, out response, timeout);
            Assert.IsFalse(result);
            Assert.IsNull(response);
            sendBack = true;
        }

    }

    internal class TestPacket : IDataWithId
    {
        public Guid Id { get; set; }
    }

    internal class ServerSettings : IServerSettings
    {
        public int Port => 60701;
    }

    internal class Endpoint : IEndpoint
    {
        public string Hostname => "localhost";

        public int Port => 60701;
    }
}
