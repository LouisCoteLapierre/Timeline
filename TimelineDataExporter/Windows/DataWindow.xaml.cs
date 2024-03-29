﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TimelineDataExporter.Containers;
using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;
using TimelineDataExporter.Models;
using TimelineDataExporter.Sorting.Comparers;

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

            UpdateNumberOfEntries();
        }

        // WPF Callbacks
        private void OnHistoricPeriodComboBoxInitialized(object sender, EventArgs e)
        {
            InitializeComboBox(HistoricPeriodComboBox, typeof(TimelineHistoricPeriod));
            HistoricPeriodComboBox.SelectedIndex = (int)TimelineHistoricPeriod.Contemporary;
        }

        private void OnDataGridInitialized(object sender, EventArgs e)
        {
            // Bind the grid to the events in the model. Whenever we modify the model, the data grid is updated as well.
            DataGrid.ItemsSource = HistoricPeriodsModel.Instance.AllEventsList;
            DataGrid.AutoGeneratedColumns += OnDataGridColumnsGenerated;

            // Custom sort for the Start and End categories, since they are stored as strings, they do not get sorted properly like ints
            DataGrid.Sorting += OnDataGridSort;
        }

        private void OnDataGridColumnsGenerated(object sender, EventArgs e)
        {
            foreach (var column in DataGrid.Columns)
            {
                var header = (string)column.Header;
                if (typeof(DataGridTextColumn) == column.GetType())
                {
                    var textColumn = (DataGridTextColumn)column;
                    if (textColumn != null && string.Compare(header, "LastModified") == 0)
                    {
                        textColumn.Binding.StringFormat = "yyyy/MM/dd";
                    }
                }
               
                if (   string.Compare(header, "Description") == 0
                    || string.Compare(header, "Works") == 0
                    || string.Compare(header, "RelatedLinks") == 0)
                {
                    column.MaxWidth = 150;
                }
                else if (string.Compare(header, "WikiLink") == 0)
                {
                    column.MaxWidth = 40;
                }
            }
        }

        private void OnDataGridSort(object sender, DataGridSortingEventArgs e)
        {
            var column = e.Column;

            // Only use the custom sort for Start and End
            if (!column.Header.Equals("Start") && !column.Header.Equals("End"))
            {
                e.Handled = false;
                return;
            }

            // Inform the datagrid that we handle sort for Start and End
            e.Handled = true;

            // Make sure the direction of the column is properly updated
            var direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            column.SortDirection = direction;

            // Setup our comparer
            var comparer = new StringDateComparer();

            comparer.Direction = direction;
            comparer.CompareStart = column.Header.Equals("Start");
            comparer.CompareEnd = column.Header.Equals("End");

            // Use the comparer on our ItemsSource
            var lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);
            lcv.CustomSort = comparer;
        }

        private void OnCreateAndUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            var historicPeriod = (TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), HistoricPeriodComboBox.SelectedValue.ToString());

            var title = TitleTextBox.Text;

            var works = new List<string>();
            ParseText(WorksTextBox.Text, ref works, '/');

            var links = new List<string>();
            ParseText(RelatedLinksTextBox.Text, ref links, '/');

            var now = DateTime.Now;
            LastModifiedLabel.Content = now.ToString();

            var newTimelineEvent = new TimelineEvent()
            {
                Title = title,
                Description = DescriptionTextBox.Text,
                Geography = GeographyTextBox.Text,
                Start = StartTextBox.Text,
                End = EndTextBox.Text,
                Period = historicPeriod,
                Type = TypeTextBox.Text,
                WikiLink = WikiLinkTextBox.Text,
                Works = FormattedList<string>.CreateFormattedList(works),
                RelatedLinks = FormattedList<string>.CreateFormattedList(links),
                LastModified = now
            };

            // Update the model
            HistoricPeriodsModel.Instance.AddEntry(newTimelineEvent);

            UpdateNumberOfEntries();
        }

        private void OnClearDataButtonClicked(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            GeographyTextBox.Text = string.Empty;
            StartTextBox.Text = string.Empty;
            EndTextBox.Text = string.Empty;
            TypeTextBox.Text = string.Empty;
            WikiLinkTextBox.Text = string.Empty;
            LastModifiedLabel.Content = string.Empty;
            WorksTextBox.Text = string.Empty;
            RelatedLinksTextBox.Text = string.Empty;

            HistoricPeriodComboBox.SelectedIndex = 0;
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
                UpdateDataCreationSection(currentlySelectedTimelineEvent);
            }
        }

        private void UpdateDataCreationSection(TimelineEvent timelineEvent)
        {
            if (timelineEvent == null)
            {
                return;
            }

            // Feed the UI from the timeline event data
            TitleTextBox.Text = timelineEvent.Title;
            DescriptionTextBox.Text = timelineEvent.Description;
            GeographyTextBox.Text = timelineEvent.Geography;
            StartTextBox.Text = timelineEvent.Start;
            EndTextBox.Text = timelineEvent.End;
            HistoricPeriodComboBox.SelectedIndex = (int)timelineEvent.Period;
            TypeTextBox.Text = timelineEvent.Type;
            WikiLinkTextBox.Text = timelineEvent.WikiLink;
            LastModifiedLabel.Content = timelineEvent.LastModified.ToString();

            // Build a tag text from the tags of the event
            WorksTextBox.Text = BuildTextFromList(timelineEvent.Works, "/").ToString();
            RelatedLinksTextBox.Text = BuildTextFromList(timelineEvent.RelatedLinks, "/").ToString();
        }

        private void OnDataWindowKeyDown(object sender, KeyEventArgs args)
        {
            // Those keys are reserved for the data grid
            if (   args.Key == Key.Back
                || args.Key == Key.Delete)
            {
                var selectedEvent = (TimelineEvent)DataGrid.SelectedItem;
                if (selectedEvent != null)
                {
                    HistoricPeriodsModel.Instance.RemoveEntry(selectedEvent);
                }

                UpdateNumberOfEntries();
            }
            else if (args.Key >= Key.A && args.Key <= Key.Z)
            {
                // Scroll to the first element of that letter
                var timelineEvent = HistoricPeriodsModel.Instance.GetFirstEventForLetter(args.Key);
                if (timelineEvent != null)
                {
                    DataGrid.SelectedItem = timelineEvent;
                    DataGrid.UpdateLayout();
                    DataGrid.ScrollIntoView(DataGrid.SelectedItem);

                    UpdateDataCreationSection(DataGrid.SelectedItem as TimelineEvent);
                }
            }
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

        private StringBuilder BuildTextFromList(FormattedList<string> texts, string separator)
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

        private void InitializeComboBox(ComboBox comboBox, Type enumType)
        {
            // Add an item for each enum category
            foreach (var categoryName in Enum.GetNames(enumType))
            {
                comboBox.Items.Add(categoryName);
            }

            // Always the first selected index by default
            comboBox.SelectedIndex = 0;
        }

        private void UpdateNumberOfEntries()
        {
            int numberOfEntries = DataGrid.Items.Count;
            NumberOfEntries.Content = "Number of Entries: " + numberOfEntries;
        }
    }
}
