using Diagnostics.Interfaces;
using System;

namespace WorkstationService.Entry.Settings
{
    internal class DiagnosticsSettings : IDiagnosticsSettings
    {
        public TimeSpan ProcessesInfoPeriod => TimeSpan.FromSeconds(2);
    }
}
