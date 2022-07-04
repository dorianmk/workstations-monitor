using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.RequestResponse;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Events
{
    internal class EventsProvider
    {
        private IRequestResponse RequestResponse { get; }
        public IFactory<EventDTO, EventViewModel> EventViewModelFactory { get; }

        internal EventsProvider(IFactory<EventDTO, EventViewModel> eventViewModelFactory, IRequestResponse requestResponse)
        {
            EventViewModelFactory = eventViewModelFactory;
            RequestResponse = requestResponse;
        }

        internal List<EventViewModel> Get(DateTimeFilterMode dateTimeFilter)
        {
            var dateTimeFrom = GetDateTimeFrom(dateTimeFilter);
            var getEventsRequest = new GetEventsRequest(Guid.NewGuid(), dateTimeFrom);
            if (RequestResponse.TryGetResponse<GetEventsResponse>(getEventsRequest, out var response, TimeSpan.FromSeconds(5)))
            {
                return response.Events.OrderByDescending(x => x.DateTime).Select(x => EventViewModelFactory.Create(x)).ToList();
            }
            return new List<EventViewModel>();
        }

        private DateTime GetDateTimeFrom(DateTimeFilterMode dateTimeFilter)
        {
            switch (dateTimeFilter)
            {
                case DateTimeFilterMode.LastHour: return DateTime.Now.Subtract(TimeSpan.FromHours(1));
                case DateTimeFilterMode.Last12Hours: return DateTime.Now.Subtract(TimeSpan.FromHours(12));
                case DateTimeFilterMode.Last24Hours: return DateTime.Now.Subtract(TimeSpan.FromHours(24));
                case DateTimeFilterMode.Last7Days: return DateTime.Now.Subtract(TimeSpan.FromDays(7));
                case DateTimeFilterMode.Last30Days: return DateTime.Now.Subtract(TimeSpan.FromDays(30));
                case DateTimeFilterMode.AnyDate: return DateTime.MinValue;
                default: throw new NotImplementedException();
            }
        }

    }
}
