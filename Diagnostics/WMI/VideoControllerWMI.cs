using System.Management;

namespace Diagnostics.WMI
{
    internal class VideoControllerWMI
    {
        internal string Name { get; }
        internal uint MemoryMB { get; }
        internal bool IsActive { get; }

        internal VideoControllerWMI(ManagementBaseObject wmiObject)
        {
            Name = wmiObject["Name"].ToString();
            MemoryMB = uint.Parse(wmiObject["AdapterRAM"].ToString()) / 1048576;
            IsActive = int.Parse(wmiObject["Availability"].ToString()) == 3;
        }
    }
}
