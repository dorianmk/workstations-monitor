using Database.Interfaces.Migration;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.Migration
{
    internal abstract class Migration : IMigration, IMongoEntity
    {
        public ObjectId Id { get; private set; }
        public int Number { get; private set; }

        protected Migration(int number)
        {
            Number = number;
        }

        public abstract void Apply();       

        public string GetId() => Id.ToString();
    }
}
