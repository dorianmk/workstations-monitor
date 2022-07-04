using System.Management;

namespace Diagnostics.WMI
{
    internal class ProcessWMI
    {
        internal int ProcessId { get; }
        internal string ExecutablePath { get; }

        internal ProcessWMI(ManagementBaseObject wmiObject)
        {
            ProcessId = int.Parse(wmiObject["ProcessId"].ToString());
            ExecutablePath = wmiObject["ExecutablePath"]?.ToString();
        }
    }
}
