using System.Collections.Generic;
using System.Linq;
using DataTransfer.Tcp.Serializers;
using NUnit.Framework;

namespace DataTransfer.Tests.Tcp
{
    [TestFixture]
    class TcpTestsJson : TcpTestsBase
    {

        [OneTimeSetUp]
        public void SetUp()
        {
            SetUp(new Endpoint(), new ServerSettings(), new JsonSerializer());
        }

        [Test]
        public void ServerGotPacketTest()
        {
            GotPacketsTest(Client.Server, Server.Clients[0], GetTestPackets(20));
        }

        [Test]
        public void ClientGotPacketTest()
        {
            GotPacketsTest(Server.Clients[0], Client.Server, GetTestPackets(20));
        }

        private List<TestPacket> GetTestPackets(int count)
        {
            var result = new List<TestPacket>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(new TestPacket("test", i, true));
            }
            return result;
        }

        [Test]
        public void SerializerTest()
        {
            var data = GetTestPackets(1).First();
            var serializer = new JsonSerializer();
            var bytes = serializer.GetBytes(data);
            Assert.IsNotNull(bytes);
            Assert.IsNotEmpty(bytes);
            var deserialized = serializer.GetData(bytes);
            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOf<TestPacket>(deserialized);
            Assert.AreEqual(data, deserialized);
        }

    }

}
