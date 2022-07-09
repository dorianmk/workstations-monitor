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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerService.Common.Hashing;
using ServerService.Entry.Components;
using ServerService.Entry.Service;
using ServerService.Entry.Settings;
using ServerService.Essential.AdminClient;
using ServerService.Essential.AdminClient.Events;
using ServerService.Essential.AdminClient.Events.EventsOriginateRules;
using ServerService.Essential.Workstation;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace ServerService.Entry
{
    static class Program
    {

        private static IServiceProvider ServiceProvider { get; }

        static Program()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>((p) => GetConfiguration());
            serviceCollection.AddSingleton<IWorker, MainWorker>();
            serviceCollection.AddSingleton<IWorkstationsManager, WorkstationsManager>();
            serviceCollection.AddSingleton<IAdminClients, AdminClients>();
            serviceCollection.AddSingleton<IDatabaseSettings, DatabaseSettings>();
            serviceCollection.AddSingleton<IDatabase, MongoDatabase>();
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<IServerSettings, TcpServerSettings>();
            serviceCollection.AddSingleton<IServerConnections, TcpServer>();
            serviceCollection.AddSingleton<IHashing, BCryptHashing>();
            serviceCollection.AddSingleton<IFactory<ServiceBase>, ServiceFactory>();
            serviceCollection.AddSingleton<IServiceRunner, Runner>();
            serviceCollection.AddSingleton<IFactory<IWorkstation, IWorkstationCache>, WorkstationCacheFactory>();       
            serviceCollection.AddSingleton<IFactory<IWorkstationCache, WorkstationDTO>, WorkstationDTOFactory>();
            serviceCollection.AddSingleton<ILoginRequestValidator, LoginRequestValidator>();
            serviceCollection.AddSingleton((p) => p.GetService<IDatabase>().Maps);
            serviceCollection.AddSingleton<IFactory<MapItemDTO, IMapItem>, MapItemsDtoToEntityConverter>();
            serviceCollection.AddSingleton<IFactory<MapDTO, IMap>, MapsDtoToEntityConverter>();
            serviceCollection.AddSingleton<IFactory<IMapItem, MapItemDTO>, MapItemsEntityToDtoConverter>();
            serviceCollection.AddSingleton<IFactory<IMap, MapDTO>, MapsEntityToDtoConverter>();
            serviceCollection.AddSingleton((p) => p.GetService<IDatabase>().Users);
            serviceCollection.AddSingleton<IFactory<IUser, UserDTO>, UsersEntityToDtoConverter>();
            serviceCollection.AddSingleton((p) => p.GetService<IDatabase>().Events);
            serviceCollection.AddSingleton<IFactory<IEvent, EventDTO>, EventEntityToDtoConverter>();
            serviceCollection.AddSingleton((p) => p.GetService<IDatabase>().EventRules);
            serviceCollection.AddAutoMapper(typeof(DataPacketsProfile));
            serviceCollection.AddSingleton<IFactory<EventRuleDTO, IEventRule>, EventRuleDtoToEntityConverter>();
            serviceCollection.AddSingleton<IEventsOriginator<ProcessesEventOriginateRule,ProcessesInfoPacket>, EventsOriginator<ProcessesEventOriginateRule,ProcessesInfoPacket>>();
            serviceCollection.AddSingleton<IFactory<List<ProcessesEventOriginateRule>>, ProcessesEventOriginateRulesFactory>();
            serviceCollection.AddSingleton<IFactory<ProcessesEventOriginateRule, EventRuleDTO>, EventRuleToDtoConverter>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var runner = ServiceProvider.GetService<IServiceRunner>();
            runner.Run();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}
