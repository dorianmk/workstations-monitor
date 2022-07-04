using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using Database.Interfaces.EventRule;

namespace ServerService.Essential.AdminClient.Events
{
    internal class EventRuleDtoToEntityConverter : IFactory<EventRuleDTO, IEventRule>
    {
        private IEventRules EventRules { get; }

        public EventRuleDtoToEntityConverter(IEventRules eventRules)
        {
            EventRules = eventRules;
        }

        public IEventRule Create(EventRuleDTO dto)
        {
            return EventRules.CreateEventRule(dto.EventSource.ToString(), dto.ComparisonMode.ToString(), dto.Value, dto.EventType.ToString());
        }
    }
}
