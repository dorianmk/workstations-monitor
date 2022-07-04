using AdminClientApp.ViewModels.Essential.Workstations;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Events
{
    internal class EventViewModelFactory : IFactory<EventDTO, EventViewModel>
    {
        private IProvider<WorkstationViewModel> WorkstationsProvider { get; }

        public EventViewModelFactory(IProvider<WorkstationViewModel> workstationsProvider)
        {
            WorkstationsProvider = workstationsProvider;
        }

        public EventViewModel Create(EventDTO eventDTO)
        {          
            var workstationName = WorkstationsProvider.Items.First(x => x.Id.Equals(eventDTO.WorkstationId)).Name;
            return new EventViewModel(eventDTO.DateTime, eventDTO.EventType, eventDTO.WorkstationId, workstationName, eventDTO.Description);            
        }
    }
}
