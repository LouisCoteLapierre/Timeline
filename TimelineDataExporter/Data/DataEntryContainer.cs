using System.Collections.Generic;

using TimelineDataExporter.Serialization;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter.Data
{
    public class DataEntryContainer : IJsonSerializable
    {
        public List<DataEntry> DataEntries { get; set; }
        public TimelineHistoricPeriod HistoricPeriod { get; set; }
    }
}
