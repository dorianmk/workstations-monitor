using AdminClientApp.ViewModels.Common;
using Common.DataTransfer.DataPackets.AdminClient;
using DataTransfer.Interfaces;

namespace AdminClientApp.ViewModels.Startup
{
    public class LoginViewModel : BindableBase
    {
        private IClientConnection Connection { get; }

        private string login;
        public string Login
        {
            get => login;
            set
            {
                SetProperty(ref login, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string password;
        public string Password
        {
            private get => password;
            set
            {
                password = value;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LoginCommand { get; }

        internal LoginViewModel(IClientConnection connection)
        {
            Connection = connection;
            Connection.Connected += (s, e) =>
            {
                LoginCommand.RaiseCanExecuteChanged(true);
            };
            Connection.Stopped += (s, e) =>
            {
                LoginCommand.RaiseCanExecuteChanged(true);
            };
            LoginCommand = new RelayCommand(DoLogin, CanLogin);
        }

        private void DoLogin(object obj)
        {
            Connection.Server.Write(new LoginRequestPacket(Login, Password));
        }

        private bool CanLogin(object obj) => Connection.IsConnected && !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(password);

    }
}
