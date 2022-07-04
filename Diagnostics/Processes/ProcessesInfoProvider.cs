using Diagnostics.ExtensionMethods;
using Diagnostics.Interfaces;
using Diagnostics.Processes.ETW;
using Diagnostics.WMI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Diagnostics.Processes
{
    internal class ProcessesInfoProvider : IProcessesInfoProvider
    {
        public event EventHandler<IEnumerable<IProcessInfo>> Actual;
        private Dictionary<string, ProcessInfo> Cache { get; }
        private TimeSpan Period { get; }
        private IETWEvents ETWEventsProvider { get; }
        private WMIObjectsProvider WMIObjectsProvider { get; }
        private int ProcessorCores { get; }

        internal ProcessesInfoProvider(TimeSpan processInfoPeriod)
        {
            Cache = new Dictionary<string, ProcessInfo>();
            Period = processInfoPeriod;
            ETWEventsProvider = new ETWEventsProvider();
            WMIObjectsProvider = new WMIObjectsProvider();
            ProcessorCores = WMIObjectsProvider.GetProcessorObjects().First().NumberOfCores;
            GetActual();
        }

        private void GetActual()
        {
            var currentProcesses = Process.GetProcesses();
            var wmiObjects = WMIObjectsProvider.GetProcessObjects().ToDictionary(x => x.ProcessId, y => y);
            var processesGroups = currentProcesses.GroupBy(x => x.ProcessName).ToList();
            Cache.Update(
                processesGroups,
                x => x.Key,
                y => new ProcessInfo(y.Key, y.ToList(), wmiObjects, ETWEventsProvider, ProcessorCores), 
                (x, y) => x.Update(y.ToList()));
            foreach (var item in Cache.Values)
                item.CalculateUsages();
            Actual?.Invoke(this, Cache.Values);
            Task.Delay(Period).ContinueWith(t => GetActual());
        }
    }
}