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

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TimelineHistoricPeriod HistoricPeriod { get; set; }

        public string Type { get; set; }
        public string WikipediaLink { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
        public List<string> RelatedLinks { get; set; } = new List<string>();

        public DateTime? LastModified { get; set; }
    }
}
