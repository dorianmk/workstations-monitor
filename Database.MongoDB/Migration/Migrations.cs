using Database.Interfaces.Migration;
using Database.MongoDB.Migration.MigrationsToApply;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace Database.MongoDB.Migration
{
    internal class Migrations : IMigrations
    {
        private IEnumerable<Migration> MigrationsToApply
        {
            get
            {
                yield return new InitMigration(1);
            }
        }

        private IMongoCollection<Migration> Collection { get; }
       
        internal Migrations(IMongoDatabase db)
        {
            Collection = db.GetCollection<Migration>("_migrations");
        }

        public int GetLastMigrationNumber() => Collection.AsQueryable().MaxAsync(x => x.Number).Result;

        public void Seed()
        {
            foreach (var item in MigrationsToApply)            
                Collection.InsertOne(item);            
        }

        public void Update()
        {
            var lastMigrationNumber = GetLastMigrationNumber();
            foreach (var item in MigrationsToApply)
            {
                if (item.Number > lastMigrationNumber)
                {
                    item.Apply();
                    Collection.InsertOne(item);
                }
            }
        }
    }
}
