using System.Windows;

using TimelineDataExporter.Data;

namespace TimelineDataExporter.Windows
{
    /// <summary>
    /// Interaction logic for NewDataEntryWindow.xaml
    /// </summary>
    public partial class NewDataEntryWindow : Window
    {
        public NewDataEntryWindow()
        {
            InitializeComponent();
        }

        private void OnCreateDataEntry(object sender, RoutedEventArgs e)
        {
            var newData = new DataEntry()
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                StartDate = StartDatePicker.SelectedDate,
                EndDate = EndDatePicker.SelectedDate
            };


        }
    }
}
