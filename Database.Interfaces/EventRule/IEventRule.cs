using Database.Interfaces.Common;

namespace Database.Interfaces.EventRule
{
    public interface IEventRule : IEntity
    {
        string EventSource { get; }
        string ComparisonMode { get; }
        int Value { get; }
        string EventType { get; }
    }
}
