using Common.DataTransfer.DataPackets.Workstation;
using Common.Enums;

namespace ServerService.Essential.AdminClient.Events.EventsOriginateRules
{
    internal class DiskEventOriginateRule : ProcessesEventOriginateRule
    {
        internal DiskEventOriginateRule(int value, ComparisonMode comparisonMode, EventType eventType)
                : base(value, comparisonMode, eventType, EventSource.Disk)
        {
        }

        protected override double GetValue(ProcessInfoDTO processInfo) => processInfo.DiskIOReadKB + processInfo.DiskIOWriteKB;       
        protected override string Unit => "KB";
    }
}
