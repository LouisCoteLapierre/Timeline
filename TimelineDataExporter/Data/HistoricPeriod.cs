using System.Collections.Generic;
using System.Linq;
using static System.Collections.Generic.SortedDictionary<string, TimelineDataExporter.Data.TimelineEvent>;

namespace TimelineDataExporter.Data
{
    public class HistoricPeriod
    {
        public bool Contains(string title)
        {
            return TimelineEvents.ContainsKey(title);
        }

        public TimelineEvent Get(string title)
        {
            return TimelineEvents[title];
        }

        public void Add(TimelineEvent newEvent)
        {
            if (!Contains(newEvent.Title))
            {
                TimelineEvents.Add(newEvent.Title, newEvent);
                return;
            }

            TimelineEvents[newEvent.Title] = newEvent;
        }

        public bool Remove(string title)
        {
            if (Contains(title))
            {
                TimelineEvents.Remove(title);
                return true;
            }

            return false;
        }

        public Enumerator GetEnumerator()
        {
            return TimelineEvents.GetEnumerator();
        }

        // Do not use TimelineEvents directly, it is only publicly exposed because of Json serialization...
        public SortedDictionary<string, TimelineEvent> TimelineEvents { get; set; } = new SortedDictionary<string, TimelineEvent>();
    }
}
