using MongoDB.Bson;

namespace Database.MongoDB.Common
{
    internal interface IMongoEntity
    {
        ObjectId Id { get; }
    }
}
