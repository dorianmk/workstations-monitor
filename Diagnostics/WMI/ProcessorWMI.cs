using System.Management;

namespace Diagnostics.WMI
{
    internal class ProcessorWMI
    {
        internal string Name { get; }
        internal int NumberOfCores { get; }

        internal ProcessorWMI(ManagementBaseObject wmiObject)
        {
            Name = wmiObject["Name"].ToString();
            NumberOfCores = int.Parse(wmiObject["NumberOfCores"].ToString());
        }
    } 
}
