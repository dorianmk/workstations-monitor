using Database.Interfaces.Common;

namespace Database.Interfaces.User
{
    public interface IUsers : IDbCollection<IUser>
    {
        IUser AddUser(string login, string passwordHash);
    }
}
