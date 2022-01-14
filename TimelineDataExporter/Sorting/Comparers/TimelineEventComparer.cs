using System.Collections.Generic;
using TimelineDataExporter.Data;

namespace TimelineDataExporter.Sorting.Comparers
{
    public class TimelineEventComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            TimelineEvent timelineEventX = x as TimelineEvent;
            TimelineEvent timelineEventY = y as TimelineEvent;
            if (timelineEventX == null || timelineEventY == null)
            {
                return 0;
            }

            return string.Compare(timelineEventX.Title, timelineEventY.Title);
        }
    }
}
