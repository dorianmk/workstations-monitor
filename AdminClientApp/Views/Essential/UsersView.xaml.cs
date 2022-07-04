using System.Windows.Controls;

namespace AdminClientApp.Views.Essential
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            passwordBox1.Clear();
            passwordBox2.Clear();
        }
    }
}
