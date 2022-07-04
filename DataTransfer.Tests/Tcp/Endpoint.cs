using DataTransfer.Tcp;

namespace DataTransfer.Tests.Tcp
{
    internal class Endpoint : IEndpoint
    {
        public string Hostname => "localhost";

        public int Port => 55501;
    }
}
