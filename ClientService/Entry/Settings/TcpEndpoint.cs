using DataTransfer.Tcp;

namespace WorkstationService.Entry.Settings
{
    internal class TcpEndpoint : IEndpoint
    {
        private readonly AppSettings _appSettings;

        public TcpEndpoint(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string Hostname => _appSettings.Hostname;

        public int Port => _appSettings.Port;
    }
}
