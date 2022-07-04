using Diagnostics.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Diagnostics.SystemInfo
{
    internal class Network : INetwork
    {
        public List<string> IpAdresses { get; }

        internal Network()
        {
            IpAdresses = GetLocalIPv4AddressList();
        }

        private List<string> GetLocalIPv4AddressList()
        {
            var result = new List<string>();
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces().Where(x => x.OperationalStatus == OperationalStatus.Up && x.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                var ips = nic.GetIPProperties().UnicastAddresses
                        .Select(uni => uni.Address)
                        .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                result.AddRange(ips.Select(x => x.ToString()));
            }

            return result;
        }

    }
}
