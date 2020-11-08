using System;
using System.Collections.Generic;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter.Models
{
    public class DataModel
    {
        // Singleton pour l'instant? ça va faire la job, le scope de cette app est petit
        public static DataModel Instance
        {
            get
            {
                return instance;
            }
        }

        public Dictionary<TimelineHistoricPeriod, Dictionary<string, TimelineEvent>> HistoricPeriods { get; set; } = new Dictionary<TimelineHistoricPeriod, Dictionary<string, TimelineEvent>>();

        private DataModel() { }
        static DataModel() { }

        public void Initialize()
        {
            foreach (var value in Enum.GetValues(typeof(TimelineHistoricPeriod)))
            {
                var enumValue = (TimelineHistoricPeriod)value;
                if (HistoricPeriods.ContainsKey(enumValue))
                {
                    if (HistoricPeriods[enumValue] == null)
                    {
                        HistoricPeriods[enumValue] = new Dictionary<string, TimelineEvent>();
                    }

                }
                else 
                {
                    HistoricPeriods.Add(enumValue, new Dictionary<string, TimelineEvent>());
                }
            }
        }

        // Returns true if the entry was created, false if it was updated
        public bool AddEntry(TimelineEvent newEvent)
        {
            // Title to lowercase
            var lowerCaseTitle = newEvent.Title.ToLower();

            bool eventExists = false;
            // Check if there is an entry for that tile in all categories
            foreach (var periodToEventsPair in Instance.HistoricPeriods)
            {
                if (periodToEventsPair.Value.ContainsKey(lowerCaseTitle))
                {
                    var existingEvent = periodToEventsPair.Value[lowerCaseTitle];
                    // If there is and the current category is different, delete the other entry
                    if (existingEvent.HistoricPeriod != newEvent.HistoricPeriod)
                    {
                        periodToEventsPair.Value.Remove(lowerCaseTitle);
                    }

                    eventExists = true;
                }
            }

            if (!eventExists)
            {
                Instance.HistoricPeriods[newEvent.HistoricPeriod].Add(lowerCaseTitle, null);
            }

            // Update the DataModel with the new timeline event
            Instance.HistoricPeriods[newEvent.HistoricPeriod][lowerCaseTitle] = newEvent;

            return !eventExists;
        }

        // Returns true if an entry was removed
        public bool RemoveEntry(string title)
        {
            // Lowercase the title
            var lowerCaseTitle = title.ToLower();

            foreach (var periodToEventsPair in Instance.HistoricPeriods)
            {
                if (periodToEventsPair.Value.ContainsKey(lowerCaseTitle))
                {
                    periodToEventsPair.Value.Remove(lowerCaseTitle);
                    return true;
                }
            }

            return false;
        }

        private static readonly DataModel instance = new DataModel();
    }
}
