using System;
using System.Collections.Generic;
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
        }

        // WPF Callbacks
        private void OnHistoricPeriodComboBoxInitialized(object sender, EventArgs e)
        {
            // Add an item for each enum category
            foreach (var categoryName in Enum.GetNames(typeof(TimelineHistoricPeriod)))
            {
                HistoricPeriodComboBox.Items.Add(categoryName);
            }
        }

        private void OnDataGridInitialized(object sender, EventArgs e)
        {
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

            // Add rows from the DataModel
            foreach (var historicPeriodEventsDictionary in DataModel.Instance.HistoricPeriods)
            {
                foreach (var timelineEvent in historicPeriodEventsDictionary.Value)
                {
                    DataGrid.Items.Add(timelineEvent.Value);
                }
            }
        }

        private void OnCreateAndUpdateTimelineEvent(object sender, RoutedEventArgs e)
        {
            var historicPeriod = (TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), HistoricPeriodComboBox.SelectedValue.ToString());

            var title = TitleTextBox.Text;

            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(title));
            var ulongHash = BitConverter.ToUInt64(hashed, 0);

            // Create a list of tags from the tags text box
            var tags = new List<string>();
            foreach (var tag in TagsTextBox.Text.Split(',', ' '))
            {
                if (tag.Length == 0)
                {
                    continue;
                }

                tags.Add(tag);
            }

            var newData = new TimelineEvent()
            {
                Title = title,
                Description = DescriptionTextBox.Text,
                GeographicOrigin = GeographicOriginTextBox.Text,
                StartDate = StartDatePicker.SelectedDate,
                EndDate = EndDatePicker.SelectedDate,
                HistoricPeriod = historicPeriod,
                Type = TypeTextBox.Text,
                WikipediaLink = WikiLinkTextBox.Text,
                Tags = tags
            };

            // Check if there is already an entry for that title, if not, create it
            if (!DataModel.Instance.HistoricPeriods[historicPeriod].ContainsKey(ulongHash))
            {
                DataModel.Instance.HistoricPeriods[historicPeriod].Add(ulongHash, null);
            }
            else
            {
                // Todo : make this better, probably with a DataGrid that's not linked to all of the data
                // but is only a view of what we currently see
                int i = 0;
                foreach (var item in DataGrid.Items)
                {
                    var timelineEvent = (TimelineEvent)item;
                    if (timelineEvent != null && timelineEvent.Title == newData.Title)
                    {
                        DataGrid.Items.RemoveAt(i);
                        break;
                    }

                    i++;
                }
            }

            DataGrid.Items.Add(newData);

            // Update the DataModel with the new timeline event
            DataModel.Instance.HistoricPeriods[historicPeriod][ulongHash] = newData;
        }

        private void OnDataGridCellChanged(object sender, EventArgs e)
        {
            if (DataGrid.SelectedItems.Count > 1 || DataGrid.CurrentCell.Item.GetType() != typeof(TimelineEvent))
            {
                // popup saying we do not support multi selection
                return;
            }

            var lastSelectedTimelineEvent = (TimelineEvent)DataGrid.SelectedItem;
            var currentlySelectedTimelineEvent = (TimelineEvent)DataGrid.CurrentCell.Item;
            
            if (lastSelectedTimelineEvent != currentlySelectedTimelineEvent && currentlySelectedTimelineEvent != null)
            {
                // Feed the UI from the timeline event data
                TitleTextBox.Text = currentlySelectedTimelineEvent.Title;
                DescriptionTextBox.Text = currentlySelectedTimelineEvent.Description;
                GeographicOriginTextBox.Text = currentlySelectedTimelineEvent.GeographicOrigin;
                StartDatePicker.SelectedDate = currentlySelectedTimelineEvent.StartDate;
                EndDatePicker.SelectedDate = currentlySelectedTimelineEvent.EndDate;
                HistoricPeriodComboBox.SelectedIndex = (int)currentlySelectedTimelineEvent.HistoricPeriod;
                TypeTextBox.Text = currentlySelectedTimelineEvent.Type;
                WikiLinkTextBox.Text = currentlySelectedTimelineEvent.WikipediaLink;

                // Build a tag text from the tags of the event
                var stringBuilder = new StringBuilder();
                foreach (var tag in currentlySelectedTimelineEvent.Tags)
                {
                    if (stringBuilder.Length != 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(tag);
                }
                TagsTextBox.Text = stringBuilder.ToString();
            }
        }
    }
}
