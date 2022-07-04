using Database.Interfaces.Event;
using Database.MongoDB.Common;
using MongoDB.Driver;

namespace Database.MongoDB.Event
{
    internal class Events : DbCollectionBase<IEvent, Event>, IEvents
    {
        internal Events(IMongoDatabase db)
                  : base(db, "events")
        {
        }

        public IEvent AddEvent(long timestampTicks, string eventType, string workstationId, string description)
        {
            var @event = new Event(timestampTicks, eventType, workstationId, description);
            Add(@event);
            return @event;
        }
    }
}
