using Common.Enums;
using System;

namespace AdminClientApp.ViewModels.Essential.Events
{
    public class EventViewModel
    {
        public DateTime DateTime { get; }
        public EventType EventType { get; }
        public string WorkstationId { get; }
        public string WorkstationName { get; }
        public string Description { get; }

        public EventViewModel(DateTime dateTime, EventType eventType, string workstationId, string workstationName, string description)
        {
            DateTime = dateTime;
            EventType = eventType;
            WorkstationId = workstationId;
            WorkstationName = workstationName;
            Description = description;
        }
    }
}
