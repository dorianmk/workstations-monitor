using System;
using AdminClientApp.Entry.Components;
using AdminClientApp.Entry.Settings;
using AdminClientApp.ViewModels.Essential;
using AdminClientApp.ViewModels.Essential.Events;
using AdminClientApp.ViewModels.Essential.Maps;
using AdminClientApp.ViewModels.Essential.Users;
using AdminClientApp.ViewModels.Essential.Workstations;
using AdminClientApp.ViewModels.Essential.Workstations.Processes;
using AdminClientApp.ViewModels.Startup;
using AdminClientApp.Views.Startup;
using AutoMapper;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.DataPackets.Workstation;
using Common.DataTransfer.RequestResponse;
using Common.Interfaces;
using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using MahApps.Metro.Controls.Dialogs;
using SimpleInjector;

namespace AdminClientApp.Entry
{
    static class Program
    {
        private static Container Container { get; }

        static Program()
        {
            Container = new Container();
            Container.Options.DefaultLifestyle = Lifestyle.Singleton;
            Container.Register<ISerializer, JsonSerializer>();
            Container.Register<IEndpoint, Endpoint>();
            Container.Register<IClientConnection, TcpClient>();
            Container.Register<WorkstationViewModel>(Lifestyle.Transient);
            Container.Register<IProvider<WorkstationViewModel>, WorkstationsProvider>();
            Container.Register<IFactory<EventDTO, EventViewModel>, EventViewModelFactory>();
            Container.Register<IFactory<EventRuleViewModel, EventRuleDTO>, EventRuleToDtoConverter>();
            Container.Register<IFactory<EventRuleDTO, EventRuleViewModel>, EventRuleToViewModelConverter>();
            Container.Register<IUpserter<WorkstationDTO, WorkstationViewModel>, WorkstationViewModelUpserter>();
            Container.Register<IUpserter<ProcessInfoDTO, ProcessInfoViewModel>, ProcessInfoViewModelUpserter>();
            Container.Register<IFactory<MapItemBase, MapItemDTO>, MapItemDTOFactory>();
            Container.Register<IFactory<MapViewModel, MapDTO>, MapDTOFactory>();
            Container.Register<IFactory<MapItemDTO, MapItemBase>, MapItemViewModelFactory>();
            Container.Register<IFactory<MapDTO, MapViewModel>, MapViewModelFactory>();
            Container.Register<IFactory<UserDTO, UserViewModel>, UserViewModelFactory>();
            Container.Register<MainWindow>();
            Container.Register(() => DialogCoordinator.Instance);
            Container.Register<MainViewModel>();
            Container.Register<IServiceProvider>(() => Container);
            Container.Register<IFactory<AdminPanelViewModel>, AdminPanelViewModelFactory>();
            Container.Register<AdminPanelViewModel>();
            Container.Register<DataPacketsProfile>();
            Container.Register<IConfigurationProvider>(() => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DataPacketsProfile>();
                cfg.ConstructServicesUsing(Container.GetInstance);
            }));
            Container.Register(() => Container.GetInstance<IConfigurationProvider>().CreateMapper());
            Container.Register(()=> Container.GetInstance<IClientConnection>().Server);
            Container.Register<IRequestResponse, RequestResponseManager>();
        }

        [STAThread]
        static void Main()
        {
            var clientConnection = Container.GetInstance<IClientConnection>(); 
            clientConnection.Stopped += (s, ex) =>
            {
                clientConnection.Start();
            };
            clientConnection.Start();           

            var app = new App();
            var mainWindow = Container.GetInstance<MainWindow>();
            app.Run(mainWindow);
        }

    }
}
