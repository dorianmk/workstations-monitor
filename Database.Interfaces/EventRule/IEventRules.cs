using Database.Interfaces.Common;

namespace Database.Interfaces.EventRule
{
    public interface IEventRules : IDbCollection<IEventRule>
    {
        IEventRule CreateEventRule(string eventSource, string comparisonMode, int value, string eventType);
    }
}
