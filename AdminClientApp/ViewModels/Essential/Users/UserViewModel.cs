using AdminClientApp.ViewModels.Common;
using Common.DataTransfer.DataPackets.AdminClient;
using DataTransfer.Interfaces;
using System.Windows.Controls;

namespace AdminClientApp.ViewModels.Essential.Users
{
    public class UserViewModel : BindableBase
    {
        private string login;
        private string newPassword;
        private string newPasswordConfirmation;
        private IClientConnection Connection { get; }

        public string Id { get; }
        public string Login
        {
            get => login;
            private set => SetProperty(ref login, value);
        }
        public RelayCommand ChangePasswordCommand { get; }
        public RelayCommand NewPasswordChangedCommand { get; }
        public RelayCommand NewPasswordConfirmationChangedCommand { get; }

        internal UserViewModel(string id, string login, IClientConnection connection)
        {
            Id = id;
            Login = login;
            Connection = connection;
            ChangePasswordCommand = new RelayCommand(ChangePassword, CanChangePassword);
            NewPasswordChangedCommand = new RelayCommand(NewPasswordChanged);
            NewPasswordConfirmationChangedCommand = new RelayCommand(NewPasswordConfirmationChanged);
        }

        private void NewPasswordChanged(object obj)
        {
            OnPasswordChange(obj, ref newPassword);
        }

        private void NewPasswordConfirmationChanged(object obj)
        {
            OnPasswordChange(obj, ref newPasswordConfirmation);
        }

        private void OnPasswordChange(object obj, ref string password)
        {
            var passwordBox = obj as PasswordBox;
            password = passwordBox.Password;
            ChangePasswordCommand.RaiseCanExecuteChanged();
        }

        private bool CanChangePassword(object obj) => !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(newPasswordConfirmation) && newPassword.Equals(newPasswordConfirmation);
        private void ChangePassword(object obj)
        {
            var packet = new ChangePasswordPacket(Id, newPassword);
            Connection.Server.Write(packet);
            newPassword = null;
            newPasswordConfirmation = null;
            ChangePasswordCommand.RaiseCanExecuteChanged();
        }
    }
}
