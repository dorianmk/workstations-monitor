using System.Linq;
using Database.Interfaces;
using Database.Interfaces.Event;
using Database.Interfaces.EventRule;
using Database.Interfaces.Map;
using Database.Interfaces.Migration;
using Database.Interfaces.User;
using Database.Interfaces.Workstation;
using Database.MongoDB.Event;
using Database.MongoDB.EventRule;
using Database.MongoDB.Map;
using Database.MongoDB.Migration;
using Database.MongoDB.User;
using Database.MongoDB.Workstation;
using MongoDB.Driver;

namespace Database.MongoDB
{
    public class MongoDatabase : IDatabase
    {
        private IMongoClient Client { get; }
        private IMongoDatabase Db { get; }

        public IUsers Users { get; }
        public IWorkstations Workstations { get; }
        public IMigrations Migrations { get; }
        public IMaps Maps { get; }
        public IEvents Events { get; }
        public IEventRules EventRules { get; }

        public MongoDatabase(IDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            var dbName = MongoUrl.Create(settings.ConnectionString).DatabaseName;
            Db = Client.GetDatabase(dbName);
            Users = new Users(Db);
            Workstations = new Workstations(Db);
            Migrations = new Migrations(Db);
            Maps = new Maps(Db);
            Events = new Events(Db);
            EventRules = new EventRules(Db);
        }

        public bool CreateIfNeeded()
        {
            var collectionsExists = Db.ListCollectionNames().Any();
            if (collectionsExists)
            {
                Migrations.Update();
                return false;
            }
            else
            {
                Migrations.Seed();
                return true;
            }
        }


    }
}
