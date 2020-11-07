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
            DataWindow = new DataWindow();
            DataWindow.Show();
            DataWindow.Focus();
            DataWindow.Closed += OnDataWindowClosed;

            Hide();
        }

        private void OnDataWindowClosed(object sender, System.EventArgs e)
        {
            Show();
            Focus();
        }

        DataWindow DataWindow;
    }
}
