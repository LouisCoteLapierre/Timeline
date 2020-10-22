using System.Collections.Generic;

using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class DataEntryContainer : IJsonSerializable
    {
        public List<DataEntry> DataEntries { get; set; }
    }
}
