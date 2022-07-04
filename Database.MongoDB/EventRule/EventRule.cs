using Database.Interfaces.EventRule;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.EventRule
{
    internal class EventRule : IEventRule, IMongoEntity
    {
        public ObjectId Id { get; private set; }
        public string EventSource { get; private set; }
        public string ComparisonMode { get; private set; }
        public int Value { get; private set; }
        public string EventType { get; private set; }

        internal EventRule(string eventSource, string comparisonMode, int value, string eventType)
        {
            Id = ObjectId.GenerateNewId();
            EventSource = eventSource;
            ComparisonMode = comparisonMode;
            Value = value;
            EventType = eventType;
        }

        public string GetId() => Id.ToString();
    }
}
