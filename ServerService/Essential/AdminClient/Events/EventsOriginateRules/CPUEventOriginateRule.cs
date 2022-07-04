using Common.DataTransfer.DataPackets.Workstation;
using Common.Enums;

namespace ServerService.Essential.AdminClient.Events.EventsOriginateRules
{
    internal class CPUEventOriginateRule : ProcessesEventOriginateRule
    {
        internal CPUEventOriginateRule(int value, ComparisonMode comparisonMode, EventType eventType)
            : base(value, comparisonMode, eventType, EventSource.CPU)
        {
        }

        protected override double GetValue(ProcessInfoDTO processInfo) => processInfo.CPUPercent;
        protected override string Unit => "%";

    }
}
