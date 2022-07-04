using AdminClientApp.Entry;
using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Users;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using DataTransfer.Interfaces;
using System.Collections.ObjectModel;

namespace AdminClientApp.ViewModels.Essential
{
    public class UsersViewModel : BindableBase
    {
        private IClientConnection Connection { get; }
        private IFactory<UserDTO, UserViewModel> UserViewModelFactory { get; }
        private UserViewModel selectedMap;

        public ObservableCollection<UserViewModel> Users { get; }
        public UserViewModel SelectedUser
        {
            get => selectedMap;
            set => SetProperty(ref selectedMap, value);
        }

        internal UsersViewModel(IClientConnection connection, IFactory<UserDTO, UserViewModel> userViewModelFactory)
        {
            Connection = connection;
            UserViewModelFactory = userViewModelFactory;
            Users = new ObservableCollection<UserViewModel>();
            Connection.Server.OnRead += OnDataRead;
        }

        private void OnDataRead(object sender, IData readData)
        {
            if (readData is GetUsersPacket getUsers)
            {
                App.Current.Dispatcher.Invoke(() => Users.Clear());
                foreach (var item in getUsers.Users)
                {
                    var userVM = UserViewModelFactory.Create(item);
                    App.Current.Dispatcher.Invoke(() => Users.Add(userVM));
                }
            }
        }
    }
}
