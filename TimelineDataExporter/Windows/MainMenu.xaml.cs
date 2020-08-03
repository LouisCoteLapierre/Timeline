using System.Windows;

namespace TimelineDataExporter.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void OnNewEntryButtonClicked(object sender, RoutedEventArgs e)
        {
            var newDataEntryWindow = new NewDataEntry();
            newDataEntryWindow.Show();
        }
    }
}
