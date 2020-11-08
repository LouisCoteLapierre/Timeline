using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

            // By default, all new timeline events will go into None.txt unless the user specified a different
            // historic period
            HistoricPeriodComboBox.SelectedIndex = 0;
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
            foreach (var historicPeriodEventsDictionary in HistoricPeriodsModel.Instance.HistoricPeriods)
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

            var tags = new List<string>();
            ParseText(TagsTextBox.Text, ref tags, ',', ' ');

            var links = new List<string>();
            ParseText(RelatedLinksTextBox.Text, ref links, ',');

            var newTimelineEvent = new TimelineEvent()
            {
                Title = title,
                Description = DescriptionTextBox.Text,
                Geography = GeographyTextBox.Text,
                StartDate = StartDatePicker.SelectedDate,
                EndDate = EndDatePicker.SelectedDate,
                HistoricPeriod = historicPeriod,
                Type = TypeTextBox.Text,
                WikipediaLink = WikiLinkTextBox.Text,
                Tags = tags,
                RelatedLinks = links
            };

            // Update the model
            bool eventExists = !HistoricPeriodsModel.Instance.AddEntry(newTimelineEvent);
            // If we get true to return, it means a new entry was created, to know if the entry exists
            // already, we invert that bool
            if (eventExists)
            {
                RemoveEventFromDataGrid(title);
            }

            DataGrid.Items.Add(newTimelineEvent);
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
                GeographyTextBox.Text = currentlySelectedTimelineEvent.Geography;
                StartDatePicker.SelectedDate = currentlySelectedTimelineEvent.StartDate;
                EndDatePicker.SelectedDate = currentlySelectedTimelineEvent.EndDate;
                HistoricPeriodComboBox.SelectedIndex = (int)currentlySelectedTimelineEvent.HistoricPeriod;
                TypeTextBox.Text = currentlySelectedTimelineEvent.Type;
                WikiLinkTextBox.Text = currentlySelectedTimelineEvent.WikipediaLink;

                // Build a tag text from the tags of the event
                TagsTextBox.Text = BuildTextFromList(currentlySelectedTimelineEvent.Tags, ", ").ToString();
                RelatedLinksTextBox.Text = BuildTextFromList(currentlySelectedTimelineEvent.RelatedLinks, ", ").ToString();
            }
        }

        private void OnDataGridKeyDown(object sender, KeyEventArgs args)
        {
            // Check if the key press was delete or backspace, if so, delete the selected entry (check the title)
            // If not those keys, don't do anything
            if (args.Key != Key.Back && args.Key != Key.Delete)
            {
                return;
            }

            HistoricPeriodsModel.Instance.RemoveEntry(TitleTextBox.Text);
            RemoveEventFromDataGrid(TitleTextBox.Text);
        }

        // Helper methods
        private void ParseText(string text, ref List<string> separatedTexts, params char[] separators)
        {
            foreach (var splitText in text.Split(separators))
            {
                if (splitText.Length == 0)
                {
                    continue;
                }

                separatedTexts.Add(splitText);
            }
        }

        private StringBuilder BuildTextFromList(List<string> texts, string separator)
        {
            var stringBuilder = new StringBuilder();
            foreach (var text in texts)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append(separator);
                }

                stringBuilder.Append(text);
            }
            return stringBuilder;
        }

        private void RemoveEventFromDataGrid(string title)
        {
            // Todo : make this better, probably with a DataGrid that's not linked to all of the data
            // but is only a view of what we currently see
            int i = 0;
            foreach (var item in DataGrid.Items)
            {
                var timelineEvent = (TimelineEvent)item;
                if (timelineEvent != null && timelineEvent.Title == title)
                {
                    DataGrid.Items.RemoveAt(i);
                    break;
                }

                i++;
            }
        }
    }
}
