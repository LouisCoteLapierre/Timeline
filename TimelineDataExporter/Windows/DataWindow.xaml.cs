using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TimelineDataExporter.Data;

namespace TimelineDataExporter.Windows
{
    /// <summary>
    /// Interaction logic for NewDataEntryWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();

            // Timeline Code
            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            DataGrid.CanUserAddRows = true;

            // Build rows from property names of the DataEntry, this ensures that any potential renames 
            // in code will be reflected in the data, no need to fumble with the wpf
            foreach (var propertyInfo in typeof(TimelineEvent).GetProperties())
            {
                var column = new DataGridTextColumn();

                var propertyName = propertyInfo.Name;

                column.Header = propertyName;
                column.Binding = new Binding(propertyName);

                DataGrid.Columns.Add(column);
            }
           
        }

        // WPF Callbacks
        private void OnCreateDataEntry(object sender, RoutedEventArgs e)
        {
            var newData = new TimelineEvent()
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                StartDate = StartDatePicker.SelectedDate,
                EndDate = EndDatePicker.SelectedDate
            };

            
        }
    }
}
