using Common.DataTransfer.DataPackets.Workstation;
using Common.Enums;

namespace ServerService.Essential.AdminClient.Events.EventsOriginateRules
{
    internal class TCPEventOriginateRule : ProcessesEventOriginateRule
    {
        internal TCPEventOriginateRule(int value, ComparisonMode comparisonMode, EventType eventType)
              : base(value, comparisonMode, eventType, EventSource.TCP)
        {
        }

        protected override double GetValue(ProcessInfoDTO processInfo) => processInfo.TcpReceivedKB + processInfo.TcpSentKB;       
        protected override string Unit => "KB";
    }
}
