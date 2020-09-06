using System.Windows;

namespace TimelineDataExporter.Windows
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void OnNewEntryButtonClicked(object sender, RoutedEventArgs e)
        {
            var newDataEntryWindow = new NewDataEntryWindow();
            newDataEntryWindow.Show();
        }
    }
}
