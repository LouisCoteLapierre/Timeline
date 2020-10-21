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

        private void OnViewDataButtonClicked(object sender, RoutedEventArgs e)
        {
            var dataWindow = new DataWindow();
            dataWindow.Show();
        }
    }
}
