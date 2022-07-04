using System.Management;

namespace Diagnostics.WMI
{
    internal class ComputerSystemWMI
    {
        internal string Name { get; }

        internal ComputerSystemWMI(ManagementBaseObject wmiObject)
        {
            Name = wmiObject["Name"].ToString();
        }
    }
}
