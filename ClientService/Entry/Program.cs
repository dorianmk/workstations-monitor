using SimpleInjector;
using DataTransfer.Tcp;
using DataTransfer.Interfaces;
using Common.Service;
using Diagnostics.Interfaces;
using DataTransfer.Tcp.Serializers;
using AutoMapper;
using Common.Interfaces;
using Common.Tools;
using System.ServiceProcess;
using WorkstationService.Entry.Service;
using WorkstationService.Entry.Components;
using WorkstationService.Essential.Id;
using Common.DataTransfer.DataPackets.Workstation;
using WorkstationService.Entry.Settings;
using WorkstationService.Essential.ProcessesInfo;
using WorkstationService.Essential.SystemInfo;
using WorkstationService.Essential;

namespace WorkstationService.Entry
{
    static class Program
    {
        static Container Container { get; }

        static Program()
        {
            Container = new Container();
            Container.Options.DefaultLifestyle = Lifestyle.Singleton;
            Container.Register<IWorker, MainWorker>();
            Container.Register<IDiagnosticsSettings, DiagnosticsSettings>();
            Container.Register<IDiagnostics, Diagnostics.Instances.Diagnostics>();
            Container.Register(() => Container.GetInstance<IDiagnostics>().SystemInfoProvider);
            Container.Register<IFactory<ConnectedPacket>, ConnectedPacketFactory>();
            Container.Register<ISerializer, JsonSerializer>();
            Container.Register<IEndpoint, TcpEndpoint>();
            Container.Register<IClientConnection, TcpClient>();
            Container.Register<IProcessesInfoWorker, ProcessesInfoWorker>();
            Container.Register<IFactory<SystemInfoDTO>, SystemInfoDTOFactory>();
            Container.Register<IConfigurationProvider>(() => new MapperConfiguration(cfg => cfg.AddProfile<DataPacketsProfile>()));
            Container.Register(() => Container.GetInstance<IConfigurationProvider>().CreateMapper());
            Container.Register<IFactory<IProcessInfo, ProcessInfoDTO>, ProcessInfoDtoFactory>();
            Container.Register<IAction<IData>, SendToServerAction>();
            Container.Register<IBuffer<IData>, Buffer<IData>>();
            Container.Register<IAction<string>, Logger>();
            Container.Register<IFactory<ServiceBase>, ServiceFactory>();
            Container.Register<IServiceRunner, Runner>();
            Container.Register<IWorkstationId, WorkstationId>();
            Container.Verify();
        }

        static void Main(string[] args)
        {
            var runner = Container.GetInstance<IServiceRunner>();
            runner.Run();
        }

    }
}
