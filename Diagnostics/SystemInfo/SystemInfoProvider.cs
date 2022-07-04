using Diagnostics.Interfaces;
using Diagnostics.WMI;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace Diagnostics.SystemInfo
{
    internal class SystemInfoProvider : ISystemInfoProvider
    {
        public string ComputerName { get; }
        public IEnumerable<string> Applications { get; }
        public IHardware Hardware { get; }
        public INetwork Network { get; }

        internal SystemInfoProvider()
        {
            var wMIObjectsProvider = new WMIObjectsProvider();
            ComputerName = wMIObjectsProvider.GetComputerSystemObjects().First().Name;
            Applications = GetApplications().OrderBy(x => x).Distinct().ToList();
            Hardware = new Hardware();
            Network = new Network();
        }

        private IEnumerable<string> GetApplications()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                    {
                        var displayName = subkey.GetValue("DisplayName")?.ToString();
                        if (!string.IsNullOrEmpty(displayName))
                            yield return displayName;
                    }
                }
            }
        }
    }
}
