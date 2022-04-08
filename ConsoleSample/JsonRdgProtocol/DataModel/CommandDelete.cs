using System;
using System.Text;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class CommandDelete
    {
        private string Command = "DbDelete";
        private string Table { get; set; }
        private Int64 Id { get; set; }

        public void SetDestinationTable(string table)
        {
            Table = table;
        }

        public void SetRecordId(Int64 id)
        {
            Id = id;
        }

        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();
            query.Append("{");
            query.Append("\"Command\":\"" + Command + "\"");
            query.Append(", \"Table\":\"" + Table + "\"");
            query.Append(", \"Id\":" + Id);
            query.Append("}");
            return query.ToString();
        }
    }
}