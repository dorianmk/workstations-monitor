using Common.DataTransfer.DataPackets.AdminClient;
using Common.Enums;
using Common.Interfaces;
using Database.Interfaces.Event;
using System;

namespace ServerService.Essential.AdminClient.Events
{
    internal class EventEntityToDtoConverter : IFactory<IEvent, EventDTO>
    {
        public EventDTO Create(IEvent @event)
        {
            if (Enum.TryParse(@event.EventType, out EventType eventType))
            {
                return new EventDTO()
                {
                    DateTime = new DateTime(@event.TimestampTicks),
                    WorkstationId = @event.WorkstationId,
                    EventType = eventType,
                    Description = @event.Description
                };
            }
            throw new InvalidCastException();

        }
    }
}
