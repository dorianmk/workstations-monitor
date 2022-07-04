using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using ServerService.Essential.AdminClient.Events.EventsOriginateRules;

namespace ServerService.Essential.AdminClient.Events
{
    internal class EventRuleToDtoConverter : IFactory<ProcessesEventOriginateRule, EventRuleDTO>
    {
        public EventRuleDTO Create(ProcessesEventOriginateRule param)
        {
            return new EventRuleDTO()
            {
                EventSource = param.EventSource,
                ComparisonMode = param.ComparisonMode,
                Value = param.Value,
                EventType = param.EventType
            };
        }
    }
}
