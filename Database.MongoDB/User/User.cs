using Database.Interfaces.User;
using Database.MongoDB.Common;
using MongoDB.Bson;

namespace Database.MongoDB.User
{
    internal class User : IUser, IMongoEntity
    {
        public ObjectId Id { get; private set; }

        public string Login { get; private set; }

        public string PasswordHash { get; set; }

        public string GetId() => Id.ToString();

        internal User(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
