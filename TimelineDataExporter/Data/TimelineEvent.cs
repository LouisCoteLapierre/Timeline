using System;
using System.Collections.Generic;

using TimelineDataExporter.Enums;
using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class TimelineEvent : IJsonSerializable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Geography { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public TimelineHistoricPeriod Period { get; set; }

        public string Type { get; set; }
        public string WikipediaLink { get; set; }

        public List<string> RelatedLinks { get; set; } = new List<string>();

        public DateTime? LastModified { get; set; }
    }
}
