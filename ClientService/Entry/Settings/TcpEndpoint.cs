using System.Configuration;
using DataTransfer.Tcp;

namespace WorkstationService.Entry.Settings
{
    internal class TcpEndpoint : IEndpoint
    {
        public string Hostname { get; }

        public int Port { get; }

        public TcpEndpoint()
        {
            Hostname = ConfigurationManager.AppSettings["hostname"].ToString();
            Port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());
        }
    }
}
