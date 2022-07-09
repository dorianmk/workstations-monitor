using DataTransfer.Tcp;
using Microsoft.Extensions.Configuration;

namespace ServerService.Entry.Settings
{
    internal class TcpServerSettings : IServerSettings
    {
        public int Port { get; }

        public TcpServerSettings(IConfiguration configuration)
        {
            Port = int.Parse(configuration["port"].ToString());
        }
    }
}
