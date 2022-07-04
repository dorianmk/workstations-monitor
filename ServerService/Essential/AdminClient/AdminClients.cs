using System.Collections.Generic;
using System.Linq;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Database.Interfaces.Event;
using Database.Interfaces.EventRule;
using Database.Interfaces.Map;
using Database.Interfaces.User;
using DataTransfer.Interfaces;
using ServerService.Common.Hashing;
using ServerService.Essential.AdminClient.Events;
using ServerService.Essential.AdminClient.Events.EventsOriginateRules;
using ServerService.Essential.Workstation;

namespace ServerService.Essential.AdminClient
{
    internal interface IAdminClients
    {
        void OnNewClient(IConnection clientSource);
    }

    internal class AdminClients : IAdminClients
    {
        private IMaps Maps { get; }
        private IUsers Users { get; }
        private IHashing Hashing { get; }
        private IFactory<MapDTO, IMap> MapsDtoToEntityConverter { get; }
        private IFactory<IMap, MapDTO> MapsEntityToDtoConverter { get; }
        private IFactory<IUser, UserDTO> UsersEntityToDtoConverter { get; }
        private List<IDataWriter> ConnectedAdmins { get; }
        private IWorkstationsManager WorkstationsManager { get; }
        private IFactory<IWorkstationCache, WorkstationDTO> WorkstationDTOFactory { get; }
        private ILoginRequestValidator LoginRequestValidator { get; }
        private IEvents Events { get; }
        private IEventRules EventRules { get; }
        private IFactory<IEvent, EventDTO> EventEntityToDtoConverter { get; }
        private IEventsOriginator<ProcessesEventOriginateRule, ProcessesInfoPacket> EventsOriginator { get; }
        private IFactory<EventRuleDTO, IEventRule> EventRuleDtoToEntityConverter { get; }
        private IFactory<ProcessesEventOriginateRule, EventRuleDTO> EventRuleToDtoConverter { get; }

        public AdminClients(
                IMaps maps,
                IUsers users,
                IHashing hashing,
                IFactory<MapDTO, IMap> mapsDtoToEntityConverter,
                IFactory<IMap, MapDTO> mapsEntityToDtoConverter,
                IFactory<IUser, UserDTO> usersEntityToDtoConverter,
                IWorkstationsManager workstationsManager,
                IFactory<IWorkstationCache, WorkstationDTO> workstationDTOFactory,
                ILoginRequestValidator loginRequestValidator,
                IEvents events,
                IFactory<IEvent, EventDTO> eventEntityToDtoConverter,
                IEventsOriginator<ProcessesEventOriginateRule, ProcessesInfoPacket> eventsOriginator,
                IEventRules eventRules,
                IFactory<EventRuleDTO, IEventRule> eventRuleDtoToEntityConverter,
                IFactory<ProcessesEventOriginateRule, EventRuleDTO> eventRuleToDtoConverter)
        {
            Maps = maps;
            Users = users;
            Hashing = hashing;
            MapsDtoToEntityConverter = mapsDtoToEntityConverter;
            MapsEntityToDtoConverter = mapsEntityToDtoConverter;
            UsersEntityToDtoConverter = usersEntityToDtoConverter;
            WorkstationsManager = workstationsManager;
            WorkstationDTOFactory = workstationDTOFactory;
            LoginRequestValidator = loginRequestValidator;
            Events = events;
            EventEntityToDtoConverter = eventEntityToDtoConverter;
            WorkstationsManager.WorkstationChanged += OnWorkstationChanged;
            WorkstationsManager.WorkstationDataRead += WorkstationsManager_WorkstationDataRead;
            ConnectedAdmins = new List<IDataWriter>();
            EventsOriginator = eventsOriginator;
            EventRules = eventRules;
            EventRuleToDtoConverter = eventRuleToDtoConverter;
            EventRuleDtoToEntityConverter = eventRuleDtoToEntityConverter;
        }

        public void OnNewClient(IConnection client)
        {
            client.OnRead += OnDataRead;
        }

