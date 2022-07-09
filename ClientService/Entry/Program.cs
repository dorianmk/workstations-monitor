using Common.DataTransfer.DataPackets.Workstation;
using Common.Interfaces;
using Common.Service;
using Common.Tools;
using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using Diagnostics.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceProcess;
using WorkstationService.Entry.Components;
using WorkstationService.Entry.Service;
using WorkstationService.Entry.Settings;
using WorkstationService.Essential;
using WorkstationService.Essential.Id;
using WorkstationService.Essential.ProcessesInfo;
using WorkstationService.Essential.SystemInfo;

namespace WorkstationService.Entry
{
    static class Program
    {
        private static IServiceProvider ServiceProvider { get; }
       
        static Program()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AppSettings>();
            serviceCollection.AddSingleton<IWorker, MainWorker>();
            serviceCollection.AddSingleton<IDiagnosticsSettings, DiagnosticsSettings>();
            serviceCollection.AddSingleton<IDiagnostics, Diagnostics.Instances.Diagnostics>();
            serviceCollection.AddSingleton((p) => p.GetService<IDiagnostics>().SystemInfoProvider);
            serviceCollection.AddSingleton<IFactory<ConnectedPacket>, ConnectedPacketFactory>();
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<IEndpoint, TcpEndpoint>();
            serviceCollection.AddSingleton<IClientConnection, TcpClient>();
            serviceCollection.AddSingleton<IProcessesInfoWorker, ProcessesInfoWorker>();
            serviceCollection.AddSingleton<IFactory<SystemInfoDTO>, SystemInfoDTOFactory>();
            serviceCollection.AddAutoMapper(typeof(DataPacketsProfile));
            serviceCollection.AddSingleton<IFactory<IProcessInfo, ProcessInfoDTO>, ProcessInfoDtoFactory>();
            serviceCollection.AddSingleton<IAction<IData>, SendToServerAction>();
            serviceCollection.AddSingleton<IBuffer<IData>, Buffer<IData>>();
            serviceCollection.AddSingleton<IAction<string>, Logger>();
            serviceCollection.AddSingleton<IFactory<ServiceBase>, ServiceFactory>();
            serviceCollection.AddSingleton<IServiceRunner, Runner>();
            serviceCollection.AddSingleton<IWorkstationId, WorkstationId>();
        
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var runner = ServiceProvider.GetService<IServiceRunner>();
            runner.Run();
        }

    }
}
