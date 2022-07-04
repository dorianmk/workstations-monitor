using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Events
{
    internal class EventRuleToDtoConverter : IFactory<EventRuleViewModel, EventRuleDTO>
    {
        public EventRuleDTO Create(EventRuleViewModel item)
        {
            return new EventRuleDTO()
            {
                EventSource = item.EventSource,
                ComparisonMode = item.ComparisonMode,
                Value = item.Value,
                EventType = item.EventType,
            };
        }
    }
}
