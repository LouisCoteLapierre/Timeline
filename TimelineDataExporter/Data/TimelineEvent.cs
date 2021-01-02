using System;
using System.Collections.Generic;

using TimelineDataExporter.Enums;
using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class TimelineEvent : IJsonSerializable
    {
        // The order of declaration here makes the order of columns in the datagrid view
        public string Title { get; set; }
        public string Description { get; set; }

        public List<string> Works { get; set; } = new List<string>();

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Geography { get; set; }
        public string Type { get; set; }

        public List<string> RelatedLinks { get; set; } = new List<string>();

        public string WikiLink { get; set; }

        public TimelineHistoricPeriod Period { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
