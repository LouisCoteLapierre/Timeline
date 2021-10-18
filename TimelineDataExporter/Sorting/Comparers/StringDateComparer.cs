using System;
using System.Collections;
using System.ComponentModel;

using TimelineDataExporter.Data;

namespace TimelineDataExporter.Sorting.Comparers
{
    public class StringDateComparer : IComparer
    {
        public bool CompareStart { private get; set; }
        public bool CompareEnd { private get; set; }
        public ListSortDirection Direction { private get; set; }

        public int Compare(object x, object y)
        {
            TimelineEvent timelineEventX = (TimelineEvent)x;
            TimelineEvent timelineEventY = (TimelineEvent)y;

            string stringX = CompareStart ? timelineEventX.Start : CompareEnd ? timelineEventX.End : string.Empty;
            stringX = stringX.Replace(" ", string.Empty);

            string stringY = CompareStart ? timelineEventY.Start : CompareEnd ? timelineEventY.End : string.Empty;
            stringY = stringY.Replace(" ", string.Empty);

            // Check against empty string
            bool isXEmpty = stringX.Equals(string.Empty);
            bool isYEmpty = stringY.Equals(string.Empty);

            if (isXEmpty && isYEmpty)
            {
                return string.Compare(timelineEventX.Title, timelineEventY.Title);
            }
            if (isXEmpty && !isYEmpty)
            {
                return Direction == ListSortDirection.Ascending ? - 1 : 1;
            }
            if (isYEmpty && !isXEmpty)
            {
                return Direction == ListSortDirection.Ascending ? 1 : -1;
            }

            // Check with the "Present" value, which means this is right now, so it's the greatest value possible
            bool isXPresent = stringX.Equals("Present");
            bool isYPresent = stringY.Equals("Present");

            if (isXPresent && isYPresent)
            {
                return string.Compare(timelineEventX.Title, timelineEventY.Title);
            }
            else if (isXPresent && !isYPresent)
            {
                return Direction == ListSortDirection.Ascending ? 1 : -1;
            }
            else if (!isXPresent && isYPresent)
            {
                return Direction == ListSortDirection.Ascending ? -1 : 1;
            }

            // Parse the string to get the actual int value which represents a year
            int xYear = int.MinValue;
            bool xParseSuccess = int.TryParse(stringX, out xYear);

            int yYear = int.MinValue;
            bool yParseSuccess = int.TryParse(stringY, out yYear);
 
            if (!xParseSuccess && yParseSuccess)
            {
                return Direction == ListSortDirection.Ascending ? -1 : 1;
            }

            if (!yParseSuccess && xParseSuccess)
            {
                return Direction == ListSortDirection.Ascending ? 1 : -1;
            }

            if (xYear == yYear)
            {
                return string.Compare(timelineEventX.Title, timelineEventY.Title);
            }

            if (xYear > yYear)
            {
                return Direction == ListSortDirection.Ascending ? 1 : -1;
            }
            else
            {
                return Direction == ListSortDirection.Ascending ? -1 : 1;
            }
        }
    }
}
