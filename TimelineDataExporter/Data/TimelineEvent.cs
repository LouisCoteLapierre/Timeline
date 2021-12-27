using System;

using TimelineDataExporter.Containers;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter.Data
{
    public class TimelineEvent
    {
        // The order of declaration here makes the order of columns in the datagrid view
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Geography { get; set; }
        public string Type { get; set; }
        public TimelineHistoricPeriod Period { get; set; }
        public string WikiLink { get; set; }

        public FormattedList<string> Works { get; set; } = new FormattedList<string>();
        public FormattedList<string> RelatedLinks { get; set; } = new FormattedList<string>();

        public DateTime? LastModified { get; set; }
    }
}
