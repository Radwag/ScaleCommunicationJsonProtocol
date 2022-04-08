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
        public int Type { get; set; }

        public TableInfoItemType TableInfoItemType => (TableInfoItemType) Type;
        public int Tag { get; set; }
        public int Alterability { get; set; }
        public List<String> Values { get; set; }
        public List<String> ValuesEn { get; set; }
        public string Unit { get; set; }
        public object Min { get; set; }
        public object Max { get; set; }
        public int DecPlaces { get; set; }
        public string Table { get; set; }

        public IDictionary<int, string> ForeignValues { get; set; }
    }
}