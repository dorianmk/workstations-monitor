using System;
using System.Collections.Generic;
using System.Management;

namespace Diagnostics.WMI
{
    internal class WMIObjectsProvider
    {
        internal List<ProcessWMI> GetProcessObjects()=> GetObjects("Win32_Process", o => new ProcessWMI(o));        
        internal List<ProcessorWMI> GetProcessorObjects() => GetObjects("Win32_Processor", o => new ProcessorWMI(o));      
        internal List<VideoControllerWMI> GetVideoControllerObjects() => GetObjects("Win32_VideoController", o => new VideoControllerWMI(o));
        internal List<BaseBoardWMI> GetBaseBoardObjects() => GetObjects("Win32_BaseBoard ", o => new BaseBoardWMI(o));
        internal List<PhysicalMemoryWMI> GetPhysicalMemoryObjects() => GetObjects("Win32_PhysicalMemory", o => new PhysicalMemoryWMI(o));
        internal List<ComputerSystemWMI> GetComputerSystemObjects() => GetObjects("Win32_ComputerSystem", o => new ComputerSystemWMI(o));

        private List<T> GetObjects<T>(string wmiClassName, Func<ManagementBaseObject, T> factory)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"Select * From {wmiClassName}");
            var result = new List<T>();
            foreach (var obj in searcher.Get())
            {
                var item = factory(obj);
                result.Add(item);
            }
            return result;
        }

    }
}
