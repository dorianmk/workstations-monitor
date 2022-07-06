using DataTransfer.Tcp;
using Microsoft.Extensions.Configuration;

namespace AdminClientApp.Entry.Settings
{
    internal class Endpoint : IEndpoint
    {
        public string Hostname { get; }

        public int Port { get; }

        public Endpoint(IConfiguration configuration)
        {
            Hostname = configuration["hostname"].ToString();
            Port = int.Parse(configuration["port"].ToString());
        }
    }
}
