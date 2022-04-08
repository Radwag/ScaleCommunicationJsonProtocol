using System;
using System.Collections.Generic;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class ColumnRecordItem
    {
        public int Index { get; set; }

        public Int64 Id { get; set; }
        public string DbName { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public object Value { get; set; }
        public object Type { get; set; }

        public TableInfoItemType TableInfoItemType => (TableInfoItemType) Convert.ToInt32(Type);
        public int Tag { get; set; }
        public object Alterability { get; set; }
        public List<String> Values { get; set; }
        public List<String> ValuesEn { get; set; }
        public string Unit { get; set; }
        public object Min { get; set; }
        public object Max { get; set; }
        public object DecPlaces { get; set; }
        public string Table { get; set; }

        public IDictionary<int, string> ForeignValues { get; set; }
    }
}