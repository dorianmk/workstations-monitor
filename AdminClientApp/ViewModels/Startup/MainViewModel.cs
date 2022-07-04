using AdminClientApp.Entry;
using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using DataTransfer.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace AdminClientApp.ViewModels.Startup
{
    public class MainViewModel : BindableBase
    {
        private IClientConnection Connection { get; }
        private IFactory<AdminPanelViewModel> AdminPanelViewModelFactory { get; }
        private IDialogCoordinator DialogCoordinator { get; }
        private LoginViewModel LoginViewModel { get; }

        private BindableBase selectedView;
        public BindableBase SelectedView
        {
            get => selectedView;
            private set
            {
                if (SetProperty(ref selectedView, value))
                    OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => SelectedView is AdminPanelViewModel;
        public RelayCommand LogoutCmd { get; }

        public MainViewModel(
            IClientConnection connection, 
            IFactory<AdminPanelViewModel> adminPanelViewModelFactory,
            IDialogCoordinator dialogCoordinator)
        {
            Connection = connection;
            Connection.Stopped += OnConnectionStopped;
            AdminPanelViewModelFactory = adminPanelViewModelFactory;
            DialogCoordinator = dialogCoordinator;
            LoginViewModel = new LoginViewModel(connection);
            SelectedView = LoginViewModel;
            if (Connection.IsConnected)
                OnServerConnected();
            else
                connection.Connected += (s, e) => OnServerConnected();
            LogoutCmd = new RelayCommand(Logout);
        }

        private void OnServerConnected()
        {
            Connection.Server.OnRead += (s, e) => OnDataRead(e);
        }

        private void OnConnectionStopped(object sender, System.Exception e)
        {
            if (IsLoggedIn)
                SelectedView = LoginViewModel;
        }

        private void OnDataRead(IData readData)
        {
            if (readData is LoginAnswerPacket loginAnswer)
            {
                if (loginAnswer.IsValid)
                {
                    SelectedView = AdminPanelViewModelFactory.Create();
                    App.Current.Dispatcher.InvokeAsync(() =>
                    {
                        LoginViewModel.Login = string.Empty;
                        LoginViewModel.Password = string.Empty;
                    });
                }
                else
                    DialogCoordinator.ShowMessageAsync(this, "Error", "Invalid login or password");
            }
        }

        private void Logout(object obj)
        {
            Connection.Server.Write(new LogoutPacket());
            SelectedView = LoginViewModel;
        }

    }

}
