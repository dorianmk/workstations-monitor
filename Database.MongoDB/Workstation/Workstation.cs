using Database.Interfaces.Workstation;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.Workstation
{
    internal class Workstation : IWorkstation, IMongoEntity
    {
        public ObjectId Id { get; private set; }

        public string Name { get; private set; }

        public string GetId() => Id.ToString();

        internal Workstation(string name)
        {
            Name = name;
        }
    }
}
