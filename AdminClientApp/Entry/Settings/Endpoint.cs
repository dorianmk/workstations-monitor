using System.Configuration;
using DataTransfer.Tcp;

namespace AdminClientApp.Entry.Settings
{
    internal class Endpoint : IEndpoint
    {
        public string Hostname { get; }

        public int Port { get; }

        public Endpoint()
        {
            Hostname = ConfigurationManager.AppSettings["hostname"].ToString();
            Port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());
        }
    }
}
