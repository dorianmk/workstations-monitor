using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using Database.Interfaces.User;

namespace ServerService.Essential.AdminClient
{
    internal class UsersEntityToDtoConverter : IFactory<IUser, UserDTO>
    {
        public UserDTO Create(IUser param)
        {
            var result = new UserDTO();
            result.Id = param.GetId();
            result.Login = param.Login;
            return result;
        }
    }
}
