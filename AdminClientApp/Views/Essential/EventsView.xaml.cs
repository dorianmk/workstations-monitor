using System.Windows.Controls;

namespace AdminClientApp.Views.Essential
{
    /// <summary>
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsView : UserControl
    {
        public EventsView()
        {
            InitializeComponent();
        }

        private void ListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var listView = sender as ListView;
            listView.SelectAll();
        }
    }
}
