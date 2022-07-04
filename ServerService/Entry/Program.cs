using AutoMapper;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Common.Service;
using Database.Interfaces;
using Database.Interfaces.Event;
using Database.Interfaces.EventRule;
using Database.Interfaces.Map;
using Database.Interfaces.Map.Item;
using Database.Interfaces.User;
using Database.Interfaces.Workstation;
using Database.MongoDB;
using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using ServerService.Common.Hashing;
using ServerService.Entry.Components;
using ServerService.Entry.Service;
using ServerService.Entry.Settings;
using ServerService.Essential.AdminClient;
using ServerService.Essential.AdminClient.Events;
using ServerService.Essential.AdminClient.Events.EventsOriginateRules;
using ServerService.Essential.Workstation;
using SimpleInjector;
using System.Collections.Generic;
using System.ServiceProcess;

namespace ServerService.Entry
{
    static class Program
    {
        static Container Container { get; }

        static Program()
        {
            Container = new Container();
            Container.Options.DefaultLifestyle = Lifestyle.Singleton;
            Container.Register<IWorker, MainWorker>();
            Container.Register<IWorkstationsManager, WorkstationsManager>();
            Container.Register<IAdminClients, AdminClients>();
            Container.Register<IDatabaseSettings, DatabaseSettings>();
            Container.Register<IDatabase, MongoDatabase>();
            Container.Register<ISerializer, JsonSerializer>();
            Container.Register<IServerSettings, TcpServerSettings>();
            Container.Register<IServerConnections, TcpServer>();
            Container.Register<IHashing, BCryptHashing>();
            Container.Register<IFactory<ServiceBase>, ServiceFactory>();
            Container.Register<IServiceRunner, Runner>();
            Container.Register<IFactory<IWorkstation, IWorkstationCache>, WorkstationCacheFactory>();
            Container.Register<IConfigurationProvider>(() => new MapperConfiguration(cfg => cfg.AddProfile<DataPacketsProfile>()));
            Container.Register(() => Container.GetInstance<IConfigurationProvider>().CreateMapper());
            Container.Register<IFactory<IWorkstationCache, WorkstationDTO>, WorkstationDTOFactory>();
            Container.Register<ILoginRequestValidator, LoginRequestValidator>();
            Container.Register(() => Container.GetInstance<IDatabase>().Maps);
            Container.Register<IFactory<MapItemDTO, IMapItem>, MapItemsDtoToEntityConverter>();
            Container.Register<IFactory<MapDTO, IMap>, MapsDtoToEntityConverter>();
            Container.Register<IFactory<IMapItem, MapItemDTO>, MapItemsEntityToDtoConverter>();
            Container.Register<IFactory<IMap, MapDTO>, MapsEntityToDtoConverter>();
            Container.Register(() => Container.GetInstance<IDatabase>().Users);
            Container.Register<IFactory<IUser, UserDTO>, UsersEntityToDtoConverter>();
            Container.Register(() => Container.GetInstance<IDatabase>().Events);
            Container.Register<IFactory<IEvent, EventDTO>, EventEntityToDtoConverter>();
            Container.Register(() => Container.GetInstance<IDatabase>().EventRules);
            Container.Register<IFactory<EventRuleDTO, IEventRule>, EventRuleDtoToEntityConverter>();
            Container.Register<IEventsOriginator<ProcessesEventOriginateRule,ProcessesInfoPacket>, EventsOriginator<ProcessesEventOriginateRule,ProcessesInfoPacket>>();
            Container.Register<IFactory<List<ProcessesEventOriginateRule>>, ProcessesEventOriginateRulesFactory>();
            Container.Register<IFactory<ProcessesEventOriginateRule, EventRuleDTO>, EventRuleToDtoConverter>();
            Container.Verify();
        }

        static void Main(string[] args)
        {
            var runner = Container.GetInstance<IServiceRunner>();
            runner.Run();
        }
    }
}
