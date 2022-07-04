using Database.Interfaces.EventRule;
using Database.MongoDB.Common;
using MongoDB.Driver;

namespace Database.MongoDB.EventRule
{
    internal class EventRules : DbCollectionBase<IEventRule, EventRule>, IEventRules
    {
        internal EventRules(IMongoDatabase db)
                    : base(db, "eventRules")
        {
        }

        public IEventRule CreateEventRule(string eventSource, string comparisonMode, int value, string eventType)
        {
            var eventRule = new EventRule(eventSource, comparisonMode, value, eventType);
            return eventRule;
        }
    }
}
