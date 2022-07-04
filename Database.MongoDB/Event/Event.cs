using Database.Interfaces.Event;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.Event
{
    internal class Event : IEvent, IMongoEntity
    {
        public ObjectId Id { get; private set; }
        public long TimestampTicks { get; private set; }
        public string EventType { get; private set; }
        public string WorkstationId { get; private set; }
        public string Description { get; private set; }

        internal Event(long timestampTicks, string eventType, string workstationId, string description)
        {
            TimestampTicks = timestampTicks;
            EventType = eventType;
            WorkstationId = workstationId;
            Description = description;
        }

        public string GetId() => Id.ToString();
    }
}
