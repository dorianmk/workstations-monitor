using Diagnostics.Interfaces;
using Diagnostics.Processes;
using Diagnostics.SystemInfo;

namespace Diagnostics.Instances
{
    public class Diagnostics : IDiagnostics
    {
        public IProcessesInfoProvider ProcessesInfoProvider { get; }
        public ISystemInfoProvider SystemInfoProvider { get; }

        public Diagnostics(IDiagnosticsSettings diagnosticsSettings)
        {
            ProcessesInfoProvider = new ProcessesInfoProvider(diagnosticsSettings.ProcessesInfoPeriod);
            SystemInfoProvider = new SystemInfoProvider();
        }        
    }
}
