using System.Collections.Generic;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter.Models
{
    public class DataModel
    {
        // Singleton pour l'instant? ça va faire la job, le scope de cette app est petit
        public static DataModel Instance
        {
            get
            {
                return instance;
            }
        }

        public Dictionary<TimelineHistoricPeriod, DataEntryContainer> HistoricPeriods { get; set; } = new Dictionary<TimelineHistoricPeriod, DataEntryContainer>();

        private DataModel() { }
        static DataModel() { }

        private static readonly DataModel instance = new DataModel();
        
    }
}
