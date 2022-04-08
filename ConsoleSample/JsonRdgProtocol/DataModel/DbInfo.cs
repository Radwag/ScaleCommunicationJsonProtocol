using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class DbInfo
    {
        public string Table { get; set; }
        public int MaxCount { get; set; }
        public int Count { get; set; }
        public List<ColumnSpecification> Items { get; set; }
        [JsonIgnore] public List<List<ColumnRecordItem>> Records { get; set; } = new List<List<ColumnRecordItem>>();
    }
}