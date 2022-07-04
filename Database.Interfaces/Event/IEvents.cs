using Database.Interfaces.Common;

namespace Database.Interfaces.Event
{
    public interface IEvents : IDbCollection<IEvent>
    {
        IEvent AddEvent(long timestampTicks, string eventType, string workstationId, string description);
    }
}
