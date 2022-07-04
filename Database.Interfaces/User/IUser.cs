using Database.Interfaces.Common;

namespace Database.Interfaces.User
{
    public interface IUser : IEntity
    {
        string Login { get; }
        string PasswordHash { get; set; }
    }
}
