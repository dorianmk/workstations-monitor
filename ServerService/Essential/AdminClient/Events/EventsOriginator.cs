using Common.Enums;
using Common.Interfaces;
using Database.Interfaces.Event;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerService.Essential.AdminClient.Events
{

    internal class EventsOriginator<TRule, TInput> : IEventsOriginator<TRule, TInput>
         where TRule : IEventOriginateRule<TInput>
    {
        private object lockObj { get; } = new object();
        private List<TRule> EventOriginateRules { get; }
        private IEvents Events { get; }
        private IFactory<List<TRule>> EventOriginateRulesFactory { get; }

        public EventsOriginator(IEvents events, IFactory<List<TRule>> eventOriginateRulesFactory)
        {
            Events = events;
            EventOriginateRulesFactory = eventOriginateRulesFactory;
            EventOriginateRules = eventOriginateRulesFactory.Create();
        }

        public void Create(TInput inputArgument)
        {
            lock (lockObj)
            {
                foreach (var eventOriginateRule in EventOriginateRules)
                    if (eventOriginateRule.IsFulfilled(inputArgument, out var newEvent))
                        Events.AddEvent(newEvent.Timestamp.Ticks, newEvent.EventType.ToString(), newEvent.WorkstationId, newEvent.Description);
            }
        }

        public void ReloadRules()
        {
            lock (lockObj)
            {
                EventOriginateRules.Clear();
                EventOriginateRules.AddRange(EventOriginateRulesFactory.Create());
            }
        }

        public List<TRule> GetRules() => EventOriginateRules.ToList();
    }

    internal interface IEventOriginateRule<TInput>
    {
        bool IsFulfilled(TInput inputArgument, out NewEvent newEvent);
    }

    internal class NewEvent
    {
        internal DateTime Timestamp { get; }
        internal EventType EventType { get; }
        internal string WorkstationId { get; }
        internal string Description { get; }

        internal NewEvent(DateTime timestamp, EventType eventType, string workstationId, string description)
        {
            Timestamp = timestamp;
            EventType = eventType;
            WorkstationId = workstationId;
            Description = description;
        }
    }

}
