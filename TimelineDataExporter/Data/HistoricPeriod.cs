using System.Collections.Generic;

using static System.Collections.Generic.Dictionary<string, TimelineDataExporter.Data.TimelineEvent>;

namespace TimelineDataExporter.Data
{
    public class HistoricPeriod
    {
        public bool Contains(string title)
        {
            return TimelineEvents.ContainsKey(title.ToLower());
        }

        public TimelineEvent Get(string title)
        {
            return TimelineEvents[title.ToLower()];
        }

        public void Add(TimelineEvent newEvent)
        {
            if (!Contains(newEvent.Title))
            {
                TimelineEvents.Add(newEvent.Title.ToLower(), newEvent);
                return;
            }

            TimelineEvents[newEvent.Title.ToLower()] = newEvent;
        }

        public bool Remove(string title)
        {
            if (Contains(title))
            {
                TimelineEvents.Remove(title.ToLower());
                return true;
            }

            return false;
        }

        public Enumerator GetEnumerator()
        {
            return TimelineEvents.GetEnumerator();
        }

        // Do not use TimelineEvents directly, it is only publicly exposed because of Json serialization...
        public Dictionary<string, TimelineEvent> TimelineEvents { get; set; } = new Dictionary<string, TimelineEvent>();
    }
}
