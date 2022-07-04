using System.Windows.Controls;

namespace AdminClientApp.Views.Essential
{
    /// <summary>
    /// Interaction logic for EventsSettingsView.xaml
    /// </summary>
    public partial class EventsSettingsView : UserControl
    {
        public EventsSettingsView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as DataGrid).UnselectAll();
        }
    }
}
