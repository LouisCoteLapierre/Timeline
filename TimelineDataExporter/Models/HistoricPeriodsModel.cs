using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        private HistoricPeriodsModel() { }
        static HistoricPeriodsModel() { }

        // Check if we can have a Dictionary<Period, ObservableCollection> and use that binding in the DataWindow and have multiple tabs,
        // so multiple datagrid, each datagrid would bind its item to the corresponding ObservableCollection in the dictionary (one for each period)
        public Dictionary<TimelineHistoricPeriod, HistoricPeriod> HistoricPeriods { get; set; } = new Dictionary<TimelineHistoricPeriod, HistoricPeriod>();
        public ObservableCollection<TimelineEvent> AllEventsList { get; set; } = new ObservableCollection<TimelineEvent>();
        
        public void VerifyIntegrity()
        {
            foreach (var value in Enum.GetValues(typeof(TimelineHistoricPeriod)))
            {
                if (HistoricPeriods.TryGetValue((TimelineHistoricPeriod)value, out var historicPeriod))
                {
                    if (historicPeriod == null)
                    {
                        HistoricPeriods[(TimelineHistoricPeriod)value] = new HistoricPeriod();
                    }

                    continue;
                }

                HistoricPeriods.Add((TimelineHistoricPeriod)value, new HistoricPeriod());
            }
        }

        public void AddHistoricPeriod(TimelineHistoricPeriod historicPeriodEnum, HistoricPeriod historicPeriod)
        {
            if (historicPeriod == null)
            {
                return;
            }

            if (HistoricPeriods.ContainsKey(historicPeriodEnum))
            {
                HistoricPeriods[historicPeriodEnum] = historicPeriod;
            }
            else
            {
                HistoricPeriods.Add(historicPeriodEnum, historicPeriod);
            }

            foreach (var timelineEventPair in historicPeriod)
            {
                AllEventsList.Add(timelineEventPair.Value);
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
                    if (existingEvent.Period != newEvent.Period)
                    {
                        historicPeriodPair.Value.Remove(newEvent.Title);
                    }

                    // Todo : Check if instead of deleting and adding, we can just update the TimelineEvent
                    foreach (var timelineEvent in AllEventsList)
                    {
                        if (timelineEvent.Title.Equals(newEvent.Title))
                        {
                            AllEventsList.Remove(timelineEvent);
                            break;
                        }
                    }

                    eventExists = true;
                    break;
                }
            }

            Instance.HistoricPeriods[newEvent.Period].Add(newEvent);
            AllEventsList.Add(newEvent);

            return !eventExists;
        }

        // Returns true if an entry was removed
        public bool RemoveEntry(string title)
        {
            foreach (var historicPeriodPair in Instance.HistoricPeriods)
            {
                if (historicPeriodPair.Value.Remove(title))
                {
                    foreach (var timelineEvent in AllEventsList)
                    {
                        if (timelineEvent.Title.Equals(title))
                        {
                            AllEventsList.Remove(timelineEvent);
                            break;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private static readonly HistoricPeriodsModel instance = new HistoricPeriodsModel();
    }
}
