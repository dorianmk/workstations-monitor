using Common.Interfaces;
using Database.Interfaces;
using DataTransfer.Interfaces;
using ServerService.Common.Hashing;
using ServerService.Essential.AdminClient;
using ServerService.Essential.Workstation;

namespace ServerService.Entry
{
    internal class MainWorker : IWorker
    {
        private IDatabase Database { get; }
        private IServerConnections ServerConnections { get; }
        private IHashing Hashing { get; }
        private IAdminClients AdminClients { get; }
        private IWorkstationsManager WorkstationsManager { get; }

        public MainWorker(IDatabase database, IServerConnections serverConnections, IHashing hashing, IAdminClients adminClients, IWorkstationsManager workstationsManager)
        {
            Database = database;
            ServerConnections = serverConnections;
            Hashing = hashing;
            AdminClients = adminClients;
            WorkstationsManager = workstationsManager;
        }

        public void Start()
        {
            if (Database.CreateIfNeeded())
                SeedDatabase();
            ServerConnections.OnNewClient += (s, e) => OnNewClient(e);
            ServerConnections.Start();
        }

        private void SeedDatabase()
        {
            Database.Users.AddUser("root", Hashing.GetHash("pass"));
        }

        private void OnNewClient(IConnection client)
        {
            AdminClients.OnNewClient(client);
            WorkstationsManager.OnNewClient(client);
        }
        
        public void Stop()
        {

        }
    }
}
