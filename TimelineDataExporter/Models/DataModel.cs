using System;
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

        public Dictionary<TimelineHistoricPeriod, Dictionary<ulong, TimelineEvent>> HistoricPeriods { get; set; } = new Dictionary<TimelineHistoricPeriod, Dictionary<ulong, TimelineEvent>>();

        private DataModel() { }
        static DataModel() { }

        public void Initialize()
        {
            foreach (var value in Enum.GetValues(typeof(TimelineHistoricPeriod)))
            {
                var enumValue = (TimelineHistoricPeriod)value;
                if (HistoricPeriods.ContainsKey(enumValue))
                {
                    if (HistoricPeriods[enumValue] == null)
                    {
                        HistoricPeriods[enumValue] = new Dictionary<ulong, TimelineEvent>();
                    }

                }
                else 
                {
                    HistoricPeriods.Add(enumValue, new Dictionary<ulong, TimelineEvent>());
                }
            }
        }

        private static readonly DataModel instance = new DataModel();
    }
}
