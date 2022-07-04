using Diagnostics.ExtensionMethods;
using Diagnostics.Interfaces;
using Diagnostics.Processes.ETW;
using Diagnostics.WMI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Diagnostics.Processes
{
    internal class ProcessInfo : IProcessInfo
    {
        private int ProcessorCores { get; }
        private Dictionary<int, ProcessUsagesProvider> ProcessesUsages { get; }
        private IETWEvents ETWEvents { get; }
        public string Name { get; }
        public double CPUPercent { get; private set; }
        public int MemoryMB { get; private set; }
        public int TcpReceivedKB { get; private set; }
        public int TcpSentKB { get; private set; }
        public int DiskIOReadKB { get; private set; }
        public int DiskIOWriteKB { get; private set; }
        public string ExecPath { get; }

        internal ProcessInfo(string name, List<Process> processes, Dictionary<int, ProcessWMI> wmiObjects, IETWEvents eTWEvents, int processorCores)
        {
            Name = name;
            ETWEvents = eTWEvents;
            ProcessorCores = processorCores;
            ProcessesUsages = processes.ToDictionary(x => x.Id, y => new ProcessUsagesProvider(y, ETWEvents, ProcessorCores));
            ExecPath = GetExecPath(processes, wmiObjects);
        }

        internal void Update(List<Process> processes)
        {
            ProcessesUsages.Update(processes, x => x.Id, y => new ProcessUsagesProvider(y, ETWEvents, ProcessorCores), (x, y) => { });
        }

        internal void CalculateUsages()
        {
            CPUPercent = Math.Round(ProcessesUsages.Values.Sum(x => x.GetCPU()), 2);
            MemoryMB = (int)ProcessesUsages.Values.Sum(x => x.GetMemory());
            TcpReceivedKB = ProcessesUsages.Values.Sum(x => x.GetTcpReceived()) / 1024;
            TcpSentKB = ProcessesUsages.Values.Sum(x => x.GetTcpSent()) / 1024;
            DiskIOReadKB = ProcessesUsages.Values.Sum(x => x.GetDiskIORead()) / 1024;
            DiskIOWriteKB = ProcessesUsages.Values.Sum(x => x.GetDiskIOWrite()) / 1024;
        }

        private string GetExecPath(List<Process> processes, Dictionary<int, ProcessWMI> wmiObjects)
        {
            foreach (var id in processes.Select(x => x.Id))
            {
                if (wmiObjects.TryGetValue(id, out var wmiObject))
                {
                    var execPath = wmiObject.ExecutablePath;
                    if (!string.IsNullOrEmpty(execPath))
                        return execPath;
                }
            }
            return string.Empty;
        }

        public override string ToString() => $"{Name} CPU: {CPUPercent} Memory: {MemoryMB}";

    }
}
