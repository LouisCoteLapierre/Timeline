using System;
using System.Collections.Generic;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter.Models
{
    public class HistoricPeriodsModel
    {
        // Singleton for now, the scope of this app is fairly small so this will do the trick
        public static HistoricPeriodsModel Instance
        {
            get
            {
                return instance;
            }
        }

        public Dictionary<TimelineHistoricPeriod, HistoricPeriod> HistoricPeriods { get; set; } = new Dictionary<TimelineHistoricPeriod, HistoricPeriod>();

        private HistoricPeriodsModel() { }
        static HistoricPeriodsModel() { }

        public void VerifyIntegrity()
        {
            foreach (var value in Enum.GetValues(typeof(TimelineHistoricPeriod)))
            {
                if (HistoricPeriods.ContainsKey((TimelineHistoricPeriod)value))
                {
                    continue;
                }

                HistoricPeriods.Add((TimelineHistoricPeriod)value, new HistoricPeriod());
            }
        }

        // Returns true if the entry was created, false if it was updated
        public bool AddEntry(TimelineEvent newEvent)
        {
            bool eventExists = false;
            // Check if there is an entry for that tile in all categories
            foreach (var historicPeriodPair in Instance.HistoricPeriods)
            {
                if (historicPeriodPair.Value.Contains(newEvent.Title))
                {
                    var existingEvent = historicPeriodPair.Value.Get(newEvent.Title);
                    // If there is and the current category is different, delete the other entry
                    if (existingEvent.HistoricPeriod != newEvent.HistoricPeriod)
                    {
                        historicPeriodPair.Value.Remove(newEvent.Title);
                    }

                    eventExists = true;
                }
            }

            Instance.HistoricPeriods[newEvent.HistoricPeriod].Add(newEvent);

            return !eventExists;
        }

        // Returns true if an entry was removed
        public bool RemoveEntry(string title)
        {
            foreach (var historicPeriodPair in Instance.HistoricPeriods)
            {
                if (historicPeriodPair.Value.Remove(title))
                {
                    return true;
                }
            }

            return false;
        }

        private static readonly HistoricPeriodsModel instance = new HistoricPeriodsModel();
    }
}
