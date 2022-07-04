using Common.DataTransfer.RequestResponse;
using Common.Enums;
using DataTransfer.Interfaces;
using System;
using System.Collections.Generic;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class GetEventsRequest : IDataWithId
    {
        public Guid Id { get; private set; }
        public DateTime From { get; private set; }

        public GetEventsRequest(Guid id, DateTime from)
        {
            Id = id;
            From = from;
        }
    }

    public class GetEventsResponse : IDataWithId
    {
        public Guid Id { get; private set; }
        public List<EventDTO> Events { get; private set; }

        public GetEventsResponse(Guid id, List<EventDTO> events)
        {
            Id = id;
            Events = events;
        }
    }

    public class EventDTO
    {
        public DateTime DateTime { get; set; }
        public EventType EventType { get; set; }
        public string WorkstationId { get; set; }
        public string Description { get; set; }
    }

    public class SaveEventRulesPacket : IData
    {
        public List<EventRuleDTO> EventRules { get; set; }
    }

    public class GetEventRulesRequest : IDataWithId
    {
        public Guid Id { get; private set; }

        public GetEventRulesRequest(Guid id)
        {
            Id = id;
        }
    }

    public class GetEventRulesResponse : IDataWithId
    {
        public Guid Id { get; private set; }
        public List<EventRuleDTO> EventRules { get; private set; }

        public GetEventRulesResponse(Guid id, List<EventRuleDTO> eventRules)
        {
            Id = id;
            EventRules = eventRules;
        }
    }

    public class EventRuleDTO
    {
        public EventSource EventSource { get; set; }
        public ComparisonMode ComparisonMode { get; set; }
        public int Value { get; set; }
        public EventType EventType { get; set; }
    }



}
