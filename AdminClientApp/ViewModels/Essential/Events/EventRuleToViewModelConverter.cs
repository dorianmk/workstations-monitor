using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Events
{
    internal class EventRuleToViewModelConverter : IFactory<EventRuleDTO, EventRuleViewModel>
    {
        public EventRuleViewModel Create(EventRuleDTO param)
        {
            return new EventRuleViewModel()
            {
                EventSource = param.EventSource,
                ComparisonMode = param.ComparisonMode,
                Value = param.Value,
                EventType = param.EventType
            };
        }
    }
}
