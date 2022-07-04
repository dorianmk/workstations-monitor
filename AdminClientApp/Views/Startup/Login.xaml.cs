using System.Windows;
using System.Windows.Controls;
using AdminClientApp.ViewModels.Startup;

namespace AdminClientApp.Views.Startup
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public Login()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Focus();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password= ((PasswordBox)sender).Password;
        }
    }
}
