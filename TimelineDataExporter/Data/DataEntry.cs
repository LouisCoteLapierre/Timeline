﻿using System;

using TimelineDataExporter.Serialization;

namespace TimelineDataExporter.Data
{
    public class DataEntry : IJsonSerializable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
