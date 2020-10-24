using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;
using TimelineDataExporter.Models;

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
            DataGrid.CanUserAddRows = false;
            DataGrid.CanUserDeleteRows = false;
            DataGrid.CanUserReorderColumns = false;

            // Build columns from property names of the DataEntry, this ensures that any potential renames 
            // in code will be reflected in the data, no need to fumble with the wpf
            foreach (var propertyInfo in typeof(TimelineEvent).GetProperties())
            {
                var column = new DataGridTextColumn();

                var propertyName = propertyInfo.Name;

                column.Header = propertyName;
                column.Binding = new Binding(propertyName);

                DataGrid.Columns.Add(column);
            }

            // Add an item for each enum category
            foreach (var categoryName in Enum.GetNames(typeof(TimelineHistoricPeriod)))
            {
                HistoricPeriodComboBox.Items.Add(categoryName);
            }

            // Add rows from the DataModel

        }

        // WPF Callbacks
        private void OnCreateDataEntry(object sender, RoutedEventArgs e)
        {
            var historicPeriod = (TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), HistoricPeriodComboBox.SelectedValue.ToString());

            var title = TitleTextBox.Text;

            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(title));
            var ulongHash = BitConverter.ToUInt64(hashed, 0);

            // Check if there is already an entry for that title, if so, do not create another entry
            if (DataModel.Instance.HistoricPeriods[historicPeriod].ContainsKey(ulongHash))
            {
                // Todo warning popup
                return;
            }
                
            var newData = new TimelineEvent()
            {
                Title = title,
                Description = DescriptionTextBox.Text,
                StartDate = StartDatePicker.SelectedDate,
                EndDate = EndDatePicker.SelectedDate,
                HistoricPeriod = historicPeriod
            };

            DataGrid.Items.Add(newData);

            // Update the DataModel with the new timeline event
            DataModel.Instance.HistoricPeriods[historicPeriod].Add(ulongHash, newData);
        }
    }
}
