using System.Configuration;
using DataTransfer.Tcp;

namespace ServerService.Entry.Settings
{
    internal class TcpServerSettings : IServerSettings
    {
        public int Port { get; }

        public TcpServerSettings()
        {
            Port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());
        }
    }
}
