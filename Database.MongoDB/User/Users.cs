using Database.Interfaces.User;
using Database.MongoDB.Common;
using MongoDB.Driver;

namespace Database.MongoDB.User
{
    internal class Users : DbCollectionBase<IUser, User>, IUsers
    {
        internal Users(IMongoDatabase db)
              : base(db, "users")
        {
        }

        public IUser AddUser(string login, string passwordHash)
        {
            var item = new User(login, passwordHash);
            Add(item);
            return item;
        }

    }
}
