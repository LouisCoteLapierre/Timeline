using System.Collections.Generic;

using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class DataEntryContainer : IJsonSerializable
    {
        public List<TimelineEvent> TimelineEvents { get; set; }
    }
}
