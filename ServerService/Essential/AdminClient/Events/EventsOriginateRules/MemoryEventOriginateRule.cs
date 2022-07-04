using Common.DataTransfer.DataPackets.Workstation;
using Common.Enums;

namespace ServerService.Essential.AdminClient.Events.EventsOriginateRules
{
    internal class MemoryEventOriginateRule : ProcessesEventOriginateRule
    {
        internal MemoryEventOriginateRule(int value, ComparisonMode comparisonMode, EventType eventType)
              : base(value, comparisonMode, eventType, EventSource.Memory)
        {
        }

        protected override double GetValue(ProcessInfoDTO processInfo) => processInfo.MemoryMB;
        protected override string Unit => "MB";
    }
}