        private void OnDataRead(object source, IData readData)
        {
            var client = source as IConnection;
            if (readData is LoginRequestPacket loginRequest)
            {
                var isValid = LoginRequestValidator.IsValid(loginRequest);
                client.Write(new LoginAnswerPacket(isValid));
                if (isValid)
                {
                    ConnectedAdmins.Add(client);
                    SendCurrentWorkstations(client);
                    SendCurrentMaps(client);
                    SendCurrentUsers(client);
                    client.Stopped -= Client_Stopped;
                    client.Stopped += Client_Stopped;
                }
            }
            else if (readData is SaveMapsPacket saveMaps)
            {
                var mapEntities = saveMaps.Maps.Select(x => MapsDtoToEntityConverter.Create(x)).ToList();
                Maps.ReplaceCollection(mapEntities);
            }
            else if (readData is ChangePasswordPacket changePassword)
            {
                var user = Users.GetOne(changePassword.Id);
                user.PasswordHash = Hashing.GetHash(changePassword.Password);
                Users.AddOrReplace(user);
            }
            else if (readData is LogoutPacket logoutPacket)
            {
                ConnectedAdmins.Remove(client);
            }
            else if (readData is GetEventsRequest getEventsRequest)
            {
                var eventsEntities = Events.FindAll(x => x.TimestampTicks >= getEventsRequest.From.Ticks);
                var eventsDto = eventsEntities.Select(x => EventEntityToDtoConverter.Create(x)).ToList();
                var response = new GetEventsResponse(getEventsRequest.Id, eventsDto);
                client.Write(response);
            }
            else if (readData is SaveEventRulesPacket saveEventRules)
            {
                var eventRulesEntities = saveEventRules.EventRules.Select(x => EventRuleDtoToEntityConverter.Create(x)).ToList();
                EventRules.ReplaceCollection(eventRulesEntities);
                EventsOriginator.ReloadRules();
            }
            else if (readData is GetEventRulesRequest getEventRulesRequest)
            {
                var eventRules = EventsOriginator.GetRules();
                var eventRulesDto = eventRules.Select(x => EventRuleToDtoConverter.Create(x)).ToList();
                var response = new GetEventRulesResponse(getEventRulesRequest.Id, eventRulesDto);
                client.Write(response);
            }
            else
            {
                client.OnRead -= OnDataRead;
            }
        }

        private void SendCurrentWorkstations(IDataWriter client)
        {
            foreach (var item in WorkstationsManager.Workstations.Values)
            {
                var workstationDTO = WorkstationDTOFactory.Create(item);
                client.Write(workstationDTO);
            }
        }

        private void SendCurrentMaps(IDataWriter client)
        {
            var packet = new GetMapsPacket();
            packet.Maps = Maps.GetAll().Select(x => MapsEntityToDtoConverter.Create(x)).ToList();
            client.Write(packet);
        }

        private void SendCurrentUsers(IDataWriter client)
        {
            var packet = new GetUsersPacket();
            packet.Users = Users.GetAll().Select(x => UsersEntityToDtoConverter.Create(x)).ToList();
            client.Write(packet);
        }

        private void OnWorkstationChanged(object sender, IWorkstationCache workstationCache)
        {
            var workstationDTO = WorkstationDTOFactory.Create(workstationCache);
            foreach (var item in ConnectedAdmins)
                item.Write(workstationDTO);
        }

        private void WorkstationsManager_WorkstationDataRead(object sender, IData data)
        {
            var workstation = sender as IWorkstationCache;
            if (data is ProcessesInfoPacket processesInfoPacket)
            {
                processesInfoPacket.WorkstationId = workstation.Workstation.GetId();
                EventsOriginator.Create(processesInfoPacket);
                foreach (var item in ConnectedAdmins)
                    item.Write(processesInfoPacket);
            }
            else if (data is SystemInfoDTO systemInfo)
            {
                workstation.SystemInfo = systemInfo;
            }
        }

        private void Client_Stopped(object sender, System.Exception e)
        {
            var client = sender as IConnection;
            ConnectedAdmins.Remove(client);
        }
    }
}
