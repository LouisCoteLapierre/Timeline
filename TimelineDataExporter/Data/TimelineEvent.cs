using System;

using TimelineDataExporter.Enums;
using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class TimelineEvent : IJsonSerializable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimelineHistoricPeriod HistoricPeriod { get; set; }
    }
}
