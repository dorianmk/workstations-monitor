using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using DataTransfer.Interfaces;

namespace AdminClientApp.ViewModels.Essential.Users
{
    internal class UserViewModelFactory : IFactory<UserDTO, UserViewModel>
    {
        private IClientConnection Connection { get; }

        public UserViewModelFactory(IClientConnection connection)
        {
            Connection = connection;
        }

        public UserViewModel Create(UserDTO param)
        {
            return new UserViewModel(param.Id, param.Login, Connection);
        }
    }
}
