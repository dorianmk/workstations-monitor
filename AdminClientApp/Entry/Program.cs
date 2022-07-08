using System;
using System.IO;
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
using Common.DataTransfer.DataPackets.AdminClient;
using Common.DataTransfer.DataPackets.Workstation;
using Common.DataTransfer.RequestResponse;
using Common.Interfaces;
using DataTransfer.Interfaces;
using DataTransfer.Tcp;
using DataTransfer.Tcp.Serializers;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminClientApp.Entry
{
    static class Program
    {
        private static IServiceProvider ServiceProvider { get; }

        static Program()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>((p) => GetConfiguration());
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<IEndpoint, Endpoint>();
            serviceCollection.AddSingleton<IClientConnection, TcpClient>();
            serviceCollection.AddTransient<WorkstationViewModel>();
            serviceCollection.AddSingleton<IProvider<WorkstationViewModel>, WorkstationsProvider>();
            serviceCollection.AddSingleton<IFactory<EventDTO, EventViewModel>, EventViewModelFactory>();
            serviceCollection.AddSingleton<IFactory<EventRuleViewModel, EventRuleDTO>, EventRuleToDtoConverter>();
            serviceCollection.AddSingleton<IFactory<EventRuleDTO, EventRuleViewModel>, EventRuleToViewModelConverter>();
            serviceCollection.AddSingleton<IUpserter<WorkstationDTO, WorkstationViewModel>, WorkstationViewModelUpserter>();
            serviceCollection.AddSingleton<IUpserter<ProcessInfoDTO, ProcessInfoViewModel>, ProcessInfoViewModelUpserter>();
            serviceCollection.AddSingleton<IFactory<MapItemBase, MapItemDTO>, MapItemDTOFactory>();
            serviceCollection.AddSingleton<IFactory<MapViewModel, MapDTO>, MapDTOFactory>();
            serviceCollection.AddSingleton<IFactory<MapItemDTO, MapItemBase>, MapItemViewModelFactory>();
            serviceCollection.AddSingleton<IFactory<MapDTO, MapViewModel>, MapViewModelFactory>();
            serviceCollection.AddSingleton<IFactory<UserDTO, UserViewModel>, UserViewModelFactory>();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<IDialogCoordinator>((p) => DialogCoordinator.Instance);
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<IFactory<AdminPanelViewModel>, AdminPanelViewModelFactory>();
            serviceCollection.AddSingleton<AdminPanelViewModel>();          
            serviceCollection.AddAutoMapper(typeof(DataPacketsProfile));           
            serviceCollection.AddSingleton<IDataTwoWay>((p) => p.GetService<IClientConnection>().Server);
            serviceCollection.AddSingleton<IRequestResponse, RequestResponseManager>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [STAThread]
        static void Main()
        {
            var clientConnection = ServiceProvider.GetService<IClientConnection>();
            clientConnection.Stopped += (s, ex) =>
            {
                clientConnection.Start();
            };
            clientConnection.Start();

            var app = new App();
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            app.Run(mainWindow);
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
