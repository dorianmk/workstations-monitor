using System.Management;

namespace Diagnostics.WMI
{

    internal class PhysicalMemoryWMI
    {
        internal ulong Capacity { get; }
        internal uint MHz { get; }

        internal PhysicalMemoryWMI(ManagementBaseObject wmiObject)
        {
            Capacity = ulong.Parse(wmiObject["Capacity"].ToString());
            MHz = uint.Parse(wmiObject["ConfiguredClockSpeed"].ToString());
        }
    }
}
