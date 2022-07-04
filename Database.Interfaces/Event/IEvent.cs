using Database.Interfaces.Common;

namespace Database.Interfaces.Event
{
    public interface IEvent : IEntity
    {
        long TimestampTicks { get; }
        string EventType { get; }
        string WorkstationId { get; }
        string Description { get; }
    }
}