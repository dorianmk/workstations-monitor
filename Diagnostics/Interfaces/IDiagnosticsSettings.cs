using System;

namespace Diagnostics.Interfaces
{
    public interface IDiagnosticsSettings
    {
        TimeSpan ProcessesInfoPeriod { get; }
    }
}
