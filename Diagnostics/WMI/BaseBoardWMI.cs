using System.Management;

namespace Diagnostics.WMI
{
    internal class BaseBoardWMI
    {
        internal string Name { get; }
        internal string Manufacturer { get; }

        internal BaseBoardWMI(ManagementBaseObject wmiObject)
        {
            Name = wmiObject["Product"].ToString();
            Manufacturer = wmiObject["Manufacturer"].ToString();
        }
    }
}
